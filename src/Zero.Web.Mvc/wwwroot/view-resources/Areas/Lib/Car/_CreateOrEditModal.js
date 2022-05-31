(function ($) {
    app.modals.CreateOrEditCarModal = function () {

        const _CarService = abp.services.app.car;

        let _modalManager;
        let _$CarInformationForm = null;

        let modal;

        let cameraTable;
        let addCameraBtn;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            modal = _modalManager.getModal();
            _modalManager.initControl();
            _modalManager.bigModal();

            let rfidTypeSelector = modal.find('#RfidTypeId');
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
                                text: item.code + ' - ' + item.cardNumber,
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

            let deviceSelector = modal.find('#DeviceId');
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

            let carTypeSelector = modal.find('#CarTypeId');
            baseHelper.SimpleSelector(carTypeSelector, app.localize('NoneSelect'), 'Lib/GetPagedCarTypes', true);

            let carGroupSelector = modal.find('#CarGroupId');
            baseHelper.SimpleSelector(carGroupSelector, app.localize('NoneSelect'), 'Lib/GetPagedCarGroups', true);

            let driverSelector = modal.find('#DriverId');
            baseHelper.SimpleSelector(driverSelector, app.localize('NoneSelect'), 'Lib/GetPagedDrivers', true);

            cameraTable = modal.find('#CameraTable');
            addCameraBtn = modal.find('#btnAddDetailCamera');

            if (addCameraBtn) {
                addCameraBtn.on('click', function () {
                    $.get(abp.appPath + 'Lib/Car/NewCamera').then(function (res) {
                        cameraTable.find('#LastDetailRowCamera').before(res);
                        baseHelper.RefreshUI(cameraTable);
                    });
                });
            }

            cameraTable.on('click', '.btnDeleteDetail', function () {
                let rowId = $(this).attr('rowId');
                cameraTable.find('.detailRow[rowId="' + rowId + '"]').remove();
                baseHelper.RefreshUI(cameraTable);
            });

            baseHelper.RefreshUI(cameraTable);

            _$CarInformationForm = _modalManager.getModal().find('form[name=CarInformationsForm]');
            _$CarInformationForm.validate();
        };

        function GetCameras() {
            let details = [];
            if (cameraTable) {
                cameraTable.find('.detailRow').each(function () {
                    let rowId = $(this).attr('rowId');
                    details.push({
                        id: cameraTable.find('.detailId[rowId="' + rowId + '"]').val(),
                        name: cameraTable.find('.name[rowId="' + rowId + '"]').val(),
                        position: cameraTable.find('.position[rowId="' + rowId + '"]').val(),
                        rotation: cameraTable.find('.rotation[rowId="' + rowId + '"]').val(),
                    })
                });
            }
            return details;
        }

        this.save = function () {
            if (!_$CarInformationForm.valid()) {
                return;
            }

            const Car = _$CarInformationForm.serializeFormToObject();
            Car.cameras = GetCameras();

            _modalManager.setBusy(true);
            _CarService.createOrEdit(
                Car
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditCarModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);