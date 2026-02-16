/**
 * Plyr-based video player with bookmarks (progress bar markers) and annotations (overlay).
 * For Blazor Server: init once in OnAfterRenderAsync(firstRender), destroy on dispose.
 */
(function () {
    'use strict';

    var ANNOTATION_DURATION_MS = 5000;
    var ANNOTATION_MATCH_THRESHOLD_SEC = 0.5;
    var TIME_UPDATE_THROTTLE_MS = 250;

    var instances = {};

    function getVideoEl(idOrEl) {
        if (typeof idOrEl === 'string') {
            var el = document.getElementById(idOrEl);
            return (el && el.tagName === 'VIDEO') ? el : null;
        }
        return (idOrEl && idOrEl.tagName === 'VIDEO') ? idOrEl : null;
    }

    function ensureAnnotationOverlay(wrap) {
        if (!wrap) return null;
        var overlay = wrap.querySelector('.vl-plyr-annotation-overlay');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.className = 'vl-plyr-annotation-overlay';
            overlay.setAttribute('aria-live', 'polite');
            overlay.style.display = 'none';
            wrap.appendChild(overlay);
        }
        return overlay;
    }

    function showAnnotationOverlay(wrap, timeSec, text) {
        var overlay = ensureAnnotationOverlay(wrap);
        if (!overlay) return;
        var safeText = (text || 'Annotation').trim() || 'Annotation';
        var t = Number(timeSec) || 0;
        var m = Math.floor(t / 60);
        var s = Math.floor(t % 60).toString().padStart(2, '0');
        overlay.innerHTML = '<span class="vl-plyr-annotation-time">' + m + ':' + s + '</span><span class="vl-plyr-annotation-text">' + escapeHtml(safeText) + '</span>';
        overlay.style.display = 'inline-flex';
        clearTimeout(overlay._hideTimer);
        overlay._hideTimer = setTimeout(function () {
            overlay.style.display = 'none';
        }, ANNOTATION_DURATION_MS);
    }

    function escapeHtml(str) {
        var div = document.createElement('div');
        div.textContent = str;
        return div.innerHTML;
    }

    function buildMarkersContainer(progressEl, duration, bookmarks) {
        var existing = progressEl.querySelector('.vl-plyr-markers');
        if (existing) existing.remove();

        var container = document.createElement('div');
        container.className = 'vl-plyr-markers';
        container.setAttribute('aria-hidden', 'true');

        if (!duration || duration <= 0 || !bookmarks || bookmarks.length === 0) {
            progressEl.appendChild(container);
            return container;
        }

        bookmarks.forEach(function (sec) {
            var left = Math.min(100, Math.max(0, (Number(sec) / duration) * 100));
            var dot = document.createElement('button');
            dot.type = 'button';
            dot.className = 'vl-plyr-marker';
            dot.style.left = left + '%';
            dot.setAttribute('data-seek', String(sec));
            dot.setAttribute('title', 'Jump to ' + formatTime(sec));
            container.appendChild(dot);
        });

        progressEl.appendChild(container);
        return container;
    }

    function formatTime(sec) {
        var t = Number(sec) || 0;
        var m = Math.floor(t / 60);
        var s = Math.floor(t % 60);
        return m + ':' + (s < 10 ? '0' : '') + s;
    }

    function findPlyrProgress(player) {
        if (!player || !player.elements) return null;
        var container = player.elements.container;
        if (!container) return null;
        return container.querySelector('.plyr__progress');
    }

    window.videoPlayerInit = function (options) {
        var containerId = options.containerId || 'vl-watch-player-wrap';
        var videoId = options.videoId || 'vl-watch-video';
        var src = options.src;
        var bookmarks = options.bookmarks || [];
        var annotations = options.annotations || [];
        var dotNetRef = options.dotNetRef;
        var timeUpdateCallback = options.timeUpdateCallback || 'OnVideoTimeUpdate';

        var container = document.getElementById(containerId);
        if (!container) return false;

        var video = document.getElementById(videoId);
        if (!video) {
            video = document.createElement('video');
            video.id = videoId;
            video.className = 'vl-video-player';
            video.setAttribute('preload', 'auto');
            video.setAttribute('playsinline', '');
            container.insertBefore(video, container.firstChild);
        }
        if (src) video.src = src;

        if (typeof window.Plyr === 'undefined') return false;

        var player = new window.Plyr(video, {
            controls: ['play-large', 'play', 'progress', 'current-time', 'duration', 'mute', 'volume', 'settings', 'fullscreen'],
            hideControls: false,
            seekTime: 10
        });

        var plyrContainer = player.elements.container;
        var wrap = plyrContainer;
        var lastTimeCall = 0;
        var activeAnnotationHideAt = 0;
        var currentAnnotationIndex = -1;

        function onTimeUpdate() {
            var t = Number(video.currentTime) || 0;

            if (dotNetRef && typeof dotNetRef.invokeMethodAsync === 'function') {
                var now = Date.now();
                if (now - lastTimeCall >= TIME_UPDATE_THROTTLE_MS) {
                    lastTimeCall = now;
                    dotNetRef.invokeMethodAsync(timeUpdateCallback, t).catch(function () {});
                }
            }

            for (var i = 0; i < annotations.length; i++) {
                var ann = annotations[i];
                var at = Number(ann.time);
                if (Math.abs(t - at) <= ANNOTATION_MATCH_THRESHOLD_SEC) {
                    if (currentAnnotationIndex !== i) {
                        currentAnnotationIndex = i;
                        activeAnnotationHideAt = Date.now() + ANNOTATION_DURATION_MS;
                        showAnnotationOverlay(wrap, at, ann.text);
                    }
                    return;
                }
            }
            if (Date.now() < activeAnnotationHideAt) return;
            currentAnnotationIndex = -1;
        }

        video.addEventListener('timeupdate', onTimeUpdate);
        video.addEventListener('seeked', onTimeUpdate);

        player.on('ready', function () {
            var progressEl = findPlyrProgress(player);
            if (progressEl) {
                var duration = video.duration;
                buildMarkersContainer(progressEl, duration, bookmarks);

                progressEl.addEventListener('click', function (e) {
                    var marker = e.target.closest('.vl-plyr-marker');
                    if (!marker) return;
                    var sec = parseFloat(marker.getAttribute('data-seek'));
                    if (Number.isFinite(sec) && sec >= 0) {
                        video.currentTime = sec;
                        e.preventDefault();
                        e.stopPropagation();
                    }
                });
            }
        });

        player.on('loadedmetadata', function () {
            var progressEl = findPlyrProgress(player);
            if (progressEl && bookmarks.length > 0) {
                buildMarkersContainer(progressEl, video.duration, bookmarks);
            }
        });

        instances[videoId] = {
            player: player,
            video: video,
            wrap: wrap,
            bookmarks: bookmarks,
            annotations: annotations,
            onTimeUpdate: onTimeUpdate,
            updateMarkers: function (newBookmarks) {
                this.bookmarks = newBookmarks || [];
                var progressEl = findPlyrProgress(player);
                if (progressEl) buildMarkersContainer(progressEl, video.duration, this.bookmarks);
            },
            updateAnnotations: function (newAnnotations) {
                this.annotations = newAnnotations || [];
            },
            getCurrentTime: function () {
                return String(video.currentTime || 0);
            },
            seekTo: function (sec) {
                if (Number.isFinite(sec) && sec >= 0) video.currentTime = sec;
            },
            destroy: function () {
                video.removeEventListener('timeupdate', onTimeUpdate);
                video.removeEventListener('seeked', onTimeUpdate);
                try {
                    player.destroy();
                } catch (err) {}
                delete instances[videoId];
            }
        };

        return true;
    };

    window.videoPlayerUpdateMarkers = function (videoId, bookmarks) {
        var inst = instances[videoId || 'vl-watch-video'];
        if (inst && inst.updateMarkers) inst.updateMarkers(bookmarks);
    };

    window.videoPlayerUpdateAnnotations = function (videoId, annotations) {
        var inst = instances[videoId || 'vl-watch-video'];
        if (inst && inst.updateAnnotations) inst.updateAnnotations(annotations);
    };

    window.videoPlayerGetCurrentTime = function (videoId) {
        var inst = instances[videoId || 'vl-watch-video'];
        return inst && inst.getCurrentTime ? inst.getCurrentTime() : '0';
    };

    window.videoPlayerSeekTo = function (videoId, seconds) {
        var inst = instances[videoId || 'vl-watch-video'];
        if (inst && inst.seekTo) inst.seekTo(seconds);
    };

    window.videoPlayerDestroy = function (videoId) {
        var id = videoId || 'vl-watch-video';
        var inst = instances[id];
        if (inst && inst.destroy) inst.destroy();
    };

    document.addEventListener('click', function (e) {
        var btn = e.target && e.target.closest && e.target.closest('.vl-seek-btn');
        if (!btn) return;
        var sec = parseFloat(btn.getAttribute('data-seek'));
        if (!Number.isFinite(sec) || sec < 0) return;
        window.videoPlayerSeekTo('vl-watch-video', sec);
        e.preventDefault();
        e.stopPropagation();
    }, true);
})();
