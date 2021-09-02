(function () {

    app.modals.CreateEditionModal = function () {

        var _modalManager;
        var editionService = abp.services.app.edition;
        var $editionInformationForm = null;
        var featuresTree;

        let _permissionsTree = null;
        let $modal;
        let dashboardWidgetGroup;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            $modal = _modalManager.getModal();

            dashboardWidgetGroup = $modal.find('.dashboardWidgetGroup');
            
            featuresTree = new FeaturesTree();
            featuresTree.init($modal.find('.feature-tree'));

            _permissionsTree = new PermissionsTree()
            _permissionsTree.init($modal.find('#PermissionFilterTree .permission-tree'), null,DashboardWidgetChange);

            let $editionItemsDiv = $modal.find('.edition-list');
            let $priceDivs = $modal.find('.SubscriptionPrice');
            let $trialDayCountDiv = $modal.find('.trial-day-count');
            let $waitingDayAfterExpireDiv = $modal.find('.waiting-day-after-expire');
            let $paidFeatures = $modal.find('.paid-features');
            let $dailyPrice = $modal.find('.paid-features > .SubscriptionPrice input[name="DailyPrice"]');
            let $weeklyPrice = $modal.find('.paid-features > .SubscriptionPrice input[name="WeeklyPrice"]');
            let $monthlyPrice = $modal.find('.paid-features > .SubscriptionPrice input[name="MonthlyPrice"]');
            let $annualPrice = $modal.find('.paid-features > .SubscriptionPrice input[name="AnnualPrice"]');

            function toggleEditionItems() {
                if (!$modal.find('#EditEdition_ExpireAction_AssignEdition').is(':checked')) {
                    $editionItemsDiv.slideUp('fast');
                } else {
                    $editionItemsDiv.slideDown('fast');
                }
            }

            function togglePriceDivs() {
                if (!$modal.find('#EditEdition_IsPaid').is(':checked')) {
                    $priceDivs.slideUp('fast');
                    $paidFeatures.slideUp('fast');

                    $priceDivs.find('input').attr('required', false);
                } else {
                    $priceDivs.slideDown('fast');
                    $paidFeatures.slideDown('fast');

                    $priceDivs.find('input').attr('required', true);
                }
            }

            function toggleTrialDayCount() {
                if (!$modal.find('#EditEdition_IsTrialActive').is(':checked')) {
                    $trialDayCountDiv.slideUp('fast');
                } else {
                    $trialDayCountDiv.slideDown('fast');
                }
            }

            function toggleWaitingDayAfterExpire() {
                if (!$modal.find('#EditEdition_IsWaitingDayActive').is(':checked')) {
                    $waitingDayAfterExpireDiv.slideUp('fast');
                } else {
                    $waitingDayAfterExpireDiv.slideDown('fast');
                }
            }

            function createCurrencyInputs() {
                var opts = {
                    radixPoint: ".",
                    groupSeparator: ",",
                    digits: 2,
                    autoGroup: true,
                    prefix: $('input[name="currency"]').val() + ' ',
                    rightAlign: false,
                    showMaskOnHover: false,
                    showMaskOnFocus: false,
                    placeholder: "_",
                    removeMaskOnSubmit: true,
                    autoUnmask: true
                };

                $dailyPrice.inputmask("numeric", $.extend({}, opts));
                $weeklyPrice.inputmask("numeric", $.extend({}, opts));
                $monthlyPrice.inputmask("numeric", $.extend({}, opts));
                $annualPrice.inputmask("numeric", $.extend({}, opts));
            }

            $modal.find('input[name=ExpireAction]').change(function () {
                toggleEditionItems();
            });

            $modal.find('input[name=SubscriptionPrice]').change(function () {
                togglePriceDivs();
            });

            $modal.find('#EditEdition_IsTrialActive').change(function () {
                toggleTrialDayCount();
            });

            $modal.find('#EditEdition_IsWaitingDayActive').change(function () {
                toggleWaitingDayAfterExpire();
            });

            toggleEditionItems();
            togglePriceDivs();
            toggleTrialDayCount();
            toggleWaitingDayAfterExpire();
            createCurrencyInputs();

            $editionInformationForm = _modalManager.getModal().find('form[name=EditionInformationsForm]');
            $editionInformationForm.validate();
        };

        function DashboardWidgetChange(selectedPermissions) {
            if (selectedPermissions && jQuery.inArray( "Pages.Dashboard", selectedPermissions ) > -1) {
                dashboardWidgetGroup.removeClass('hidden');
            } else {
                dashboardWidgetGroup.addClass('hidden');
            }
        }

        function GetDashboardWidget() {
            let res = [];
            $modal.find('.dashboardWidgetChecker').each(function(){
                if ($(this).prop('checked') === true)
                    res.push($(this).attr('widgetId'))
            });
            return res;
        }
        
        this.save = function () {
            if (!$editionInformationForm.valid()) {
                return;
            }

            if (!featuresTree.isValid()) {
                abp.message.warn(app.localize('InvalidFeaturesWarning'));
                return;
            }

            var edition = $editionInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            editionService.createEdition({
                edition: edition,
                featureValues: featuresTree.getFeatureValues(),
                grantedPermissionNames: _permissionsTree.getSelectedPermissionNames(),
                grantedDashboardWidgets: GetDashboardWidget()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditEditionModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})();