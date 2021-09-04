(function () {
    $(function () {

        let _$EmailTemplatesTable = $('#EmailTemplatesTable');
        let _$EmailTemplatesTableFilter = $('#EmailTemplatesTableFilter');
        let _$EmailTemplatesFormFilter = $('#EmailTemplatesFormFilter');

        let _$refreshButton = _$EmailTemplatesFormFilter.find("button[name='RefreshButton']");
        let _createSingleButton = $('#CreateNewButton');
        let _createMultiButton = $('#CreateMultiButton');

        let _emailTemplatesService = abp.services.app.emailTemplate;

        let _scriptUrl = abp.appPath + 'view-resources/Areas/App/Views/EmailTemplate/';
        let _viewUrl = abp.appPath + 'App/EmailTemplate/';

        const _permissions = {
            create: abp.auth.hasPermission('Pages.EmailTemplates.Create'),
            edit: abp.auth.hasPermission('Pages.EmailTemplates.Edit'),
            'delete': abp.auth.hasPermission('Pages.EmailTemplates.Delete')
        };

        const _createOrEditModal = new app.ModalManager({
            viewUrl: _viewUrl + 'CreateOrEditModal',
            scriptUrl: _scriptUrl + '_CreateOrEditModal.js',
            modalClass: 'CreateOrEditEmailTemplateModal'
        });

        let getFilter = function () {
            return {
                filter: _$EmailTemplatesTableFilter.val()
            };
        };

        let dataTable;
        
        if (abp.session.tenantId === null) {
            dataTable = _$EmailTemplatesTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                deferLoad: 0,
                listAction: {
                    ajaxFunction: _emailTemplatesService.getAll,
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
                                    icon: baseHelper.SimpleTableIcon('edit'),
                                    text: app.localize('Edit'),
                                    visible: function (data) {
                                        return _permissions.edit;
                                    },
                                    action: function (data) {
                                        _createOrEditModal.open({id: data.record.emailTemplate.id});
                                    }
                                },
                                {
                                    icon: baseHelper.SimpleTableIcon('delete'),
                                    text: app.localize('Delete'),
                                    visible: function (data) {
                                        return _permissions.delete;
                                    },
                                    action: function (data) {
                                        baseHelper.Delete(data.record.emailTemplate, _emailTemplatesService, getEmailTemplates);
                                    }
                                }]
                        }
                    },
                    {
                        targets: 2,
                        data: "emailTemplate.emailTemplateType",
                        name: "emailTemplateType",
                        render: function(emailTemplateType) {
                            return baseHelper.ShowEmailTemplateType(emailTemplateType);
                        }
                    },
                    {
                        targets: 3,
                        data: "emailTemplate.title",
                        name: "title"
                    },
                    {
                        targets: 4,
                        data: "emailTemplate.note",
                        name: "note"
                    },
                    {
                        targets: 5,
                        data: "emailTemplate.autoCreateForNewTenant",
                        name: "autoCreateForNewTenant",
                        class: "text-center",
                        render: function(autoCreateForNewTenant) {
                            return baseHelper.ShowActive(autoCreateForNewTenant);
                        }
                    },
                    {
                        targets: 6,
                        data: "emailTemplate.isActive",
                        name: "isActive",
                        class: "text-center",
                        render: function(isActive) {
                            return baseHelper.ShowActive(isActive);
                        }
                    }
                ]
            });
        } else {
            dataTable = _$EmailTemplatesTable.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                deferLoad: 0,
                listAction: {
                    ajaxFunction: _emailTemplatesService.getAll,
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
                                    icon: baseHelper.SimpleTableIcon('edit'),
                                    text: app.localize('Edit'),
                                    visible: function (data) {
                                        return _permissions.edit;
                                    },
                                    action: function (data) {
                                        _createOrEditModal.open({id: data.record.emailTemplate.id});
                                    }
                                },
                                {
                                    icon: baseHelper.SimpleTableIcon('delete'),
                                    text: app.localize('Delete'),
                                    visible: function (data) {
                                        return _permissions.delete;
                                    },
                                    action: function (data) {
                                        baseHelper.Delete(data.record.emailTemplate, _emailTemplatesService, getEmailTemplates);
                                    }
                                }]
                        }
                    },
                    {
                        targets: 2,
                        data: "emailTemplate.emailTemplateType",
                        name: "emailTemplateType",
                        render: function(emailTemplateType) {
                            return baseHelper.ShowEmailTemplateType(emailTemplateType);
                        }
                    },
                    {
                        targets: 3,
                        data: "emailTemplate.title",
                        name: "title"
                    },
                  
                    {
                        targets: 4,
                        data: "emailTemplate.note",
                        name: "note"
                    },
                    {
                        targets: 5,
                        data: "emailTemplate.isActive",
                        name: "isActive",
                        class: "text-center",
                        render: function(isActive) {
                            return baseHelper.ShowActive(isActive);
                        }
                    }
                ]
            });
        }
        

        function getEmailTemplates() {
            dataTable.ajax.reload();
        }

        if (_$refreshButton) {
            _$refreshButton.on('click', getEmailTemplates);
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

        abp.event.on('app.createOrEditEmailTemplateModalSaved', getEmailTemplates);

        $(document).keypress(function (e) {
            let sideBar = $('#kt_quick_sidebar');
            if (e.which === 13 && (sideBar === undefined || !sideBar.hasClass('kt-quick-panel--on'))) {
                getEmailTemplates();
            }
        });
    });
})();