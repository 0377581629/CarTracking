(function () {
    $(function () {
        let _$PointTable = $('#PointTable');
        let _$PointTableFilter = $('#PointTableFilter');
        let _$PointFormFilter = $('#PointFormFilter');

        let _$refreshButton = _$PointFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _PointService = abp.services.app.point;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Point/';
        let _viewUrl = abp.appPath + 'Lib/Point/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Point.Create'),
            edit: abp.auth.hasPermission('Lib.Point.Edit'),
            'delete': abp.auth.hasPermission('Lib.Point.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPointModal'
        });

        let getFilter = function () {
            return {
                filter: _$PointTableFilter.val()
            };
        };

        let dataTable = _$PointTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _PointService.getAll,
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
                                    _createOrEditModal.open({id: data.record.point.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.point, _PointService, getPoint);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "point.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "point.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "point.managementUnitName",
                    name: "managementUnitName"
                },
                {
                    targets: 5,
                    data: "point.pointTypeName",
                    name: "pointTypeName"
                },
                {
                    targets: 6,
                    data: "point.note",
                    name: "note"
                }
            ]
        });

        function getPoint() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getPoint);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditPointModalSaved', getPoint);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getPoint();
            }
        });
    });
})();