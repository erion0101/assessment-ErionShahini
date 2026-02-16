// Watch page helper: get current video time (for the "Now" button).
function getVideoEl(arg) {
    if (typeof arg === 'string') {
        var el = document.getElementById(arg);
        if (el && el.tagName === 'VIDEO') return el;
        // Fallback: first video on the page (if id is missing or invalid)
        el = document.querySelector('.vl-video-player') || document.querySelector('video');
        return (el && el.tagName === 'VIDEO') ? el : null;
    }
    return (arg && arg.tagName === 'VIDEO') ? arg : null;
}

function getTextForSeekButton(btn) {
    if (!btn) return '';
    var row = btn.closest('.vl-note-item') || btn.closest('.vl-preview-item');
    if (!row) return (btn.textContent || '').trim();
    var textEl = row.querySelector('.vl-note-text') || row.querySelector('.vl-preview-text');
    if (!textEl) return (btn.textContent || '').trim();
    return (textEl.textContent || '').trim();
}

function ensureAnnotationOverlay(video) {
    if (!video) return null;
    var wrap = video.closest('.vl-video-wrap');
    if (!wrap) return null;
    var overlay = wrap.querySelector('.vl-active-annotation-overlay.is-js');
    if (!overlay) {
        overlay = document.createElement('div');
        overlay.className = 'vl-active-annotation-overlay is-js';
        overlay.style.display = 'none';
        wrap.appendChild(overlay);
    }
    return overlay;
}

function showAnnotationOverlay(video, sec, text) {
    if (!video) return;
    var overlay = ensureAnnotationOverlay(video);
    if (!overlay) return;
    var safeText = (text || 'Annotation').trim() || 'Annotation';
    var t = Number(sec) || 0;
    var m = Math.floor(t / 60);
    var s = Math.floor(t % 60).toString().padStart(2, '0');
    overlay.innerHTML = '<span class="vl-active-annotation-time">' + m + ':' + s + '</span><span class="vl-active-annotation-text">' + safeText + '</span>';
    overlay.style.display = 'inline-flex';
    clearTimeout(overlay.__hideTimer);
    overlay.__hideTimer = setTimeout(function () {
        overlay.style.display = 'none';
    }, 3000);
}
// Return time as string so Blazor reads it safely (double serialization may return 0).
window.videoLabGetCurrentTime = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el || typeof el.currentTime !== 'number') return '0';
    return String(el.currentTime);
};

// Seek: callable from Blazor (kept for the "Now" flow if needed).
window.videoLabSetCurrentTime = function (arg1, arg2) {
    var idOrEl = (typeof arg1 === 'string' || (arg1 && arg1.tagName)) ? arg1 : arg2;
    var sec = typeof arg1 === 'number' ? arg1 : (typeof arg2 === 'number' ? arg2 : Number(arg1) || Number(arg2) || 0);
    var el = getVideoEl(idOrEl);
    if (el && !isNaN(sec) && sec >= 0)
        el.currentTime = sec;
};

// Timestamp button click (data-seek): seek entirely in browser, without Blazor round-trip.
document.addEventListener('click', function (e) {
    if (!e.target || typeof e.target.closest !== 'function') return;
    var btn = e.target.closest('.vl-seek-btn');
    if (!btn) return;
    var sec = parseFloat(btn.getAttribute('data-seek'));
    if (isNaN(sec) || sec < 0) return;
    var video = getVideoEl('vl-watch-video');
    if (video) {
        video.currentTime = sec;
        showAnnotationOverlay(video, sec, getTextForSeekButton(btn));
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    }
}, true);

window.videoLabEnterFullscreen = async function (elementId) {
    var target = document.getElementById(elementId) || document.querySelector('.vl-video-wrap');
    if (!target || typeof target.requestFullscreen !== 'function') return false;
    if (document.fullscreenElement === target) return true;
    await target.requestFullscreen();
    return document.fullscreenElement === target;
};

window.videoLabExitFullscreen = async function () {
    if (!document.fullscreenElement || typeof document.exitFullscreen !== 'function') return true;
    await document.exitFullscreen();
    return !document.fullscreenElement;
};

window.videoLabIsFullscreen = function () {
    return !!document.fullscreenElement;
};

window.videoLabCanPiP = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el) return false;
    if (!document.pictureInPictureEnabled) return false;
    if (el.disablePictureInPicture) return false;
    return typeof el.requestPictureInPicture === 'function';
};

window.videoLabIsPiP = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el || !document.pictureInPictureElement) return false;
    return document.pictureInPictureElement === el;
};

window.videoLabEnterPiP = async function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!window.videoLabCanPiP(el)) return false;
    if (document.pictureInPictureElement === el) return true;

    if (document.pictureInPictureElement && typeof document.exitPictureInPicture === 'function') {
        await document.exitPictureInPicture();
    }

    await el.requestPictureInPicture();
    return document.pictureInPictureElement === el;
};

window.videoLabExitPiP = async function () {
    if (!document.pictureInPictureElement || typeof document.exitPictureInPicture !== 'function') return true;
    await document.exitPictureInPicture();
    return !document.pictureInPictureElement;
};

window.videoLabAttachTimeObserver = function (videoElementOrId, dotNetRef, callbackMethod, throttleMs) {
    var el = getVideoEl(videoElementOrId);
    if (!el || !dotNetRef || typeof dotNetRef.invokeMethodAsync !== 'function') return false;

    window.__videoLabTimeObservers = window.__videoLabTimeObservers || {};
    var key = el.id || '__default_video';

    if (window.__videoLabTimeObservers[key]) {
        var prev = window.__videoLabTimeObservers[key];
        el.removeEventListener('timeupdate', prev.timeupdate);
        el.removeEventListener('seeked', prev.seeked);
        el.removeEventListener('loadedmetadata', prev.loadedmetadata);
        delete window.__videoLabTimeObservers[key];
    }

    var minGap = Number(throttleMs);
    if (!Number.isFinite(minGap) || minGap < 50) minGap = 250;
    var lastCall = 0;
    var method = callbackMethod || 'OnVideoTimeUpdate';

    var throttled = function () {
        var now = Date.now();
        if (now - lastCall < minGap) return;
        lastCall = now;

        dotNetRef.invokeMethodAsync(method, Number(el.currentTime) || 0).catch(function () { });
    };

    // Trigger immediately when user seeks/clicks timestamps, not only during playback.
    var immediate = function () {
        lastCall = Date.now();
        dotNetRef.invokeMethodAsync(method, Number(el.currentTime) || 0).catch(function () { });
    };

    el.addEventListener('timeupdate', throttled);
    el.addEventListener('seeked', immediate);
    el.addEventListener('loadedmetadata', immediate);

    window.__videoLabTimeObservers[key] = {
        timeupdate: throttled,
        seeked: immediate,
        loadedmetadata: immediate
    };
    return true;
};

window.videoLabDetachTimeObserver = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el || !window.__videoLabTimeObservers) return;
    var key = el.id || '__default_video';
    var handlers = window.__videoLabTimeObservers[key];
    if (!handlers) return;
    el.removeEventListener('timeupdate', handlers.timeupdate);
    el.removeEventListener('seeked', handlers.seeked);
    el.removeEventListener('loadedmetadata', handlers.loadedmetadata);
    delete window.__videoLabTimeObservers[key];
};

window.videoLabAttachAnnotationOverlay = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el) return false;

    window.__videoLabAnnotationOverlay = window.__videoLabAnnotationOverlay || {};
    var key = el.id || '__default_video';

    if (window.__videoLabAnnotationOverlay[key]) {
        var prev = window.__videoLabAnnotationOverlay[key];
        el.removeEventListener('timeupdate', prev.timeupdate);
        el.removeEventListener('seeked', prev.seeked);
        delete window.__videoLabAnnotationOverlay[key];
    }

    var pickNearestAnnotation = function () {
        var annotationButtons = document.querySelectorAll('.vl-seek-btn[data-kind="annotation"][data-seek]');
        if (!annotationButtons || annotationButtons.length === 0) return null;
        var current = Number(el.currentTime) || 0;
        var best = null;
        for (var i = 0; i < annotationButtons.length; i++) {
            var b = annotationButtons[i];
            var sec = parseFloat(b.getAttribute('data-seek'));
            if (!Number.isFinite(sec)) continue;
            var diff = Math.abs(sec - current);
            if (diff > 1.5) continue;
            if (!best || diff < best.diff) {
                best = { sec: sec, diff: diff, text: getTextForSeekButton(b) };
            }
        }
        return best;
    };

    var onTick = function () {
        var hit = pickNearestAnnotation();
        if (!hit) return;
        showAnnotationOverlay(el, hit.sec, hit.text);
    };

    el.addEventListener('timeupdate', onTick);
    el.addEventListener('seeked', onTick);
    window.__videoLabAnnotationOverlay[key] = { timeupdate: onTick, seeked: onTick };
    return true;
};

window.videoLabDetachAnnotationOverlay = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el || !window.__videoLabAnnotationOverlay) return;
    var key = el.id || '__default_video';
    var handlers = window.__videoLabAnnotationOverlay[key];
    if (!handlers) return;
    el.removeEventListener('timeupdate', handlers.timeupdate);
    el.removeEventListener('seeked', handlers.seeked);
    delete window.__videoLabAnnotationOverlay[key];
};
