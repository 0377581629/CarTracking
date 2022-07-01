(function () {
    $(function () {
        let _$AssignmentRouteTable = $('#AssignmentRouteTable');
        let _$AssignmentRouteTableFilter = $('#AssignmentRouteTableFilter');
        let _$AssignmentRouteFormFilter = $('#AssignmentRouteFormFilter');

        let _$refreshButton = _$AssignmentRouteFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _AssignmentRouteService = abp.services.app.assignmentRoute;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/AssignmentRoute/';
        let _viewUrl = abp.appPath + 'Lib/AssignmentRoute/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.AssignmentRoute.Create'),
            edit: abp.auth.hasPermission('Lib.AssignmentRoute.Edit'),
            'delete': abp.auth.hasPermission('Lib.AssignmentRoute.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditAssignmentRouteModal'
        });

        let getFilter = function () {
            return {
                filter: _$AssignmentRouteTableFilter.val()
            };
        };

        let dataTable = _$AssignmentRouteTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _AssignmentRouteService.getAll,
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
                                    _createOrEditModal.open({id: data.record.assignmentRoute.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.assignmentRoute, _AssignmentRouteService, getAssignmentRoute);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "assignmentRoute.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "assignmentRoute.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "assignmentRoute.carLicensePlate",
                    name: "carLicensePlate"
                },
                {
                    targets: 5,
                    data: "assignmentRoute.driverName",
                    name: "driverName"
                },
                {
                    targets: 6,
                    data: "assignmentRoute.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getAssignmentRoute() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getAssignmentRoute);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditAssignmentRouteModalSaved', getAssignmentRoute);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getAssignmentRoute();
            }
        });
    });
})();