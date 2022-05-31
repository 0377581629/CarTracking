(function () {
    $(function () {
        let _$NetworkProviderTable = $('#NetworkProviderTable');
        let _$NetworkProviderTableFilter = $('#NetworkProviderTableFilter');
        let _$NetworkProviderFormFilter = $('#NetworkProviderFormFilter');

        let _$refreshButton = _$NetworkProviderFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _NetworkProviderService = abp.services.app.networkProvider;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/NetworkProvider/';
        let _viewUrl = abp.appPath + 'Lib/NetworkProvider/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.NetworkProvider.Create'),
            edit: abp.auth.hasPermission('Lib.NetworkProvider.Edit'),
            'delete': abp.auth.hasPermission('Lib.NetworkProvider.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNetworkProviderModal'
        });

        let getFilter = function () {
            return {
                filter: _$NetworkProviderTableFilter.val()
            };
        };

        let dataTable = _$NetworkProviderTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _NetworkProviderService.getAll,
                inputFilter: getFilter
            },
            columnDefs: [
                {
                    targets: 0,
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    }
                },
                {
                    width: 80,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        dropDownStyle: false,
                        cssClass: 'text-center',
                        items: [
                            {
                                icon: 'la la-edit text-primary',
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.networkProvider.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.networkProvider, _NetworkProviderService, getNetworkProvider);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "networkProvider.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "networkProvider.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "networkProvider.accessPoint",
                    name: "accessPoint"
                },
                {
                    targets: 5,
                    data: "networkProvider.gprsUserName",
                    name: "gprsUserName"
                },
                {
                    targets: 6,
                    data: "networkProvider.gprsPassword",
                    name: "gprsPassword"
                },
            ]
        });

        function getNetworkProvider() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getNetworkProvider);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditNetworkProviderModalSaved', getNetworkProvider);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getNetworkProvider();
            }
        });
    });
})();