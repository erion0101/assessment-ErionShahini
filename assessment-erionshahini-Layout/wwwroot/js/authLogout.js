// Login nga browseri që API të vendosë refresh_token në cookie dhe të ruajë në DB.
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

// Thirret nga Blazor që browseri të dërgojë cookie-n dhe të marrë përgjigjen që e fshin (logout-by-cookie).
window.logoutClearCookie = function (apiBaseUrl) {
    if (!apiBaseUrl) return Promise.resolve();
    return fetch((apiBaseUrl + '').replace(/\/$/, '') + '/api/auth/logout-by-cookie', {
        method: 'POST',
        credentials: 'include'
    });
};
