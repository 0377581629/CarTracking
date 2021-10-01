// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Show blockUI on ajaxStart and hide it on ajax stop
$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

// Add string.format support
String.prototype.format = function () {
    a = this;
    for (k in arguments) {
        a = a.replace("{" + k + "}", arguments[k])
    }
    return a
}

function handleAjaxError(jqXHR, textStatus, errorThrown) {
    var msg = 'Ajax request error! textStatus: ' + textStatus +
        ', errorThrown: ' + errorThrown;
    console.error(msg);
    showUIError(msg, 'Ajax Error')
}

function showUIError(error, title) {
    if (title) {
        alert('[' + title + ']\r\n' + error);
        return;
    }
    alert(error);
}