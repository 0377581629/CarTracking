using System.Collections.Generic;

namespace alepay
{
    public static class AlePayDefs
    {
        // For live environment
        //private const string APIBaseUrl = "https://alepay-v3.nganluong.vn/api/v3/checkout";

        // Sanbox environment
        public static string APIBaseUrl = "https://alepay-v3-sandbox.nganluong.vn/api/v3/checkout";

        public const string BankLogoBaseUrl = "https://alepay.vn/dataimage";
        
        public const string CURRENCY_VND = "VND";
        public const string CURRENCY_USD = "USD";

        public const string CARD_VISA = "VISA";
        public const string CARD_MASTERCARD = "MASTERCARD";
        public const string CARD_JCB = "JCB";

        public const int CO_TYPE_INSTANT_PAYMENT_WITH_INTL_CARDS_AND_INSTALLMENT = 0;
        public const int CO_TYPE_INSTANT_PAYMENT_WITH_INTL_CARDS = 1;
        public const int CO_TYPE_ONLY_INSTALLMENT = 2;
        public const int CO_TYPE_INSTANT_PAYMENT_WITH_ATM_IB_QRCODE_INTL_CARDS_INSTALLMENT = 3;
        public const int CO_TYPE_INSTANT_PAYMENT_WITH_ATM_IB_QRCODE_INTL_CARDS = 4;

        /// <summary>
        /// ATM_ON: thanh toán bằng thẻ ATM
        /// </summary>
        public const string PM_ATM_ON = "ATM_ON";

        /// <summary>
        /// IB_ON: thanh toán bằng tài khoản IB
        /// </summary>
        public const string PM_IB_ON = "IB_ON";

        /// <summary>
        /// QRCODE: thanh toán bằng cách quét mã QRCODE
        /// </summary>
        public const string PM_QR_CODE = "QRCODE";

        public const string LANG_VI = "vi";
        public const string LANG_EN = "en";

        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>() {
            { 0 , "Thành công"},
            { 1, "Tham số không hợp lệ"},
            { 5, "Không có dữ liệu"},
            { 101, "Checksum không hợp lệ"},
            { 102, "Mã hoá không hợp lệ"},
            { 103, "IP không được phép truy cập"},
            { 104, "Dữ liệu không hợp lệ"},
            { 105, "Token key không hợp lệ"},
            { 106, "Token thanh toán Alepay không tồn tại hoặc đã bị hủy"},
            { 107, "Giao dịch đang được xử lý"},
            { 108, "Dữ liệu không tìm thấy"},
            { 109, "Mã đơn hàng không tìm thấy"},
            { 110, "Phải có email hoặc số điện thoại người mua"},
            { 111, "Giao dịch thất bại"},
            { 120, "Giá trị đơn hàng phải lớn hơn 0"},
            { 121, "Loại tiền tệ không hợp lệ"},
            { 122, "Mô tả đơn hàng không tìm thấy"},
            { 123, "Tổng số sản phẩm phải lớn hơn không"},
            { 124, "Định dạng URL không chính xác (http://, https://)"},
            { 125, "Tên người mua không đúng định dạng"},
            { 126, "Email người mua không đúng định dạng"},
            { 127, "SĐT người mua không đúng định dạng"},
            { 128, "Địa chỉ người mua không hợp lệ"},
            { 129, "City người mua không hợp lệ"},
            { 130, "Quốc gia người mua không hợp lệ"},
            { 131, "Hạn thanh toán phải lớn hơn 0"},
            { 132, "Email không hợp lệ"},
            { 133, "Thông tin thẻ không hợp lệ"},
            { 134, "Thẻ hết hạn mức thanh toán"},
            { 135, "Giao dịch bị từ chối bởi ngân hàng phát hành thẻ"},
            { 136, "Mã giao dịch không tồn tại"},
            { 137, "Giao dịch không hợp lệ"},
            { 138, "Tài khoản Merchant không tồn tại"},
            { 139, "Tài khoản Merchant không hoạt động"},
            { 140, "Tài khoản Merchant không hợp lệ"},
            { 142, "Ngân hàng không hỗ trợ trả góp"},
            { 143, "Thẻ không được phát hành bởi ngân hàng đã chọn"},
            { 144, "Kỳ thanh toán không hợp lệ"},
            { 145, "Số tiền giao dịch trả góp không hợp lệ"},
            { 146, "Thẻ của bạn không thuộc ngân hang hỗ trợ trả góp"},
            { 147, "Số điện thoại không hợp lệ"},
            { 148, "Thông tin trả góp không hợp lệ"},
            { 149, "Loại thẻ không hợp lệ"},
            { 150, "Thẻ bị review"},
            { 151, "Ngân hàng không hỗ trợ thanh toán"},
            { 152, "Số thẻ không phù hợp với loại thẻ đã chọn"},
            { 153, "Giao dịch không tồn tại"},
            { 154, "Số tiền vượt quá hạn mức cho phép"},
            { 155, "Đợi người mua xác nhận trả góp"},
            { 156, "Số tiền thanh toán không hợp lệ"},
            { 157, "email không khớp với profile đã tồn tại"},
            { 158, "Số điện thoại không khớp với profile đã tồn tại"},
            { 159, "Id không được để trống"},
            { 160, "First name không được để trống"},
            { 161, "Last name không được để trống"},
            { 162, "Email không được để trống"},
            { 163, "city không được để trống"}
            // Còn nữa cần cập nhật thêm
        };
    }
}
