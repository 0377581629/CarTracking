(function ($) {
    app.modals.CreateOrEditImageBlockModal = function () {

        const _ImageBlocksService = abp.services.app.imageBlock;

        let _modalManager;
        let _$ImageBlockInformationForm = null;

        let modal;
        let imageBlockGroupId;

        let _imageWrap;
        let _imageHolder;
        let _changeImageButton;
        let _cancelImageButton;
        let _imageValue;

        let _imageMobileWrap;
        let _imageMobileHolder;
        let _changeImageMobileButton;
        let _cancelImageMobileButton;
        let _imageMobileValue;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();

            _imageWrap = modal.find('#ImageWrap');
            _imageHolder = modal.find('#ImageHolder');
            _changeImageButton = modal.find('#ChangeImage');
            _cancelImageButton = modal.find('#CancelImage');
            _imageValue = modal.find('#Image');

            baseHelper.SelectSingleFile("*.jpg;*.png;*.jpeg", _changeImageButton, _cancelImageButton, null, _imageValue, _imageHolder, _imageWrap);

            _imageMobileWrap = modal.find('#ImageMobileWrap');
            _imageMobileHolder = modal.find('#ImageMobileHolder');
            _changeImageMobileButton = modal.find('#ChangeImageMobile');
            _cancelImageMobileButton = modal.find('#CancelImageMobile');
            _imageMobileValue = modal.find('#ImageMobile');

            baseHelper.SelectSingleFile("*.jpg;*.png;*.jpeg", _changeImageMobileButton, _cancelImageMobileButton, null, _imageMobileValue, _imageMobileHolder, _imageMobileWrap, "image-input-changed");
            
            imageBlockGroupId = modal.find('#ImageBlockGroupId');
            baseHelper.SimpleRequiredSelector(imageBlockGroupId, app.localize('PleaseSelect'), "/Cms/GetPagedImageBlockGroups");

            _$ImageBlockInformationForm = _modalManager.getModal().find('form[name=ImageBlockInformationsForm]');
            _$ImageBlockInformationForm.validate();
        };
        
        this.save = function () {
            
            let validSelector = _modalManager.validSelectors();
            if (!_$ImageBlockInformationForm.valid() || !validSelector) {
                return;
            }
            
            const ImageBlock = _$ImageBlockInformationForm.serializeFormToObject();
            
            _modalManager.setBusy(true);
            _ImageBlocksService.createOrEdit(
                ImageBlock
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditImageBlockModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);