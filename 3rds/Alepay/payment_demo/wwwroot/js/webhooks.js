$(document).ready(function () {
    $('#frmSearchWebhook').submit(function (event) {
        event.preventDefault();
        doSearchWebhookByKeyword();
    });

    $('body').on('click', 'button.viewWebHook', doLoadWebHookDetails);

    doSearchWebhook();
});

function doSearchWebhookByKeyword() {
    var keyword = $('#transactionCode').val();
    doSearchWebhook(keyword);
}

function doSearchWebhook(keyword) {
    var data = {
        transactionCode: keyword
    };

    $('#webhooks').hide();
    $('#webhookDetails').hide();

    $.ajax({
        url: '/home/searchwebhooks',
        method: 'POST',
        data: data,
        error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxError(jqXHR, textStatus, errorThrown);
        },
        success: function (response) {
            console.log('[doSearchWebhook] server response:');
            console.log(response);

            if (!response.success) {
                showUIError('Lỗi: ' + response.errorMessage);
                return;
            }

            if (!response.data || response.data.length <= 0) {
                showUIError('Không có kết quả');
                return;
            }

            populateWebhookSearchResult(response.data);
        }
    });
}

function populateWebhookSearchResult(webhooks) {
    var webHooksHTML = '';
    webhooks.forEach(function (item, index) {
        webHooksHTML += makeWebhookHTML(item);
    });
    webHooksHTML = '<div class=\"row\">{0}</div>'.format(webHooksHTML);
    $('#webhooks').empty();
    $('#webhooks').html(webHooksHTML);
    $('#webhooks').show();
}

function makeWebhookHTML(webhook) {
    var html = "<div class=\"col-sm-3 webhook\"> \
                    <div class=\"card\">\
                        <div class=\"card-body\">\
                            <h5 class=\"card-title transactionCode\">{0}</h5>\
                            <p class=\"card-text\">\
                                <p class=\"receivedTimeStr\">{1} - {2}</p>\
                            </p>\
                            <button class=\"btn btn-sm btn-primary viewWebHook\" data-id=\"{3}\">Xem</button>\
                        </div>\
                    </div>\
               </div>".format(webhook.transactionCode, webhook.receivedDateStr,
                              webhook.receivedTimeStr, webhook.id);
    return html;
}

function doLoadWebHookDetails() {
    var id = $(this).attr('data-id');
    if (!id) {
        showUIError('Không tìm thấy id của webhook');
        return;
    }

    var data = {
        id: id
    };

    $.ajax({
        url: '/home/getwebhook',
        method: 'POST',
        data: data,
        error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxError(jqXHR, textStatus, errorThrown);
        },
        success: function (response) {
            if (!response.success) {
                showUIError('Lỗi: ' + response.errorMessage);
                return;
            }

            restoreFormData('#webhookDetails', response);
            restoreFormData('#webhookDetails', response.transactionInfo);
            $('#webhooks').hide();
            $('#webhookDetails').show();
        }
    });
}