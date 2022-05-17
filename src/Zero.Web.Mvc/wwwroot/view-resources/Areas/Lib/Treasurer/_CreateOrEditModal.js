(function ($) {
    app.modals.CreateOrEditTreasurerModal = function () {

        const _TreasurerService = abp.services.app.treasurer;

        let _modalManager;
        let _$TreasurerInformationForm = null;

        let modal;
        let rfidTypeSelector;

        let _imageWrap;
        let _imageHolder;
        let _changeImageButton;
        let _cancelImageButton;
        let _imageValue;
        let uploadFileInput;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            _imageWrap = modal.find('#AvatarWrap');
            _imageHolder = modal.find('#AvatarHolder');
            _changeImageButton = modal.find('#ChangeAvatar');
            _cancelImageButton = modal.find('#CancelAvatar');
            _imageValue = modal.find('#Avatar');
            uploadFileInput = modal.find(".uploadFileInput");

            baseHelper.SelectSingleFile("*.jpg;*.png", _changeImageButton, _cancelImageButton, null,_imageValue, _imageHolder, _imageWrap,"kt-avatar--changed");

            _cancelImageButton.on("click",function (){
                uploadFileInput.val("");
                _imageValue.val("");
                _imageHolder.fadeIn("fast").attr('src',"/Common/Images/default-profile-picture.png");
            })

            rfidTypeSelector = modal.find('#RfidTypeId');
            rfidTypeSelector.select2({
                placeholder: app.localize('NoneSelect'),
                allowClear: true,
                width: '100%',
                language: baseHelper.Select2Language(),
                ajax: {
                    url: abp.appPath + "api/services/app/Lib/GetPagedRfidTypes",
                    dataType: 'json',
                    delay: 50,
                    data: function (params) {
                        return {
                            filter: params.term
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;

                        let res = $.map(data.result.items, function (item) {
                            return {
                                text: item.cardNumber,
                                id: item.id,
                            }
                        });

                        return {
                            results: res,
                            pagination: {
                                more: (params.page * 10) < data.result.totalCount
                            }
                        };
                    },
                    cache: true
                }
            });

            _$TreasurerInformationForm = _modalManager.getModal().find('form[name=TreasurerInformationsForm]');
            _$TreasurerInformationForm.validate();
        };

        this.save = function () {
            if (!_$TreasurerInformationForm.valid()) {
                return;
            }

            const Treasurer = _$TreasurerInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TreasurerService.createOrEdit(
                Treasurer
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditTreasurerModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);