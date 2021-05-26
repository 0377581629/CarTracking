(function ($) {
    app.modals.CreateOrEditDashboardWidgetModal = function () {

        const _DashboardWidgetsService = abp.services.app.dashboardWidget;

        let _modalManager;
        let _$DashboardWidgetInformationForm = null;

        let _useForAllOu;
        let _ouSelector;
        let _listUnUsedOuId;
        let modal;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();
            
           _modalManager.initControl();

            _useForAllOu = modal.find('#DashboardWidget_UseForAllOu');
            
            if (_useForAllOu) {
                _useForAllOu.on('change', function() {
                   if (_useForAllOu.prop('checked') === true) {
                       modal.find('#UnUsedGroup').addClass('hidden');
                   }else{
                       modal.find('#UnUsedGroup').removeClass('hidden');
                   }
                });
            }

            _ouSelector = modal.find('#OrganizationUnitSelector').first().kendoMultiSelect({autoClose: false}).data("kendoMultiSelect");
            _listUnUsedOuId = modal.find('#ListUnUsedOuId');

            if (_listUnUsedOuId.val() !== undefined && _listUnUsedOuId.val().length>0 && _ouSelector !== undefined) {
                _ouSelector.value(_listUnUsedOuId.val().split(","));
            }
            
            _$DashboardWidgetInformationForm = _modalManager.getModal().find('form[name=DashboardWidgetInformationsForm]');
            _$DashboardWidgetInformationForm.validate();
        };
        
        this.save = function () {
            if (!_$DashboardWidgetInformationForm.valid()) {
                return;
            }

            if (_listUnUsedOuId !== undefined && _ouSelector !== undefined)
                _listUnUsedOuId.val(_ouSelector.value().join(','));
            
            const DashboardWidget = _$DashboardWidgetInformationForm.serializeFormToObject();
            DashboardWidget.useForAllOu = _useForAllOu.prop('checked');
            _modalManager.setBusy(true);
            _DashboardWidgetsService.createOrEdit(
                DashboardWidget
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDashboardWidgetModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);