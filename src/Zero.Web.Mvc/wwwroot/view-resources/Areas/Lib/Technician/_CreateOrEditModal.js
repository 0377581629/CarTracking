(function ($) {
    app.modals.CreateOrEditTechnicianModal = function () {

        const _TechnicianService = abp.services.app.technician;

        let _modalManager;
        let _$TechnicianInformationForm = null;

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

            _$TechnicianInformationForm = _modalManager.getModal().find('form[name=TechnicianInformationsForm]');
            _$TechnicianInformationForm.validate();
        };

        this.save = function () {
            if (!_$TechnicianInformationForm.valid()) {
                return;
            }

            const Technician = _$TechnicianInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TechnicianService.createOrEdit(
                Technician
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditTechnicianModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);