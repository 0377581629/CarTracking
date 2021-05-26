(function () {
    $(function () {

        let _$DashboardWidgetsTable = $('#DashboardWidgetsTable');
        let _$DashboardWidgetsTableFilter = $('#DashboardWidgetsTableFilter');
        let _$DashboardWidgetsFormFilter = $('#DashboardWidgetsFormFilter');

        let _$refreshButton = _$DashboardWidgetsFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _DashboardWidgetsService = abp.services.app.dashboardWidget;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/App/Views/DashboardWidget/';
        let _viewUrl = abp.appPath + 'App/DashboardWidget/';

        const _permissions = {
            create: abp.auth.hasPermission('Pages.DashboardWidget.Create'),
            edit: abp.auth.hasPermission('Pages.DashboardWidget.Edit'),
            'delete': abp.auth.hasPermission('Pages.DashboardWidget.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDashboardWidgetModal'
        });

        let getFilter = function () {
            return {
                filter: _$DashboardWidgetsTableFilter.val()
            };
        };

        let dataTable = _$DashboardWidgetsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _DashboardWidgetsService.getAll,
                inputFilter: getFilter
            },
            columnDefs: [
                {
                    width: 80,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        dropDownStyle: false,
                        cssClass: 'text-center',
                        items: [
                            {
                                icon: baseHelper.SimpleTableIcon('edit'),
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.dashboardWidget.id});
                                }
                            },
                            {
                                icon: baseHelper.SimpleTableIcon('delete'),
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.dashboardWidget, _DashboardWidgetsService, getDashboardWidgets);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "dashboardWidget.name",
                    name: "name"
                },
                {
                    targets: 2,
                    data: "dashboardWidget.description",
                    name: "description"
                },
                {
                    targets: 3,
                    data: "dashboardWidget",
                    name: "viewName, jsPath, cssPath",
                    render: function (dashboardWidget) {
                        return '<p>View Name: ' + dashboardWidget.viewName + '</p>' +
                               '<p>JS Path: ' + dashboardWidget.jsPath + '</p>' +
                               '<p>CSS Path: ' + dashboardWidget.cssPath + '</p>';
                    }
                },
                {
                    targets: 4,
                    data: "dashboardWidget",
                    name: "width, height, positionX, positionY",
                    render: function (dashboardWidget) {
                        return '<p>Width X Height: ' + dashboardWidget.width + ' X ' + dashboardWidget.height + '</p>' +
                               '<p>Default location X:Y: ' + dashboardWidget.positionX + ':' + dashboardWidget.positionY + '</p>';
                    }
                },
                {
                    targets: 5,
                    data: "dashboardWidget.isDefault",
                    name: "isDefault",
                    class: "text-center",
                    render: function (isDefault) {
                        return baseHelper.ShowActive(isDefault);
                    }
                }
            ]
        });


        function getDashboardWidgets() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getDashboardWidgets);
        }
        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditDashboardWidgetModalSaved', function () {
            getDashboardWidgets();
        });

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getDashboardWidgets();
            }
        });

    });
})();