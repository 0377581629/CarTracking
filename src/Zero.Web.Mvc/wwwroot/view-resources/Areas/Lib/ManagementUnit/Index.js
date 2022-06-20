(function () {
    $(function () {
        let _$ManagementUnitTable = $('#ManagementUnitTable');
        let _$ManagementUnitTableFilter = $('#ManagementUnitTableFilter');
        let _$ManagementUnitFormFilter = $('#ManagementUnitFormFilter');

        let _$refreshButton = _$ManagementUnitFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _ManagementUnitService = abp.services.app.managementUnit;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/ManagementUnit/';
        let _viewUrl = abp.appPath + 'Lib/ManagementUnit/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.ManagementUnit.Create'),
            edit: abp.auth.hasPermission('Lib.ManagementUnit.Edit'),
            'delete': abp.auth.hasPermission('Lib.ManagementUnit.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditManagementUnitModal'
        });

        let getFilter = function () {
            return {
                filter: _$ManagementUnitTableFilter.val()
            };
        };

        let dataTable = _$ManagementUnitTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _ManagementUnitService.getAll,
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
                                    _createOrEditModal.open({id: data.record.managementUnit.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.managementUnit, _ManagementUnitService, getManagementUnit);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "managementUnit.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "managementUnit.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "managementUnit.note",
                    name: "note"
                }
            ]
        });

        function getManagementUnit() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getManagementUnit);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditManagementUnitModalSaved', getManagementUnit);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getManagementUnit();
            }
        });
    });
})();