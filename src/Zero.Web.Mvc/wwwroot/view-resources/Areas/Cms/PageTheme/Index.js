(function () {
    $(function () {

        let _$PageThemesTable = $('#PageThemesTable');
        let _$PageThemesTableFilter = $('#PageThemesTableFilter');
        let _$PageThemesFormFilter = $('#PageThemesFormFilter');

        let _$refreshButton = _$PageThemesFormFilter.find("button[name='RefreshButton']");

        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _PageThemesService = abp.services.app.pageTheme;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/PageTheme/';
        let _viewUrl = abp.appPath + 'Cms/PageTheme/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.PageTheme.Create'),
            edit: abp.auth.hasPermission('Cms.PageTheme.Edit'),
            'delete': abp.auth.hasPermission('Cms.PageTheme.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPageThemeModal'
        });

        let getFilter = function () {
            return {
                filter: _$PageThemesTableFilter.val()
            };
        };

        let dataTable = _$PageThemesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _PageThemesService.getAll,
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
                                    _createOrEditModal.open({id: data.record.pageTheme.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.pageTheme, _PageThemesService, getPageThemes);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "pageTheme.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "pageTheme.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "pageTheme.note",
                    name: "note"
                },
                {
                    targets: 5,
                    data: "pageTheme.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getPageThemes() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getPageThemes);
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

        abp.event.on('app.createOrEditPageThemeModalSaved', getPageThemes);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getPageThemes();
            }
        });
    });
})();