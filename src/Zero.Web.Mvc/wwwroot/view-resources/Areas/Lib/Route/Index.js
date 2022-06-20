(function () {
    $(function () {
        let _$RouteTable = $('#RouteTable');
        let _$RouteTableFilter = $('#RouteTableFilter');
        let _$RouteFormFilter = $('#RouteFormFilter');

        let _$refreshButton = _$RouteFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _RouteService = abp.services.app.route;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Route/';
        let _viewUrl = abp.appPath + 'Lib/Route/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Route.Create'),
            edit: abp.auth.hasPermission('Lib.Route.Edit'),
            'delete': abp.auth.hasPermission('Lib.Route.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRouteModal'
        });

        let getFilter = function () {
            return {
                filter: _$RouteTableFilter.val()
            };
        };

        let dataTable = _$RouteTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _RouteService.getAll,
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
                                    _createOrEditModal.open({id: data.record.route.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.route, _RouteService, getRoute);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "route.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "route.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "route.minuteLate",
                    name: "minuteLate"
                },
                {
                    targets: 5,
                    data: "route.range",
                    name: "range"
                },
                {
                    targets: 6,
                    data: "route.estimatedTime",
                    name: "range"
                },
                {
                    targets: 7,
                    data: "route.estimateDistance",
                    name: "range"
                },
                {
                    targets: 8,
                    data: "author.isPermanentRoute",
                    name: "isPermanentRoute",
                    class: "text-center",
                    width: 80,
                    render: function(isPermanentRoute) {
                        return baseHelper.ShowActive(isPermanentRoute);
                    }
                },
                {
                    targets: 9,
                    data: "author.hasConstraintTime",
                    name: "hasConstraintTime",
                    class: "text-center",
                    width: 80,
                    render: function(hasConstraintTime) {
                        return baseHelper.ShowActive(hasConstraintTime);
                    }
                }
            ]
        });

        function getRoute() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getRoute);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditRouteModalSaved', getRoute);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getRoute();
            }
        });
    });
})();