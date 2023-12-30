"use strict";
var chatInitialized = false;
function chat() {
    if (!chatInitialized) {
        var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        document.getElementById("sendButton").disabled = true;
        connection.on("ReceiveMessage", function (idnguoidunggui, idnguoidungnhan, NoiDung) {
            var idnguoidungnhan = parseInt(document.getElementById("idnguoidungnhaninput").value);
            displayMessages(idnguoidungnhan);
            document.getElementById('NoiDunginput').value = '';
            callChatBot();
        });
        connection.start()
            .then(function () {
                document.getElementById("sendButton").disabled = false;
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
        document.getElementById("sendButton").addEventListener("click", function (event) {
            var idnguoidunggui = parseInt(document.getElementById("idnguoidungguiinput").value);
            var idnguoidungnhan = parseInt(document.getElementById("idnguoidungnhaninput").value);
            var NoiDung = document.getElementById("NoiDunginput").value;
            connection.invoke("SendMessage", idnguoidunggui, idnguoidungnhan, NoiDung)
                .catch(function (err) {
                    return console.error(err.toString());
                });
            event.preventDefault();
        });
        chatInitialized = true;
    }
}
var timtinhans = false;
function timtinnhan() {
    if (!timtinhans) {
        var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        var buttons = document.querySelectorAll('[class^="sentimButton_"]');
        buttons.forEach(function (button) {
            button.disabled = false;
            button.addEventListener("click", function (event) {
                var classes = event.target.className.split('_');
                var idnguoidungnhan = parseInt(classes[3]);
                displayMessages(idnguoidungnhan);
                callChatBot();
            });
        });
        connection.start()
            .then(function () {
                var buttons = document.querySelectorAll('[class^="sentimButton_"]');
                buttons.forEach(function (button) {
                    button.addEventListener("click", function (event) {
                        var classes = event.target.className.split('_');
                        var idicon = parseInt(classes[1]);
                        var idntinnhan = parseInt(classes[2]);
                        var idnguoidungnhan = parseInt(classes[3]);
                        connection.invoke("SendTimTinNhan", idicon, idntinnhan, idnguoidungnhan)
                            .catch(function (err) {
                                return console.error(err.toString());
                            });
                        event.preventDefault();
                    });
                });
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
        timtinhans = true;
    }
}
function displayMessages(idnguoidungnhan) {
    $.ajax({
        type: 'POST',
        url: '/Home/Tennguoidung',
        data: { idnguoidungnhan: idnguoidungnhan },
        success: function (data) {
            $('#text-area').html(data);
            $('#text-area').scrollTop($('#text-area')[0].scrollHeight);
            $('#text-area').css({ 'display': 'block' });
            $('#text-area li').on('click', function () {
                $('#text-area').css({ 'display': 'none' });
            });
        }
    });
    $.ajax({
        type: 'POST',
        url: '/Home/TinNhan',
        data: { idnguoidungnhan: idnguoidungnhan },
        success: function (data) {
            $('#conversations').html(data);
            $('#conversations').scrollTop($('#conversations')[0].scrollHeight);
            timtinnhan();
        },
        error: function () {
            alert('Đã xảy ra lỗi khi lấy tin nhắn.');
        }
    });
    $.ajax({
        type: 'POST',
        url: '/Home/Nguoidung',
        data: { idnguoidungnhan: idnguoidungnhan },
        success: function (data) {
            $('.active-user').html(data);
            $('.active-user').scrollTop($('.active-user')[0].scrollHeight);
        },
        error: function () {
            alert('Đã xảy ra lỗi khi lấy tin nhắn.');
        }
    });
    $.ajax({
        type: 'POST',
        url: '/Home/ThongTinNguoidung',
        data: { idnguoidungnhan: idnguoidungnhan },
        success: function (data) {
            $('.chater-info').html(data);
            $('.chater-info').scrollTop($('.chater-info')[0].scrollHeight);
        },
        error: function () {
            alert('Đã xảy ra lỗi khi lấy tin nhắn.');
        }
    });
}
function callChatBot() {
    $.ajax({
        url: '/Home/KetBan',
        type: 'POST',
        success: function (data) {
            $('#msg-pepl-list').html(data);
        },
        error: function () {
            alert('Đã xảy ra lỗi khi lấy tin nhắn.');
        }
    });
    $.ajax({
        type: 'POST',
        url: '/Home/Iconsss',
        success: function (data) {
            $('.emojies').html(data);
            $('.emojies').scrollTop($('.emojies')[0].scrollHeight);
        },
        error: function () {
            alert('Đã xảy ra lỗi khi lấy tin nhắn.');
        }
    });
    chat();
}