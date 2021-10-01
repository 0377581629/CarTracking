(function () {
    $(function () {

        var _$paymentHistoryTable = $('#PaymentHistoryTable');
        var _paymentService = abp.services.app.payment;
        var _invoiceService = abp.services.app.invoice;
        var _subscriptionService = abp.services.app.subscription;

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
                        data: "editionDisplayName"
                    },
                    {
                        targets: 4,
                        data: "gateway",
                        render: function (gateway) {
                            return app.localize("SubscriptionPaymentGatewayType_" + gateway);
                        }
                    },
                    {
                        targets: 5,
                        data: "amount",
                        render: $.fn.dataTable.render.number(',', '.', 2),
                        class: "text-right"
                    },
                    {
                        targets: 6,
                        data: "currency",
                        class: "text-center"
                    },
                    {
                        targets: 7,
                        data: "status",
                        class: "text-center",
                        render: function (status) {
                            return app.localize("SubscriptionPaymentStatus_" + status);
                        }
                    },
                    {
                        targets: 8,
                        data: "paymentPeriodType",
                        render: function (paymentPeriodType) {
                            return app.localize("PaymentPeriodType_" + paymentPeriodType);
                        }
                    },
                    {
                        targets: 9,
                        data: "dayCount",
                        class: "text-center"
                    },
                    {
                        targets: 10,
                        data: "externalPaymentId"
                    },
                    {
                        targets: 11,
                        data: "invoiceNo"
                    },
                    {
                        targets: 12,
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

        $('#btnDisableRecurringPayments').click(function () {
            abp.ui.setBusy();

            _subscriptionService.disableRecurringPayments({}).done(function () {
                abp.ui.clearBusy();
                $('#btnEnableRecurringPayments').closest("div.form-group").removeClass("d-none");
                $('#btnDisableRecurringPayments').closest("div.form-group").addClass("d-none");
                if ($('#btnExtend')) {
                    $('#btnExtend').removeClass('d-none');
                }
            });
        });

        $('#btnEnableRecurringPayments').click(function () {
            abp.ui.setBusy();

            _subscriptionService.enableRecurringPayments({}).done(function () {
                abp.ui.clearBusy();
                $('#btnDisableRecurringPayments').closest("div.form-group").removeClass("d-none");
                $('#btnEnableRecurringPayments').closest("div.form-group").addClass("d-none");
                $('#btnExtend').addClass('d-none');
            });
        });

        function createOrShowInvoice(data) {
            var invoiceNo = data["invoiceNo"];
            var paymentId = data["id"];

            if (invoiceNo) {
                window.open('/App/Invoice?paymentId=' + paymentId, '_blank');
            } else {
                _invoiceService.createInvoice({
                    subscriptionPaymentId: paymentId
                }).done(function () {
                    _dataTable.ajax.reload();
                    window.open('/App/Invoice?paymentId=' + paymentId, '_blank');
                });
            }
        }
    });
})();