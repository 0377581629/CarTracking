(function () {
    $(function () {
        let _$TreasurerTable = $('#TreasurerTable');
        let _$TreasurerTableFilter = $('#TreasurerTableFilter');
        let _$TreasurerFormFilter = $('#TreasurerFormFilter');

        let _$refreshButton = _$TreasurerFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _TreasurerService = abp.services.app.treasurer;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Treasurer/';
        let _viewUrl = abp.appPath + 'Lib/Treasurer/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Treasurer.Create'),
            edit: abp.auth.hasPermission('Lib.Treasurer.Edit'),
            'delete': abp.auth.hasPermission('Lib.Treasurer.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTreasurerModal'
        });

        let getFilter = function () {
            return {
                filter: _$TreasurerTableFilter.val()
            };
        };

        let dataTable = _$TreasurerTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _TreasurerService.getAll,
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
                                    _createOrEditModal.open({id: data.record.treasurer.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.treasurer, _TreasurerService, getTreasurer);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "treasurer.avatar",
                    name: "avatar",
                    orderable: false,
                    width: 52,
                    render: function (avatar){
                        return baseHelper.ShowAvatar(avatar);
                    }
                },
                {
                    targets: 3,
                    data: "treasurer.code",
                    name: "code"
                },
                {
                    targets: 4,
                    data: "treasurer.name",
                    name: "name"
                },
                {
                    targets: 5,
                    data: "treasurer.rfidTypeCardNumber",
                    name: "rfidTypeCardNumber"
                },
                {
                    targets: 6,
                    data: "treasurer.phoneNumber",
                    name: "phoneNumber"
                },
                {
                    targets: 7,
                    data: "treasurer.email",
                    name: "email"
                },
                {
                    targets: 8,
                    data: "treasurer.isStopWorking",
                    name: "isStopWorking",
                    class: "text-center",
                    width: 80,
                    render: function(isStopWorking) {
                        return baseHelper.ShowActive(isStopWorking);
                    }
                }
            ]
        });

        function getTreasurer() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getTreasurer);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditTreasurerModalSaved', getTreasurer);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getTreasurer();
            }
        });
    });
})();