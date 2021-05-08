
function deleteChapter(chapter_id) {
    var username = getCookie('comicusername');
    var password = getCookie('comicpassword');
    var settings = {
        "url": baseApiUrl + "chapterapi?chapter_id=" + chapter_id + "&KEY=" + username + password,
        "method": "DELETE",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        if (response == 0) {
            $('#li' + chapter_id).remove();
            alert("Thành công!");
        } else{
            alert("Thất bại!");
        }
    });
}


function createChapter(comic_id) {
    window.location.href = "http://127.0.0.1:5500/dashboard.html?comic_id=" + comic_id;
}

function deleteComic(comic_id) {
    var username = getCookie('comicusername');
    var password = getCookie('comicpassword');
    var settings = {
        "url": baseApiUrl + "comicapi?comic_id=" + comic_id + "&KEY=" + username + password,
        "method": "DELETE",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        if (response == 0) {
            $('#li' + comic_id).remove();
            alert("Thành công!");
            window.location.reload();
        } else {
            alert("Thất bại!");
        }
    });
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkLogin(username, password) {
    var username = getCookie("comicusername");
    var password = getCookie("comicpassword");
    $.get('http://www.truyensieuhay.somee.com//api/userapi?username=' + username + '&password=' + password, function (data) {
        if (data != '0' && data != '1') {
            window.location.href = baseUrl + "login.html";
        }
    });
}