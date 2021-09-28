$(document).ready(function () {
    $('#frmTransactionInfo').submit(function (event) {
        event.preventDefault();
        doGetTransactionInfo();
    });
});

function doGetTransactionInfo() {
    var data = getFormData('#frmTransactionInfo');
    $.ajax({
        url: '/home/querytransactionInfo',
        method: 'POST',
        data: data,
        error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxError(jqXHR, textStatus, errorThrown);
        },
        success: function (response) {
            if (response.code) {
                showUIError('Lỗi: {0}\r\nNội dung: {1}'.format(response.code, response.message));
                return;
            }
            restoreFormData("#transactionInfo", response);
            $('#transactionInfo').show();
        }
    });
}