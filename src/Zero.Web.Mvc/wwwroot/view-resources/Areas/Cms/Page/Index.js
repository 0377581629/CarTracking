(function () {
    $(function () {

        let _$PagesTable = $('#PagesTable');
        let _$PagesTableFilter = $('#PagesTableFilter');
        let _$PagesFormFilter = $('#PagesFormFilter');

        let _$refreshButton = _$PagesFormFilter.find("button[name='RefreshButton']");
        
        let _createSingleButton = $('#CreateNewButton');

        let _PagesService = abp.services.app.page;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Page/';
        let _viewUrl = abp.appPath + 'Cms/Page/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.Page.Create'),
            edit: abp.auth.hasPermission('Cms.Page.Edit'),
            'delete': abp.auth.hasPermission('Cms.Page.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPageModal'
        });

        let pageThemeId = $('#PageThemeIdSelector');
        baseHelper.SimpleSelector(pageThemeId, app.localize('NoneSelect'), "/Cms/GetPagedPageThemes")
        
        let getFilter = function () {
            return {
                filter: _$PagesTableFilter.val(),
                pageThemeId: pageThemeId.val()
            };
        };

        let dataTable = _$PagesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _PagesService.getAll,
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
                                text: app.localize('Config'),
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    window.location = "/Cms/Page/Config?id=" + data.record.page.id;
                                }
                            },
                            {
                                icon: 'la la-edit text-primary',
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.page.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.page, _PagesService, getPages);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "page.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "page.about",
                    name: "about"
                },
                {
                    targets: 4,
                    data: "page.slug",
                    name: "slug"
                },
                {
                    targets: 5,
                    data: "page.pageThemeName",
                    name: "pageThemeName"
                },
                {
                    targets: 6,
                    data: "page.pageLayoutName",
                    name: "pageLayoutName"
                },
                {
                    targets: 7,
                    data: "page.isHomePage",
                    name: "isHomePage",
                    class: "text-center",
                    width: 80,
                    render: function(isHomePage) {
                        return baseHelper.ShowActive(isHomePage);
                    }
                },
                {
                    targets: 8,
                    data: "page.publish",
                    name: "publish",
                    class: "text-center",
                    width: 80,
                    render: function(publish) {
                        return baseHelper.ShowActive(publish);
                    }
                },
                {
                    targets: 9,
                    data: "page.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getPages() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getPages);
        }
        
        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }
        abp.event.on('app.createOrEditPageModalSaved', getPages);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getPages();
            }
        });
    });
})();