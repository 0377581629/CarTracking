(function () {
    $(function () {
        let _$DriverTable = $('#DriverTable');
        let _$DriverTableFilter = $('#DriverTableFilter');
        let _$DriverFormFilter = $('#DriverFormFilter');

        let _$refreshButton = _$DriverFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _DriverService = abp.services.app.driver;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Driver/';
        let _viewUrl = abp.appPath + 'Lib/Driver/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Driver.Create'),
            edit: abp.auth.hasPermission('Lib.Driver.Edit'),
            'delete': abp.auth.hasPermission('Lib.Driver.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDriverModal'
        });

        let getFilter = function () {
            return {
                filter: _$DriverTableFilter.val()
            };
        };

        let dataTable = _$DriverTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _DriverService.getAll,
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
                                    _createOrEditModal.open({id: data.record.driver.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.driver, _DriverService, getDriver);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "driver.avatar",
                    name: "avatar",
                    orderable: false,
                    width: 52,
                    render: function (avatar){
                        return baseHelper.ShowAvatar(avatar);
                    }
                },
                {
                    targets: 3,
                    data: "driver.code",
                    name: "code"
                },
                {
                    targets: 4,
                    data: "driver.name",
                    name: "name"
                },
                {
                    targets: 5,
                    data: "driver.rfidTypeCardNumber",
                    name: "rfidTypeCardNumber"
                },
                {
                    targets: 6,
                    data: "driver.phoneNumber",
                    name: "phoneNumber"
                },
                {
                    targets: 7,
                    data: "driver.email",
                    name: "email"
                },
                {
                    targets: 8,
                    data: "driver.isStopWorking",
                    name: "isStopWorking",
                    class: "text-center",
                    width: 80,
                    render: function(isStopWorking) {
                        return baseHelper.ShowActive(isStopWorking);
                    }
                }
            ]
        });

        function getDriver() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getDriver);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditDriverModalSaved', getDriver);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getDriver();
            }
        });
    });
})();