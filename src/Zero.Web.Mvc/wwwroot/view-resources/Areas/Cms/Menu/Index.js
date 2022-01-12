(function () {
    $(function () {

        let _$MenusTable = $('#MenusTable');
        let _$MenusTableFilter = $('#MenusTableFilter');
        let _$MenusFormFilter = $('#MenusFormFilter');

        let _$refreshButton = _$MenusFormFilter.find("button[name='RefreshButton']");

        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _MenusService = abp.services.app.menu;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Menu/';
        let _viewUrl = abp.appPath + 'Cms/Menu/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.Menu.Create'),
            edit: abp.auth.hasPermission('Cms.Menu.Edit'),
            'delete': abp.auth.hasPermission('Cms.Menu.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMenuModal'
        });

        let menuGroupSelector = $('#MenuGroupSelector');
        baseHelper.SimpleRequiredSelector(menuGroupSelector, app.localize('SelectAll'), "/Cms/GetPagedMenuGroups");

        let getFilter = function () {
            return {
                filter: _$MenusTableFilter.val(),
                menuGroupId : menuGroupSelector.val()
            };
        };

        let dataTable = _$MenusTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _MenusService.getAll,
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
                                    _createOrEditModal.open({id: data.record.menu.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.menu, _MenusService, getMenus);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "menu.menuGroupName",
                    name: "menuGroupName",
                    width: 120
                },
                {
                    targets: 3,
                    data: "menu.name",
                    name: "name",
                    width: 200
                },
                {
                    targets: 4,
                    data: "menu.url",
                    name: "url"
                },
                {
                    targets: 5,
                    data: "menu.order",
                    name: "order"
                },
                {
                    targets: 6,
                    data: "menu.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getMenus() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getMenus);
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

        abp.event.on('app.createOrEditMenuModalSaved', getMenus);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getMenus();
            }
        });
    });
})();