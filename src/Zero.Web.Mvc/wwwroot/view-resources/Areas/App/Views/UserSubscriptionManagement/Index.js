(function () {
    $(function () {

        var _$paymentHistoryTable = $('#PaymentHistoryTable');
        var _paymentService = abp.services.app.userPayment;
        var _invoiceService = abp.services.app.userInvoice;

        var _dataTable;

        function createDatatable() {
            var dataTable = _$paymentHistoryTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                listAction: {
                    ajaxFunction: _paymentService.getPaymentHistory
                },
                columnDefs: [
                    {
                        className: 'control responsive',
                        orderable: false,
                        render: function () {
                            return '';
                        },
                        targets: 0
                    },
                    {
                        targets: 1,
                        data: "creationTime",
                        class: "text-center",
                        render: function (creationTime) {
                            return moment(creationTime).format('L LT');
                        }
                    },
                    {
                        targets: 2,
                        data: "gateway",
                        render: function (gateway) {
                            return app.localize("SubscriptionPaymentGatewayType_" + gateway);
                        }
                    },
                    {
                        targets: 3,
                        data: "amount",
                        class: "text-right",
                        render: $.fn.dataTable.render.number(',', '.', 2)
                    },
                    {
                        targets: 4,
                        data: "currency",
                        class: "text-center"
                    },
                    {
                        targets: 5,
                        data: "status",
                        class: "text-center",
                        render: function (status) {
                            return app.localize("SubscriptionPaymentStatus_" + status);
                        }
                    },
                    {
                        targets: 6,
                        data: "dayCount",
                        class:  "text-right"
                    },
                    {
                        targets: 7,
                        data: "externalPaymentId"
                    },
                    {
                        targets: 8,
                        visible: false,
                        data: "id"
                    }
                ]
            });

            return dataTable;
        }

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var target = $(e.target).attr("href");
            if (target === '#SubscriptionManagementPaymentHistoryTab') {

                if (_dataTable) {
                    return;
                }

                _dataTable = createDatatable();
            }
        });

        function createOrShowInvoice(data) {
            var invoiceNo = data["invoiceNo"];
            var paymentId = data["id"];

            if (invoiceNo) {
                window.open('/App/UserInvoice?paymentId=' + paymentId, '_blank');
            } else {
                _invoiceService.createInvoice({
                    userSubscriptionPaymentId: paymentId
                }).done(function () {
                    _dataTable.ajax.reload();
                    window.open('/App/UserInvoice?paymentId=' + paymentId, '_blank');
                });
            }
        }
    });
})();