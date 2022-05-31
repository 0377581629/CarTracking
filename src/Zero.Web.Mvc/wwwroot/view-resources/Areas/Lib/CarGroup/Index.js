(function () {
    $(function () {
        let _$CarGroupTable = $('#CarGroupTable');
        let _$CarGroupTableFilter = $('#CarGroupTableFilter');
        let _$CarGroupFormFilter = $('#CarGroupFormFilter');

        let _$refreshButton = _$CarGroupFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _CarGroupService = abp.services.app.carGroup;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/CarGroup/';
        let _viewUrl = abp.appPath + 'Lib/CarGroup/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.CarGroup.Create'),
            edit: abp.auth.hasPermission('Lib.CarGroup.Edit'),
            'delete': abp.auth.hasPermission('Lib.CarGroup.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCarGroupModal'
        });

        let getFilter = function () {
            return {
                filter: _$CarGroupTableFilter.val()
            };
        };

        let dataTable = _$CarGroupTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _CarGroupService.getAll,
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
                                    _createOrEditModal.open({id: data.record.carGroup.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.carGroup, _CarGroupService, getCarGroup);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "carGroup.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "carGroup.name",
                    name: "name"
                },
                {
                    targets: 4,
                    data: "carGroup.description",
                    name: "description"
                },
                {
                    targets: 5,
                    data: "carGroup.isActive",
                    name: "isActive",
                    class: "text-center",
                    width: 80,
                    render: function(isActive) {
                        return baseHelper.ShowActive(isActive);
                    }
                },
                {
                    targets: 6,
                    data: "carGroup.isSpecialGroup",
                    name: "isSpecialGroup",
                    class: "text-center",
                    width: 80,
                    render: function(isSpecialGroup) {
                        return baseHelper.ShowActive(isSpecialGroup);
                    }
                },
            ]
        });

        function getCarGroup() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getCarGroup);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditCarGroupModalSaved', getCarGroup);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getCarGroup();
            }
        });
    });
})();