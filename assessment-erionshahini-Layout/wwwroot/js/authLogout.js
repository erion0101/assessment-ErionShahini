// Login from browser so API sets refresh_token cookie and stores it in DB.
window.loginFromBrowser = function (apiBaseUrl, email, password) {
    if (!apiBaseUrl || !email || !password)
        return Promise.resolve({ success: false, message: 'Missing parameters' });
    var url = (apiBaseUrl + '').replace(/\/$/, '') + '/api/auth/Login';
    return fetch(url, {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: email, password: password })
    }).then(function (r) {
        return r.json().then(function (data) {
            var token = data.accessToken || data.AccessToken || null;
            var ok = !!((data.success === true || data.Success === true) && token);
            return { success: ok, accessToken: token, message: data.message || data.Message || (r.ok ? null : 'Login failed') };
        }).catch(function () {
            return { success: false, message: 'Invalid response' };
        });
    }).catch(function (err) {
        return { success: false, message: err.message || 'Network error' };
    });
};

window.registerFromBrowser = function (apiBaseUrl, email, password) {
    if (!apiBaseUrl || !email || !password)
        return Promise.resolve({ success: false, message: 'Missing parameters' });

    var url = (apiBaseUrl + '').replace(/\/$/, '') + '/api/auth/Register';
    return fetch(url, {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: email, password: password })
    }).then(function (r) {
        return r.json().then(function (data) {
            var token = data.accessToken || data.AccessToken || null;
            var ok = !!((data.success === true || data.Success === true) && token);
            return { success: ok, accessToken: token, message: data.message || data.Message || (r.ok ? null : 'Register failed') };
        }).catch(function () {
            return { success: false, message: 'Invalid response' };
        });
    }).catch(function (err) {
        return { success: false, message: err.message || 'Network error' };
    });
};

// Called by Blazor after page refresh: browser sends HttpOnly refresh-token cookie to API and receives a new access token.
window.refreshTokenFromBrowser = function (apiBaseUrl) {
    if (!apiBaseUrl) return Promise.resolve({ success: false, accessToken: null });
    var url = (apiBaseUrl + '').replace(/\/$/, '') + '/api/auth/refresh';
    return fetch(url, {
        method: 'POST',
        credentials: 'include'
    }).then(function (r) {
        return r.json().then(function (data) {
            var token = data.accessToken || data.AccessToken || null;
            var ok = !!(r.ok && (data.success === true || data.Success === true) && token);
            return { success: ok, accessToken: token };
        }).catch(function () {
            return { success: false, accessToken: null };
        });
    }).catch(function () {
        return { success: false, accessToken: null };
    });
};

// Returns the refresh result (stored by script on load) and clears it so Blazor can read it.
window.getBlazorRefreshResult = function () {
    var r = window.__blazorRefreshResult;
    window.__blazorRefreshResult = null;
    return r;
};

// Called by Blazor so browser sends cookie and gets the response that clears it (logout-by-cookie).
window.logoutClearCookie = function (apiBaseUrl) {
    if (!apiBaseUrl) return Promise.resolve();
    return fetch((apiBaseUrl + '').replace(/\/$/, '') + '/api/auth/logout-by-cookie', {
        method: 'POST',
        credentials: 'include'
    });
};

// On page load: performs browser refresh (cookie is sent) and stores result for Blazor.
(function () {
    if (!window.__apiBaseUrl) return;
    window.refreshTokenFromBrowser(window.__apiBaseUrl).then(function (r) {
        window.__blazorRefreshResult = r;
    });
})();
