var connection;
var joinedChannel;
var currentNickname;

$(function () {
    connect();
});

function connect() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:5001" + "/chat")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.start().then(function () {
        console.log("bağlantı kuruldu");
        connection.on("ReceiveMsg", function (channel, nickName, msg) {
            renderReceiveMsg(channel, nickName, msg);
        })
        connection.on("ErrorMsg", function (msg) {
            alert("İşleminiz gerçekleştirilemedi.")
        })
    })
        .catch(error => {
            alert("Sunucuyla bağlantı kurulamadı.");
        });
}

function subscribe(channel) {
    connection.invoke("Subscribe", channel);
}

function unSubscribe(channel) {
    connection.invoke("Unsubscribe", channel);
}

function sendMsg(channel, nickName, msg) {
    connection.invoke("SendMsg", channel, nickName, msg);
}