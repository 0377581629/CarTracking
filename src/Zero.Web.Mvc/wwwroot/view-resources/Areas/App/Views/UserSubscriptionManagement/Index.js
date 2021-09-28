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
                        data: null,
                        orderable: false,
                        defaultContent: '',
                        rowAction: {
                            dropDownStyle: false,
                            cssClass: 'text-center',
                            items: [
                                {
                                    icon: baseHelper.SimpleTableIcon('view'),
                                    text: app.localize('View'),
                                    visible: function () {
                                        return true;
                                    },
                                    action: function (data) {
                                        createOrShowInvoice(data.record);
                                    }
                                }
                            ]
                        }
                    },
                    {
                        targets: 2,
                        data: "creationTime",
                        render: function (creationTime) {
                            return moment(creationTime).format('L');
                        }
                    },
                    {
                        targets: 3,
                        data: "gateway",
                        render: function (gateway) {
                            return app.localize("SubscriptionPaymentGatewayType_" + gateway);
                        }
                    },
                    {
                        targets: 4,
                        data: "amount",
                        render: $.fn.dataTable.render.number(',', '.', 2)
                    },
                    {
                        targets: 5,
                        data: "status",
                        render: function (status) {
                            return app.localize("SubscriptionPaymentStatus_" + status);
                        }
                    },
                    {
                        targets: 6,
                        data: "dayCount"
                    },
                    {
                        targets: 7,
                        data: "externalPaymentId"
                    },
                    {
                        targets: 8,
                        data: "invoiceNo"
                    },
                    {
                        targets: 9,
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