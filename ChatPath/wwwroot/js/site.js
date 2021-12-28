if (!!$.cookie('nickName')) {
    $("#nickNameInput").val($.cookie("nickName"));
    renderNickname($.cookie("nickName"));
}
else {
    plsWait();
}

function plsWait() {
    window.loading_screen = window.pleaseWait({
        logo: "../img/loading2.gif",
        backgroundColor: '#f2f2f2',
        loadingHtml: "<p class='loading-message' style='display:none;'></p>"
            + "<input type='text' id='nickNameInput' placeholder='Nickname Giriniz' maxlength='50' autocomplete='off' autocorrect='off' autocapitalize='off' spellcheck='false' /><div style='clear:both;margin:15px;'></div>"
            + "<button type='button' id='nickNameBtn' onclick='signIn()'class='btn btn-primary'>Sohbete Başla</button>"
        //+ '<div class="hero-left-shape">&nbsp;</div>'
        //+ '<img class="hr-rec hr-rec-4" src="https://path.com.tr/assets/images/hero-rec-circle-2.svg" alt="">'
        //+ '<img class="hr-rec hr-rec-1" src="https://path.com.tr/assets/images/hero-rec-oval.svg" alt="">'
        //+ '<img class="hr-rec hr-rec-2" src="https://path.com.tr/assets/images/hero-rec-blu.svg" alt="">'
        //+ '<img class="hr-rec hr-rec-3" src="https://path.com.tr/assets/images/hero-rec-zigzag.svg" alt="">'
        //+ '<section class="text-center align-items-center d-flex hero-area">'
        //+ '<div class= "hero-left-shape" >&nbsp;</div> '
        //+ '<div class= "hero-top-bg-shape" style = "'     
        //+ '">&nbsp;</div>'
        //+ '    <img class= "hr-rec hr-rec-4" src = "https://path.com.tr/assets/images/hero-rec-circle-2.svg" alt = "" > <img class="hr-rec hr-rec-1" src="https://path.com.tr/assets/images/hero-rec-oval.svg" alt=""> <img class="hr-rec hr-rec-2" src="https://path.com.tr/assets/images/hero-rec-blu.svg" alt=""> <img class="hr-rec hr-rec-3" src="https://path.com.tr/assets/images/hero-rec-zigzag.svg" alt="">'
        //+ '    <div class="container py-5 hero-cont" style="'
        //+ '">'
        //+ '        <div class="row">'
        //+ '            <div class="px-lg-5 col-sm-12 col-lg-6 align-items-start text-left hero-left-area">'
        //+ '<div class="col-sm-12 col-lg-6">'
        //+ '<div class="hero-img-container">&nbsp;</div>'
        //+ '</div>'
        //+ '</div>'
        //+ '</div>'
        //+ '</section>'
    });
}

$(function () {
    $("#txtSearchChannel").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        if (!!value) {
            $("#searchBtn").removeClass("fa-search").addClass("fa-times");
        }
        else {
            $("#searchBtn").removeClass("fa-times").addClass("fa-search");
        }
        $("#channelContent .channelCell").filter(function () {
            $(this).toggle($(this).data("search").toLowerCase().indexOf(value) > -1)
        });
    });

    $("#sendMsg").on("click", function () {
        if (!!$("#msgTextInput").val()) {
            sendMsg(joinedChannel, currentNickname, $("#msgTextInput").val());
        }
    });

    $('#msgTextInput').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            if (!!$(this).val()) {
                sendMsg(joinedChannel, currentNickname, $(this).val());
            }
            return false;
        }
    });

    $('#nickNameInput').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            signIn();
            return false;
        }
    });
});

function renderNickname(_nick) {
    try {
        currentNickname = _nick;
        $("#nickNameInfo").html(_nick);
        $("#nickAvatar").html(_nick[0].toLocaleUpperCase());
    } catch (e) {

    }
}

function changeNickname() {
    $("#nickNameInput").val("");
    $("#nickNameInfo").html("");
    $("#nickAvatar").html("");
    $.cookie("nickName", null, { expires: -31, path: '/' });
    currentNickname = undefined;
    plsWait();
    exitChannel();
    $("#channelContent").scrollTop(0);
}


function signIn() {
    var nick = $("#nickNameInput").val();
    if (!nick) {
        $(".loading-message").html("Lütfen bir nickname giriniz!").show();
        return;
    }
    $.cookie("nickName", nick, { expires: 30 });
    window.loading_screen.finish();
    renderNickname(nick);
}

function exitChannel() {
    $("#messages").hide();
    $("#preView").show();
    if (!!joinedChannel) {
        unSubscribe(joinedChannel);
    }
    joinedChannel = undefined;
}

function searchChannel() {
    if ($("#searchBtn").hasClass("fa-times")) {
        $("#searchBtn").removeClass("fa-times").addClass("fa-search");
        $("#txtSearchChannel").val("");
    }
    var value = $("#txtSearchChannel").val().toLowerCase();
    $("#channelContent .channelCell").filter(function () {
        $(this).toggle($(this).data("search").toLowerCase().indexOf(value) > -1)
    });
    if (!!value) {
        $("#searchBtn").removeClass("fa-search").addClass("fa-times");
    }
}

function joinChannel(element) {
    var chn = $(element).closest(".channelCell").data("chn");
    var nm = $(element).closest(".channelCell").data("nm");
    var bg = $(element).parent().find(".channelAvatar").css("background-image");
    $("#msgChannelAvatar").css("background-image", bg);
    $("#msgChannelName").html(nm);

    $("#preView").hide();
    $("#messages").show();

    var ajaxData = {
        chn: chn
    };
    $.ajax({
        cache: false,
        async: true,
        method: 'POST',
        url: '/Home/GetMessages',
        data: ajaxData,
        success: function (result) {
            $("#msgTexts").html(result);
            $("#msgTexts")[0].scrollTo(0, document.querySelector("#msgTexts").scrollHeight);
            joinedChannel = nm;
            subscribe(joinedChannel);
        },
        error: function (result) {
            //var test = result;
        }
    });
}

function renderReceiveMsg(channel, nickName, msg) {
    var today = new Date();
    var time = today.getHours() + ":" + today.getMinutes();
    if (joinedChannel == channel && nickName == currentNickname) {
        var html = '<div class="outGoing">'
            + '<div class="bubble" >'
            + '<div class="msg_text">' + msg + '</div>'
            + '<div class="msg_time">' + time + '</div>'
            + '<div class="msg_date"></div>'
            + '</div >'
            + '</div >';
        $("#msgTexts").append(html);
        $("#msgTexts")[0].scrollTo(0, document.querySelector("#msgTexts").scrollHeight);
        $("#msgTextInput").val("");
    }
    else if (joinedChannel == channel && nickName != currentNickname) {
        var html = '<div class="inComing">'
            + '<div class="bubble">'
            + '<div class="nick">' + nickName + '</div>'
            + '<div class="msg_text">' + msg + '</div>'
            + '<div class="msg_time">' + time + '</div>'
            + '</div>'
            + '</div>';
        $("#msgTexts").append(html);
        $("#msgTexts")[0].scrollTo(0, document.querySelector("#msgTexts").scrollHeight);
    }
}

