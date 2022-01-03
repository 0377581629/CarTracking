(function () {
    $(function () {

        let _$WidgetsTable = $('#WidgetsTable');
        let _$WidgetsTableFilter = $('#WidgetsTableFilter');
        let _$WidgetsFormFilter = $('#WidgetsFormFilter');

        let _$refreshButton = _$WidgetsFormFilter.find("button[name='RefreshButton']");
        
        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _WidgetsService = abp.services.app.widget;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Widget/';
        let _viewUrl = abp.appPath + 'Cms/Widget/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.Widget.Create'),
            edit: abp.auth.hasPermission('Cms.Widget.Edit'),
            'delete': abp.auth.hasPermission('Cms.Widget.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditWidgetModal'
        });

        let pageThemeId = $('#PageThemeIdSelector');
        baseHelper.SimpleSelector(pageThemeId, app.localize('NoneSelect'), "/Cms/GetPagedPageThemes")
        
        let getFilter = function () {
            return {
                filter: _$WidgetsTableFilter.val(),
                pageThemeId: pageThemeId.val()
            };
        };

        let dataTable = _$WidgetsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _WidgetsService.getAll,
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
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.widget.id});
                                }
                            },
                            {
                                icon: baseHelper.SimpleTableIcon('delete'),
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.widget, _WidgetsService, getWidgets);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "widget.name",
                    name: "name"
                },
                {
                    targets: 2,
                    data: "widget.actionName",
                    name: "actionName"
                },
                {
                    targets: 3,
                    data: "widget.contentType",
                    name: "contentType",
                    class: "text-center",
                    render: function(contentType) {
                        return baseHelper.ShowWidgetContentType(contentType);
                    }
                },
                {
                    targets: 4,
                    data: "widget.contentCount",
                    name: "contentCount",
                    class: "text-center",
                    render: function(contentCount) {
                        return baseHelper.ShowNumber(contentCount);
                    }
                },
                {
                    targets: 5,
                    data: "widget.about",
                    name: "about"
                },
                {
                    targets: 6,
                    data: "widget.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getWidgets() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getWidgets);
        }
        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        if (_createMultiButton) {
            _createMultiButton.click(function() {
                _createOrEditModal.open({
                    multiInsert: true
                });
            });
        }

        abp.event.on('app.createOrEditWidgetModalSaved', getWidgets);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getWidgets();
            }
        });
    });
})();