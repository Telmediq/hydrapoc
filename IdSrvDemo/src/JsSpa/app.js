function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

function login() {
    mgr.signinRedirect();
}

function callApi(url) {
    mgr.getUser().then(function (user) {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}

var config = {
    authority: "http://localhost:5000",
    client_id: "poc-client",
    redirect_uri: "http://localhost:5002/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://localhost:5002/index.html",
};

var mgr = new Oidc.UserManager(config);
mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        log("User not logged in");
    }
});

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", e => callApi("http://localhost:5001/authtest/echo-my-claims"), false);
document.getElementById("api2").addEventListener("click", e => callApi("http://localhost:5001/authtest/echo-servers-claims"), false);
document.getElementById("logout").addEventListener("click", logout, false);