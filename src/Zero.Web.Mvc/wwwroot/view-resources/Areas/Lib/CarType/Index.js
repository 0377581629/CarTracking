(function () {
    $(function () {
        let _$CarTypeTable = $('#CarTypeTable');
        let _$CarTypeTableFilter = $('#CarTypeTableFilter');
        let _$CarTypeFormFilter = $('#CarTypeFormFilter');

        let _$refreshButton = _$CarTypeFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _CarTypeService = abp.services.app.carType;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/CarType/';
        let _viewUrl = abp.appPath + 'Lib/CarType/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.CarType.Create'),
            edit: abp.auth.hasPermission('Lib.CarType.Edit'),
            'delete': abp.auth.hasPermission('Lib.CarType.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCarTypeModal'
        });

        let getFilter = function () {
            return {
                filter: _$CarTypeTableFilter.val()
            };
        };

        let dataTable = _$CarTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _CarTypeService.getAll,
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
                                    _createOrEditModal.open({id: data.record.carType.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.carType, _CarTypeService, getCarType);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "carType.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "carType.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "carType.description",
                    name: "description"
                }
            ]
        });

        function getCarType() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getCarType);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditCarTypeModalSaved', getCarType);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getCarType();
            }
        });
    });
})();