(function () {
    $(function () {
        let _$PointTypeTable = $('#PointTypeTable');
        let _$PointTypeTableFilter = $('#PointTypeTableFilter');
        let _$PointTypeFormFilter = $('#PointTypeFormFilter');

        let _$refreshButton = _$PointTypeFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _PointTypeService = abp.services.app.pointType;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/PointType/';
        let _viewUrl = abp.appPath + 'Lib/PointType/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.PointType.Create'),
            edit: abp.auth.hasPermission('Lib.PointType.Edit'),
            'delete': abp.auth.hasPermission('Lib.PointType.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPointTypeModal'
        });

        let getFilter = function () {
            return {
                filter: _$PointTypeTableFilter.val()
            };
        };

        let dataTable = _$PointTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _PointTypeService.getAll,
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
                                    _createOrEditModal.open({id: data.record.pointType.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.pointType, _PointTypeService, getPointType);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "pointType.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "pointType.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "pointType.note",
                    name: "note"
                }
            ]
        });

        function getPointType() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getPointType);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditPointTypeModalSaved', getPointType);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getPointType();
            }
        });
    });
})();