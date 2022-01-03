(function () {
    $(function () {

        let _$PageLayoutsTable = $('#PageLayoutsTable');
        let _$PageLayoutsTableFilter = $('#PageLayoutsTableFilter');
        let _$PageLayoutsFormFilter = $('#PageLayoutsFormFilter');

        let _$refreshButton = _$PageLayoutsFormFilter.find("button[name='RefreshButton']");
        
        let _createSingleButton = $('#CreateNewButton');

        let _PageLayoutsService = abp.services.app.pageLayout;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/PageLayout/';
        let _viewUrl = abp.appPath + 'Cms/PageLayout/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.PageLayout.Create'),
            edit: abp.auth.hasPermission('Cms.PageLayout.Edit'),
            'delete': abp.auth.hasPermission('Cms.PageLayout.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPageLayoutModal'
        });


        let pageThemeId = $('#PageThemeIdSelector');
        baseHelper.SimpleSelector(pageThemeId, app.localize('NoneSelect'), "/Cms/GetPagedPageThemes")
        
        let getFilter = function () {
            return {
                filter: _$PageLayoutsTableFilter.val(),
                pageThemeId: pageThemeId.val()
            };
        };

        let dataTable = _$PageLayoutsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _PageLayoutsService.getAll,
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
                    width: 150,
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
                                icon: 'la la-cog text-info',
                                text: app.localize('ConfigLayout'),
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location = "/Cms/PageLayout/Config?id=" + data.record.pageLayout.id;
                                }
                            },
                            {
                                icon: 'la la-edit text-primary',
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.pageLayout.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.pageLayout, _PageLayoutsService, getPageLayouts);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "pageLayout.pageThemeName",
                    name: "pageThemeName"
                },
                {
                    targets: 3,
                    data: "pageLayout.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "pageLayout.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getPageLayouts() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getPageLayouts);
        }
        
        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditPageLayoutModalSaved', getPageLayouts);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getPageLayouts();
            }
        });
    });
})();