$(document).ready(function () {
    $('button#pay').click(function (event) {
        doPay(event);
    });
    $('#pmpmInstant').click(switchToInstantPaymentMode);
    $('#pmInstallment').click(switchToInstallmentPaymentMode);
});

function switchToInstantPaymentMode() {
    $('#installmentOptions').hide();
}

function switchToInstallmentPaymentMode() {
    var orderInfo = getFormData('#orderInfo');
    var data = {
        amount: orderInfo.amount,
        currencyCode: orderInfo.currency
    };
    $.ajax({
        url: '/home/getinstallmentoptions',
        method: 'POST',
        data: data,
        error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxError(jqXHR, textStatus, errorThrown);
        },
        success: function (response) {
            onInstallmentOptionsResponse(response);
        }
    });
}

function doPay(event) {
    event.preventDefault();
    var paymentInfo = getFormData('#paymentInfo');
    var orderInfo = getFormData('#orderInfo');
    var data = Object.assign(paymentInfo, orderInfo);

    $('#paymentRedirectInfo').hide();
    $.ajax({
        url: '/home/requestpayment',
        method: 'POST',
        data: data,
        error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxError(jqXHR, textStatus, errorThrown);
        },
        success: function (response) {
            console.log('[requestpayment] server response:');
            console.log(response);
            if (response.code) {
                showUIError('Lỗi: ' + response.code + '\r\nNội dung: ' + response.message);
                return;
            }
            $('#transactionCode').text(response.transactionCode);
            $('#paymentLink').text(response.checkoutUrl);
            $('#paymentLink').attr('href', response.checkoutUrl);
            $('#paymentRedirectInfo').show();
     }});
}

function onInstallmentOptionsResponse(response) {
    console.log('[onInstallmentOptionsResponse]');
    console.log(response);
    if (response.code) {
        showUIError('Lỗi: ' + response.code + '\r\nNội dung: ' + response.message);
        return;
    }

    if (!response.data || response.data.length <= 0) {
        showUIError('Không có tuỳ chọn trả góp nào khả dụng');
        return;
    }

    var html = '';
    response.data.forEach(function (item, index) {
        html += createBankHTML(item);
    });
    $('#banks').html(html);
    $('#installmentOptions').show();
    
}

function createBankHTML(item) {
    var bankImageName = item.bankCode.toUpperCase() + '.jpg';
    var bankHTML = '<image src="images/bank-logos/{0}" class="img-bank img-rounded shadow" data-bank-code="{1}" title="{2}" />'.format(bankImageName, item.bankCode, item.bankName);
    return bankHTML;
}

function doPayInstallment(event) {

}
