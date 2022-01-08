(function () {
    app.modals.CreateOrEditCategoryModal = function () {

        let _modalManager;
        const _categoryService = abp.services.app.category;
        let _$form = null;
        let modal;

        let _imageWrap;
        let _imageHolder;
        let _changeImageButton;
        let _cancelImageButton;
        let _imageValue;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();

            _imageWrap = modal.find('#ImageWrap');
            _imageHolder = modal.find('#ImageHolder');
            _changeImageButton = modal.find('#ChangeImage');
            _cancelImageButton = modal.find('#CancelImage');
            _imageValue = modal.find('#Image');

            baseHelper.SelectSingleFile("*.jpg;*.png;*.jpeg", _changeImageButton, _cancelImageButton, null, _imageValue, _imageHolder, _imageWrap, "image-input-changed");

            _$form = _modalManager.getModal().find('form[name=CategoryInformationsForm]');
            _$form.validate({ignore: ""});
        };

        this.save = function () {
            let validSelectors = _modalManager.validSelectors();
            if (!_$form.valid() || !validSelectors) {
                return;
            }
            const Category = _$form.serializeFormToObject();

            _modalManager.setBusy(true);

            _categoryService.createOrEditCategory(
                Category
            ).done(function (result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.setResult(result);
                _modalManager.close();
                abp.event.trigger('app.createOrEditCategoryModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });


        };
    };
})();