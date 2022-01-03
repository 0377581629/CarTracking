(function () {
    $(function () {

        let _$MenuGroupsTable = $('#MenuGroupsTable');
        let _$MenuGroupsTableFilter = $('#MenuGroupsTableFilter');
        let _$MenuGroupsFormFilter = $('#MenuGroupsFormFilter');

        let _$refreshButton = _$MenuGroupsFormFilter.find("button[name='RefreshButton']");

        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _MenuGroupsService = abp.services.app.menuGroup;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/MenuGroup/';
        let _viewUrl = abp.appPath + 'Cms/MenuGroup/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.MenuGroup.Create'),
            edit: abp.auth.hasPermission('Cms.MenuGroup.Edit'),
            'delete': abp.auth.hasPermission('Cms.MenuGroup.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditMenuGroupModal'
        });

        let getFilter = function () {
            return {
                filter: _$MenuGroupsTableFilter.val()
            };
        };

        let dataTable = _$MenuGroupsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _MenuGroupsService.getAll,
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
                                    _createOrEditModal.open({id: data.record.menuGroup.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.menuGroup, _MenuGroupsService, getMenuGroups);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "menuGroup.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "menuGroup.note",
                    name: "note"
                },
                {
                    targets: 4,
                    data: "menuGroup.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getMenuGroups() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getMenuGroups);
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

        abp.event.on('app.createOrEditMenuGroupModalSaved', getMenuGroups);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getMenuGroups();
            }
        });
    });
})();