(function () {
    $(function () {
        let _$CarTable = $('#CarTable');
        let _$CarTableFilter = $('#CarTableFilter');
        let _$CarFormFilter = $('#CarFormFilter');

        let _$refreshButton = _$CarFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _CarService = abp.services.app.car;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Car/';
        let _viewUrl = abp.appPath + 'Lib/Car/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Car.Create'),
            edit: abp.auth.hasPermission('Lib.Car.Edit'),
            'delete': abp.auth.hasPermission('Lib.Car.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCarModal'
        });

        let getFilter = function () {
            return {
                filter: _$CarTableFilter.val()
            };
        };

        let dataTable = _$CarTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _CarService.getAll,
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
                                    _createOrEditModal.open({id: data.record.car.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.car, _CarService, getCar);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "car.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "car.licensePlate",
                    name: "licensePlate"
                },
                {
                    targets: 4,
                    data: "car.deviceSimCard",
                    name: "deviceSimCard"
                },
                {
                    targets: 5,
                    data: "car.carTypeName",
                    name: "carTypeName"
                },
                {
                    targets: 6,
                    data: "car.carGroupName",
                    name: "carGroupName"
                },
                {
                    targets: 7,
                    data: "car.driverName",
                    name: "driverName"
                },
                {
                    targets: 8,
                    data: "car.rfidTypeCardNumber",
                    name: "rfidTypeCardNumber"
                }
            ]
        });

        function getCar() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getCar);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditCarModalSaved', getCar);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getCar();
            }
        });
    });
})();