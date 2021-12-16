(function () {
    $(function () {

        let _$CurrencyRatesTable = $('#CurrencyRatesTable');
        let _$CurrencyRatesTableFilter = $('#CurrencyRatesTableFilter');
        let _$CurrencyRatesFormFilter = $('#CurrencyRatesFormFilter');

        let _$refreshButton = _$CurrencyRatesFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _CurrencyRatesService = abp.services.app.currencyRate;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/App/Views/CurrencyRate/';
        let _viewUrl = abp.appPath + 'App/CurrencyRate/';

        const _permissions = {
            create: abp.auth.hasPermission('Pages.CurrencyRate.Create'),
            edit: abp.auth.hasPermission('Pages.CurrencyRate.Edit'),
            'delete': abp.auth.hasPermission('Pages.CurrencyRate.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCurrencyRateModal'
        });

        let getFilter = function () {
            return {
                filter: _$CurrencyRatesTableFilter.val()
            };
        };

        let dataTable = _$CurrencyRatesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _CurrencyRatesService.getAll,
                inputFilter: getFilter
            },
            columnDefs: [
                {
                    width: 80,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        dropDownStyle: false,
                        cssClass: 'text-center',
                        items: [
                            {
                                icon: baseHelper.SimpleTableIcon('edit'),
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.currencyRate.id});
                                }
                            },
                            {
                                icon: baseHelper.SimpleTableIcon('delete'),
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.currencyRate, _CurrencyRatesService, getCurrencyRates);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "currencyRate.sourceCurrency",
                    name: "sourceCurrency"
                },
                {
                    targets: 2,
                    data: "currencyRate.targetCurrency",
                    name: "targetCurrency"
                },
                {
                    targets: 3,
                    data: "currencyRate.rate",
                    name: "rate",
                    class: "text-center",
                    render: function (rate) {
                        return baseHelper.ShowNumber(rate);
                    }
                }
            ]
        });


        function getCurrencyRates() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getCurrencyRates);
        }
        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditCurrencyRateModalSaved', function () {
            getCurrencyRates();
        });

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getCurrencyRates();
            }
        });

    });
})();