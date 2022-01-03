(function ($) {
    app.modals.CreateOrEditImageBlockGroupModal = function () {

        const _ImageBlockGroupsService = abp.services.app.imageBlockGroup;

        let _modalManager;
        let _$ImageBlockGroupInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            _$ImageBlockGroupInformationForm = _modalManager.getModal().find('form[name=ImageBlockGroupInformationsForm]');
            _$ImageBlockGroupInformationForm.validate();
        };
        
        this.save = function () {
            if (!_$ImageBlockGroupInformationForm.valid()) {
                return;
            }
            
            const ImageBlockGroup = _$ImageBlockGroupInformationForm.serializeFormToObject();
            
            _modalManager.setBusy(true);
            _ImageBlockGroupsService.createOrEdit(
                ImageBlockGroup
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditImageBlockGroupModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);