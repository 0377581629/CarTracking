(function () {
    $(function () {
        let _$TechnicianTable = $('#TechnicianTable');
        let _$TechnicianTableFilter = $('#TechnicianTableFilter');
        let _$TechnicianFormFilter = $('#TechnicianFormFilter');

        let _$refreshButton = _$TechnicianFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');

        let _TechnicianService = abp.services.app.technician;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/Lib/Technician/';
        let _viewUrl = abp.appPath + 'Lib/Technician/';

        const _permissions = {
            create: abp.auth.hasPermission('Lib.Technician.Create'),
            edit: abp.auth.hasPermission('Lib.Technician.Edit'),
            'delete': abp.auth.hasPermission('Lib.Technician.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTechnicianModal'
        });

        let getFilter = function () {
            return {
                filter: _$TechnicianTableFilter.val()
            };
        };

        let dataTable = _$TechnicianTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            deferLoad: 0,
            listAction: {
                ajaxFunction: _TechnicianService.getAll,
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
                                    _createOrEditModal.open({id: data.record.technician.id});
                                }
                            },
                            {
                                icon: 'la la-trash text-danger',
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    baseHelper.Delete(data.record.technician, _TechnicianService, getTechnician);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "technician.avatar",
                    name: "avatar",
                    orderable: false,
                    width: 52,
                    render: function (avatar){
                        return baseHelper.ShowAvatar(avatar);
                    }
                },
                {
                    targets: 3,
                    data: "technician.code",
                    name: "code"
                },
                {
                    targets: 4,
                    data: "technician.name",
                    name: "name"
                },
                {
                    targets: 5,
                    data: "technician.rfidTypeCardNumber",
                    name: "rfidTypeCardNumber"
                },
                {
                    targets: 6,
                    data: "technician.phoneNumber",
                    name: "phoneNumber"
                },
                {
                    targets: 7,
                    data: "technician.email",
                    name: "email"
                },
                {
                    targets: 8,
                    data: "technician.isStopWorking",
                    name: "isStopWorking",
                    class: "text-center",
                    width: 80,
                    render: function(isStopWorking) {
                        return baseHelper.ShowActive(isStopWorking);
                    }
                }
            ]
        });

        function getTechnician() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getTechnician);
        }

        if (_createSingleButton) {
            _createSingleButton.click(function () {
                _createOrEditModal.open();
            });
        }

        abp.event.on('app.createOrEditTechnicianModalSaved', getTechnician);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getTechnician();
            }
        });
    });
})();