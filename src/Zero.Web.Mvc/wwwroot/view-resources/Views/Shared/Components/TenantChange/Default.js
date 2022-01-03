(function () {

    var _tenantChangeModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Account/TenantChangeModal',
        scriptUrl: abp.appPath + 'view-resources/Views/Shared/Components/TenantChange/_ChangeModal.js',
        modalClass: 'TenantChangeModal',
        modalSize: null
    });

    $('.tenant-change-component a')
        .click(function (e) {
            e.preventDefault();
            _tenantChangeModal.open({}, null, function ($modal) {
                var $tenancyNameInput = $modal.find('#TenancyName');
                var $switchToTenant = $modal.find('#SwitchToTenant');
                var $tenancyNameRequired = $modal.find('#TenancyNameRequired');
                var $saveButton = $modal.find('.save-button');
                var tenancyName = $tenancyNameInput.val();

                var validate = function (pristine) {
                    if ($switchToTenant.is(":checked") && !$tenancyNameInput.val()) {
                       $saveButton.prop('disabled', true);
                       if (!pristine) {
                           $tenancyNameRequired.removeClass("d-none");
                       }
                    } else {
                        $saveButton.prop('disabled', false);
                        if (!pristine) {
                            $tenancyNameRequired.addClass("d-none");
                        }
                    }
                };

                $switchToTenant.change(function () {
                    $tenancyNameInput.prop('disabled', !this.checked);

                    if (this.checked) {
                        $tenancyNameInput.focus();
                        $tenancyNameInput.val(tenancyName);
                        $saveButton.find('span').text(app.localize('SwitchToTheTenant'));
                    } else {
                        tenancyName = $tenancyNameInput.val();
                        $tenancyNameInput.val('');
                        $saveButton.find('span').text(app.localize('SwitchToTheHost'));
                    }

                    validate(true);
                });

                $tenancyNameInput.keyup(function() {
                    validate(false);
                });

            });
        });

    const _accountService = abp.services.app.account;
    
    $('body').on('change', '#TenantPicker', function(){
        let tenancyName = $(this).val();
        
        if (!tenancyName || tenancyName.length === 0) {
            abp.multiTenancy.setTenantIdCookie(null);
            location.reload();
            return;
        }

        abp.ui.setBusy();
        _accountService.isTenantAvailable({
            tenancyName: tenancyName
        }).done(function (result) {
            switch (result.state) {
                case 1: //Available
                    abp.multiTenancy.setTenantIdCookie(result.tenantId);
                    //location.reload();
                    return;
                case 2: //InActive
                    abp.message.warn(app.localize('TenantIsNotActive', tenancyName));
                    break;
                case 3: //NotFound
                    abp.message.warn(app.localize('ThereIsNoTenantDefinedWithName{0}', tenancyName));
                    break;
            }
        }).always(function () {
            abp.ui.clearBusy();
        });
    });
})();