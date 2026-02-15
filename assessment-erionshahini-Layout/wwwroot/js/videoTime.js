// Për faqen Watch: merr kohën aktuale të videos (për butonin "Tani").
function getVideoEl(arg) {
    if (typeof arg === 'string') {
        var el = document.getElementById(arg);
        if (el && el.tagName === 'VIDEO') return el;
        // Fallback: videoja e parë e faqes (nëse id mungon ose është i gabuar)
        el = document.querySelector('.vl-video-player') || document.querySelector('video');
        return (el && el.tagName === 'VIDEO') ? el : null;
    }
    return (arg && arg.tagName === 'VIDEO') ? arg : null;
}
// Kthen kohën si string që Blazor ta lexojë pa humbje (serializimi double ndonjëherë jep 0).
window.videoLabGetCurrentTime = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el || typeof el.currentTime !== 'number') return '0';
    return String(el.currentTime);
};

// Seek: thirret nga Blazor (mbetet për "Tani" nëse duhet).
window.videoLabSetCurrentTime = function (arg1, arg2) {
    var idOrEl = (typeof arg1 === 'string' || (arg1 && arg1.tagName)) ? arg1 : arg2;
    var sec = typeof arg1 === 'number' ? arg1 : (typeof arg2 === 'number' ? arg2 : Number(arg1) || Number(arg2) || 0);
    var el = getVideoEl(idOrEl);
    if (el && !isNaN(sec) && sec >= 0)
        el.currentTime = sec;
};

// Klik mbi butonin e kohës (data-seek): vendos videon në atë sekondë – 100% në browser, pa Blazor.
document.addEventListener('click', function (e) {
    if (!e.target || typeof e.target.closest !== 'function') return;
    var btn = e.target.closest('.vl-seek-btn');
    if (!btn) return;
    var sec = parseFloat(btn.getAttribute('data-seek'));
    if (isNaN(sec) || sec < 0) return;
    var video = getVideoEl('vl-watch-video');
    if (video) {
        video.currentTime = sec;
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
        el.removeEventListener('timeupdate', window.__videoLabTimeObservers[key]);
        delete window.__videoLabTimeObservers[key];
    }

    var minGap = Number(throttleMs);
    if (!Number.isFinite(minGap) || minGap < 50) minGap = 250;
    var lastCall = 0;
    var method = callbackMethod || 'OnVideoTimeUpdate';

    var handler = function () {
        var now = Date.now();
        if (now - lastCall < minGap) return;
        lastCall = now;

        dotNetRef.invokeMethodAsync(method, Number(el.currentTime) || 0).catch(function () { });
    };

    el.addEventListener('timeupdate', handler);
    window.__videoLabTimeObservers[key] = handler;
    return true;
};

window.videoLabDetachTimeObserver = function (videoElementOrId) {
    var el = getVideoEl(videoElementOrId);
    if (!el || !window.__videoLabTimeObservers) return;
    var key = el.id || '__default_video';
    var handler = window.__videoLabTimeObservers[key];
    if (!handler) return;
    el.removeEventListener('timeupdate', handler);
    delete window.__videoLabTimeObservers[key];
};
