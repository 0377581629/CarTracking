(function ($) {
    app.modals.CreateOrEditAssignmentRouteModal = function () {

        const _AssignmentRouteService = abp.services.app.assignmentRoute;

        let _modalManager;
        let _$AssignmentRouteInformationForm = null;

        let modal;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            modal = _modalManager.getModal();

            _modalManager.initControl();

            let managementUnitSelector = modal.find('#ManagementUnitId');
            baseHelper.SimpleSelector(managementUnitSelector, app.localize('NoneSelect'), 'Lib/GetPagedManagementUnits', true);

            let carSelector = modal.find('#CarId');
            carSelector.select2({
                placeholder: app.localize('NoneSelect'),
                allowClear: true,
                width: '100%',
                language: baseHelper.Select2Language(),
                ajax: {
                    url: abp.appPath + "api/services/app/Lib/GetPagedCars",
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
                                text: item.code + "-" + item.licensePlate,
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

            let driverSelector = modal.find('#DriverId');
            baseHelper.SimpleSelector(driverSelector, app.localize('NoneSelect'), 'Lib/GetPagedDrivers', true);

            let routeSelector = modal.find('#RouteId');
            baseHelper.SimpleSelector(routeSelector, app.localize('NoneSelect'), 'Lib/GetPagedRoutes', true);

            let treasurerSelector = modal.find('#TreasurerId');
            baseHelper.SimpleSelector(treasurerSelector, app.localize('NoneSelect'), 'Lib/GetPagedTreasurers', true);

            let technicianSelector = modal.find('#TechnicianId');
            baseHelper.SimpleSelector(technicianSelector, app.localize('NoneSelect'), 'Lib/GetPagedTechnicians', true);

            _$AssignmentRouteInformationForm = _modalManager.getModal().find('form[name=AssignmentRouteInformationsForm]');
            _$AssignmentRouteInformationForm.validate();
        };

        function GetDayOfWeeks() {
            let _selectedIds = [];
            $('.dayOfWeekChk').each(function () {
                if ($(this).prop('checked') === true) {
                    _selectedIds.push($(this).val());
                }
            });
            return _selectedIds.join();
        }

        this.save = function () {
            if (!_$AssignmentRouteInformationForm.valid()) {
                return;
            }

            const AssignmentRoute = _$AssignmentRouteInformationForm.serializeFormToObject();

            AssignmentRoute.dayOfWeeks = GetDayOfWeeks();
            console.log('AssignmentRoute.dayOfWeeks = ',AssignmentRoute.dayOfWeeks);

            _modalManager.setBusy(true);
            _AssignmentRouteService.createOrEdit(
                AssignmentRoute
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAssignmentRouteModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);