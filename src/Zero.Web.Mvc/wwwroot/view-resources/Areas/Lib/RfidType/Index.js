(function () {
    $(function () {
        let _$RfidTypeTable = $('#RfidTypeTable');
        let _$RfidTypeTableFilter = $('#RfidTypeTableFilter');
        let _$RfidTypeFormFilter = $('#RfidTypeFormFilter');

        let _$refreshButton = _$RfidTypeFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _RfidTypeService = abp.services.app.rfidType;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/RfidType/';
        let _viewUrl = abp.appPath + 'Lib/RfidType/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.RfidType.Create'),
            edit: abp.auth.hasPermission('Lib.RfidType.Edit'),
            'delete': abp.auth.hasPermission('Lib.RfidType.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRfidTypeModal'
        });

        let getFilter = function () {
            return {
                filter: _$RfidTypeTableFilter.val()
            };
        };

        let dataTable = _$RfidTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _RfidTypeService.getAll,
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
                                    _createOrEditModal.open({id: data.record.rfidType.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.rfidType, _RfidTypeService, getRfidType);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "rfidType.code",
                    name: "code"
                },
                {
                    targets: 3,
                    data: "rfidType.userName",
                    name: "userName"
                },
                {
                    targets: 4,
                    data: "rfidType.cardNumber",
                    name: "cardNumber"
                },
                {
                    targets: 5,
                    data: "rfidType.cardType",
                    name: "cardType",
                    render: function (cardType) {
                        if (cardType === 1)
                            return app.localize('CardType_Owner')
                        if (cardType === 2)
                            return app.localize('CardType_Driver')
                        if (cardType === 3)
                            return app.localize('CardType_Car')
                    }
                },
                {
                    targets: 6,
                    width: 80,
                    data: "rfidType.registerDate",
                    name: "registerDate",
                    class: "text-center",
                    render: function (date) {
                        return moment(date).format('L');
                    }
                },
                {
                    targets: 7,
                    data: "rfidType.serialNumber",
                    name: "serialNumber"
                },
                {
                    targets: 8,
                    data: "rfidType.isBlackList",
                    name: "isBlackList",
                    class: "text-center",
                    width: 80,
                    render: function(isBlackList) {
                        return baseHelper.ShowActive(isBlackList);
                    }
                }
            ]
        });

        function getRfidType() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getRfidType);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditRfidTypeModalSaved', getRfidType);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getRfidType();
            }
        });
    });
})();