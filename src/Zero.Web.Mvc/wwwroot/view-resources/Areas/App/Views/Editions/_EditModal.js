(function () {

    app.modals.EditEditionModal = function () {

        let _modalManager;
        let editionService = abp.services.app.edition;
        let $editionInformationForm = null;
        let featuresTree;

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
            _permissionsTree.init($modal.find('#PermissionFilterTree .permission-tree'),null,DashboardWidgetChange);
            
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
            editionService.updateEdition({
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