(function () {
    app.modals.CreateOrEditRoleModal = function () {

        let _modalManager;
        const _roleService = abp.services.app.role;
        let _$roleInformationForm = null;
        let _permissionsTree;
        let $modal;
        let dashboardWidgetGroup;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            $modal = _modalManager.getModal();
            dashboardWidgetGroup = $modal.find('.dashboardWidgetGroup');

            _permissionsTree = new PermissionsTree();
            _permissionsTree.init($modal.find('.permission-tree'), null, DashboardWidgetChange);
            
            _$roleInformationForm = _modalManager.getModal().find('form[name=RoleInformationsForm]');
            _$roleInformationForm.validate({ ignore: "" });
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
        this.save = function() {
            if (!_$roleInformationForm.valid()) {
                return;
            }

            var role = _$roleInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _roleService.createOrUpdateRole({
                role: role,
                grantedPermissionNames: _permissionsTree.getSelectedPermissionNames(),
                grantedDashboardWidgets: GetDashboardWidget()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRoleModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})();