(function () {
    $(function () {

        let _$TagsTable = $('#TagsTable');
        let _$TagsTableFilter = $('#TagsTableFilter');
        let _$TagsFormFilter = $('#TagsFormFilter');

        let _$refreshButton = _$TagsFormFilter.find("button[name='RefreshButton']");
        
        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _TagsService = abp.services.app.tags;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/Tags/';
        let _viewUrl = abp.appPath + 'Cms/Tags/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.Tags.Create'),
            edit: abp.auth.hasPermission('Cms.Tags.Edit'),
            'delete': abp.auth.hasPermission('Cms.Tags.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTagsModal'
        });
        
        let getFilter = function () {
            return {
                filter: _$TagsTableFilter.val()
            };
        };

        let dataTable = _$TagsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _TagsService.getAll,
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
                                    _createOrEditModal.open({id: data.record.tags.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.tags, _TagsService, getTags);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "tags.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "tags.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "tags.note",
                    name: "note"
                },
                {
                    targets: 5,
                    data: "tags.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getTags() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getTags);
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

        abp.event.on('app.createOrEditTagsModalSaved', getTags);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getTags();
            }
        });
    });
})();