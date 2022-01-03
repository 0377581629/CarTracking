(function () {
    $(function () {

        let _$ImageBlockGroupsTable = $('#ImageBlockGroupsTable');
        let _$ImageBlockGroupsTableFilter = $('#ImageBlockGroupsTableFilter');
        let _$ImageBlockGroupsFormFilter = $('#ImageBlockGroupsFormFilter');

        let _$refreshButton = _$ImageBlockGroupsFormFilter.find("button[name='RefreshButton']");
        
        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _ImageBlockGroupsService = abp.services.app.imageBlockGroup;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/ImageBlockGroup/';
        let _viewUrl = abp.appPath + 'Cms/ImageBlockGroup/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.ImageBlockGroup.Create'),
            edit: abp.auth.hasPermission('Cms.ImageBlockGroup.Edit'),
            'delete': abp.auth.hasPermission('Cms.ImageBlockGroup.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditImageBlockGroupModal'
        });
        
        let getFilter = function () {
            return {
                filter: _$ImageBlockGroupsTableFilter.val()
            };
        };

        let dataTable = _$ImageBlockGroupsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _ImageBlockGroupsService.getAll,
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
                                    _createOrEditModal.open({id: data.record.imageBlockGroup.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.imageBlockGroup, _ImageBlockGroupsService, getImageBlockGroups);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "imageBlockGroup.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "imageBlockGroup.note",
                    name: "note"
                },
                {
                    targets: 4,
                    data: "imageBlockGroup.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getImageBlockGroups() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getImageBlockGroups);
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

        abp.event.on('app.createOrEditImageBlockGroupModalSaved', getImageBlockGroups);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getImageBlockGroups();
            }
        });
    });
})();