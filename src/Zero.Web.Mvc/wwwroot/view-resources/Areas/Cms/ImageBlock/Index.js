(function () {
    $(function () {

        let _$ImageBlocksTable = $('#ImageBlocksTable');
        let _$ImageBlocksTableFilter = $('#ImageBlocksTableFilter');
        let _$ImageBlocksFormFilter = $('#ImageBlocksFormFilter');

        let _$refreshButton = _$ImageBlocksFormFilter.find("button[name='RefreshButton']");
        
        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _ImageBlocksService = abp.services.app.imageBlock;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Cms/ImageBlock/';
        let _viewUrl = abp.appPath + 'Cms/ImageBlock/';

        const _permissions = {
            create: abp.auth.hasPermission('Cms.ImageBlock.Create'),
            edit: abp.auth.hasPermission('Cms.ImageBlock.Edit'),
            'delete': abp.auth.hasPermission('Cms.ImageBlock.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditImageBlockModal'
        });

        let imageBlockGroupSelector = $('#ImageBlockGroupSelector');
        baseHelper.SimpleRequiredSelector(imageBlockGroupSelector, app.localize('SelectAll'), "/Cms/GetPagedImageBlockGroups");
        
        let getFilter = function () {
            return {
                filter: _$ImageBlocksTableFilter.val(),
                imageBlockGroupId : imageBlockGroupSelector.val()
            };
        };

        let dataTable = _$ImageBlocksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _ImageBlocksService.getAll,
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
                                    _createOrEditModal.open({id: data.record.imageBlock.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.imageBlock, _ImageBlocksService, getImageBlocks);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "imageBlock.image",
                    name: "image",
                    width: 50,
                    render: function(image) {
                        return baseHelper.ShowImage(image);
                    }
                },
                {
                    targets: 3,
                    data: "imageBlock.imageBlockGroupName",
                    name: "imageBlockGroupName",
                    width: 120
                },
                {
                    targets: 4,
                    data: "imageBlock.name",
                    name: "name",
                    width: 200
                },
                {
                    targets: 5,
                    data: "imageBlock.targetUrl",
                    name: "targetUrl"
                },
                {
                    targets: 6,
                    data: "imageBlock.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                }
            ]
        });

        function getImageBlocks() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getImageBlocks);
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

        abp.event.on('app.createOrEditImageBlockModalSaved', getImageBlocks);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            let modalShowing = $('.modal');

            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on')) && (modalShowing === undefined || !modalShowing.hasClass('show'))) {
                getImageBlocks();
            }
        });
    });
})();