(function ($) {
    app.modals.CreateOrEditRfidTypeModal = function () {

        const _RfidTypeService = abp.services.app.rfidType;

        let _modalManager;
        let _$RfidTypeInformationForm = null;

        let modal;
        let userSelector;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            userSelector = modal.find('#UserId');
            userSelector.select2({
                placeholder: app.localize('NoneSelect'),
                allowClear: true,
                width: '100%',
                language: baseHelper.Select2Language(),
                ajax: {
                    url: abp.appPath + "api/services/app/Lib/GetPagedUsers",
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
                                text: item.userName,
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

            _$RfidTypeInformationForm = _modalManager.getModal().find('form[name=RfidTypeInformationsForm]');
            _$RfidTypeInformationForm.validate();
        };

        this.save = function () {
            if (!_$RfidTypeInformationForm.valid()) {
                return;
            }

            const RfidType = _$RfidTypeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _RfidTypeService.createOrEdit(
                RfidType
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRfidTypeModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);