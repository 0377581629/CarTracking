(function () {
    $(function () {
        let _$DeviceTable = $('#DeviceTable');
        let _$DeviceTableFilter = $('#DeviceTableFilter');
        let _$DeviceFormFilter = $('#DeviceFormFilter');

        let _$refreshButton = _$DeviceFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _DeviceService = abp.services.app.device;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Device/';
        let _viewUrl = abp.appPath + 'Lib/Device/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Device.Create'),
            edit: abp.auth.hasPermission('Lib.Device.Edit'),
            'delete': abp.auth.hasPermission('Lib.Device.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDeviceModal'
        });

        let getFilter = function () {
            return {
                filter: _$DeviceTableFilter.val()
            };
        };

        let dataTable = _$DeviceTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _DeviceService.getAll,
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
                                    _createOrEditModal.open({id: data.record.device.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.device, _DeviceService, getDevice);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "device.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "device.simCard",
                    name: "simCard"
                },
                {
                    targets: 4,
                    data: "device.networkProviderName",
                    name: "networkProviderName"
                },
                {
                    targets: 5,
                    width: 80,
                    data: "device.startDate",
                    name: "startDate",
                    class: "text-center",
                    render: function (date) {
                        return moment(date).format('L');
                    }
                },
                {
                    targets: 6,
                    data: "device.imei",
                    name: "imei"
                },
                {
                    targets: 7,
                    data: "device.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                },
                {
                    targets: 8,
                    data: "device.needUpdate",
                    name: "needUpdate",
                    class: "text-center",
                    width: 80,
                    render: function(needUpdate) {
                        return baseHelper.ShowActive(needUpdate);
                    }
                }
            ]
        });

        function getDevice() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getDevice);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditDeviceModalSaved', getDevice);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getDevice();
            }
        });
    });
})();