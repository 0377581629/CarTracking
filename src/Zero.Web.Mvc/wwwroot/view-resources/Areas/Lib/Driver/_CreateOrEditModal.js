(function ($) {
    app.modals.CreateOrEditDriverModal = function () {

        const _DriverService = abp.services.app.driver;

        let _modalManager;
        let _$DriverInformationForm = null;

        let modal;
        let rfidTypeSelector;
        let deviceSelector;

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

            deviceSelector = modal.find('#DeviceId');
            deviceSelector.select2({
                placeholder: app.localize('NoneSelect'),
                allowClear: true,
                width: '100%',
                language: baseHelper.Select2Language(),
                ajax: {
                    url: abp.appPath + "api/services/app/Lib/GetPagedDevices",
                    dataType: 'json',
                    delay: 50,
                    data: function (params) {
                        return {
                            filter: params.term
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;
                        console.log('device =',data.result.items)
                        let res = $.map(data.result.items, function (item) {
                            return {
                                text: item.code + ' - ' + item.simCard,
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

            _$DriverInformationForm = _modalManager.getModal().find('form[name=DriverInformationsForm]');
            _$DriverInformationForm.validate();
        };

        this.save = function () {
            if (!_$DriverInformationForm.valid()) {
                return;
            }

            const Driver = _$DriverInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _DriverService.createOrEdit(
                Driver
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDriverModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);