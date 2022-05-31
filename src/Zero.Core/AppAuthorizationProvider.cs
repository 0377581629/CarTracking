using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Zero.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            pages.CreateChildPermission(AppPermissions.Dashboard, L("Dashboard"));

            #region Settings

            var settings = pages.CreateChildPermission(CommonPermissions.Settings, L("Settings"));

            #region Cms Settings

            var cmsSettings = settings.CreateChildPermission(CmsPermissions.Settings, L("HomePage"));

            var imageBlockGroup =
                cmsSettings.CreateChildPermission(CmsPermissions.ImageBlockGroup, L("ImageBlockGroup"));
            imageBlockGroup.CreateChildPermission(CmsPermissions.ImageBlockGroup_Create, L("Create"));
            imageBlockGroup.CreateChildPermission(CmsPermissions.ImageBlockGroup_Edit, L("Edit"));
            imageBlockGroup.CreateChildPermission(CmsPermissions.ImageBlockGroup_Delete, L("Delete"));

            var imageBlock = cmsSettings.CreateChildPermission(CmsPermissions.ImageBlock, L("ImageBlock"));
            imageBlock.CreateChildPermission(CmsPermissions.ImageBlock_Create, L("Create"));
            imageBlock.CreateChildPermission(CmsPermissions.ImageBlock_Edit, L("Edit"));
            imageBlock.CreateChildPermission(CmsPermissions.ImageBlock_Delete, L("Delete"));

            var pageTheme = cmsSettings.CreateChildPermission(CmsPermissions.PageTheme, L("PageThemes"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            pageTheme.CreateChildPermission(CmsPermissions.PageTheme_Create, L("Create"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            pageTheme.CreateChildPermission(CmsPermissions.PageTheme_Edit, L("Edit"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            pageTheme.CreateChildPermission(CmsPermissions.PageTheme_Delete, L("Delete"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            var pageLayout = cmsSettings.CreateChildPermission(CmsPermissions.PageLayout, L("PageLayouts"));
            pageLayout.CreateChildPermission(CmsPermissions.PageLayout_Create, L("Create"));
            pageLayout.CreateChildPermission(CmsPermissions.PageLayout_Edit, L("Edit"));
            pageLayout.CreateChildPermission(CmsPermissions.PageLayout_Delete, L("Delete"));

            var page = cmsSettings.CreateChildPermission(CmsPermissions.Page, L("Pages"));
            page.CreateChildPermission(CmsPermissions.Page_Create, L("Create"));
            page.CreateChildPermission(CmsPermissions.Page_Edit, L("Edit"));
            page.CreateChildPermission(CmsPermissions.Page_Delete, L("Delete"));

            var widget = cmsSettings.CreateChildPermission(CmsPermissions.Widget, L("Widgets"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            widget.CreateChildPermission(CmsPermissions.Widget_Create, L("Create"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            widget.CreateChildPermission(CmsPermissions.Widget_Edit, L("Edit"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            widget.CreateChildPermission(CmsPermissions.Widget_Delete, L("Delete"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);

            var tags = cmsSettings.CreateChildPermission(CmsPermissions.Tags, L("Tags"));
            tags.CreateChildPermission(CmsPermissions.Tags_Create, L("Create"));
            tags.CreateChildPermission(CmsPermissions.Tags_Edit, L("Edit"));
            tags.CreateChildPermission(CmsPermissions.Tags_Delete, L("Delete"));

            var menuGroup = cmsSettings.CreateChildPermission(CmsPermissions.MenuGroup, L("MenuGroup"));
            menuGroup.CreateChildPermission(CmsPermissions.MenuGroup_Create, L("Create"));
            menuGroup.CreateChildPermission(CmsPermissions.MenuGroup_Edit, L("Edit"));
            menuGroup.CreateChildPermission(CmsPermissions.MenuGroup_Delete, L("Delete"));

            var menu = cmsSettings.CreateChildPermission(CmsPermissions.Menu, L("Menu"));
            menu.CreateChildPermission(CmsPermissions.Menu_Create, L("Create"));
            menu.CreateChildPermission(CmsPermissions.Menu_Edit, L("Edit"));
            menu.CreateChildPermission(CmsPermissions.Menu_Delete, L("Delete"));

            var category = cmsSettings.CreateChildPermission(CmsPermissions.Category, L("Category"));
            category.CreateChildPermission(CmsPermissions.Category_Create, L("Create"));
            category.CreateChildPermission(CmsPermissions.Category_Edit, L("Edit"));
            category.CreateChildPermission(CmsPermissions.Category_Delete, L("Delete"));

            var post = cmsSettings.CreateChildPermission(CmsPermissions.Post, L("Post"));
            post.CreateChildPermission(CmsPermissions.Post_Publish, L("Publish"));
            post.CreateChildPermission(CmsPermissions.Post_Create, L("Create"));
            post.CreateChildPermission(CmsPermissions.Post_Edit, L("Edit"));
            post.CreateChildPermission(CmsPermissions.Post_Delete, L("Delete"));

            #endregion

            SetLibConfiguration(ref settings);

            #endregion
            
            #region Administration
            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var dashboardWidgets = administration.CreateChildPermission(AppPermissions.DashboardWidget, L("DashboardWidget"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            dashboardWidgets.CreateChildPermission(AppPermissions.DashboardWidget_Create, L("Create"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            dashboardWidgets.CreateChildPermission(AppPermissions.DashboardWidget_Edit, L("Edit"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            dashboardWidgets.CreateChildPermission(AppPermissions.DashboardWidget_Delete, L("Delete"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            
            var emailTemplates = administration.CreateChildPermission(AppPermissions.Pages_EmailTemplates, L("EmailTemplates"));
            emailTemplates.CreateChildPermission(AppPermissions.Pages_EmailTemplates_Create, L("Create"));
            emailTemplates.CreateChildPermission(AppPermissions.Pages_EmailTemplates_Edit, L("Edit"));
            emailTemplates.CreateChildPermission(AppPermissions.Pages_EmailTemplates_Delete, L("Delete"));
            
            var currencyRates = administration.CreateChildPermission(AppPermissions.CurrencyRate, L("CurrencyRate"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            currencyRates.CreateChildPermission(AppPermissions.CurrencyRate_Create, L("Create"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            currencyRates.CreateChildPermission(AppPermissions.CurrencyRate_Edit, L("Edit"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            currencyRates.CreateChildPermission(AppPermissions.CurrencyRate_Delete, L("Delete"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            
            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("Create"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("Edit"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("Delete"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("Create"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("Edit"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("Delete"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("Create"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("Edit"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("Delete"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("Create"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("Edit"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("Delete"));

            //TENANT-SPECIFIC PERMISSIONS

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("TenantSettings"));
            
            if (ZeroConst.MultiTenancyEnabled)
                administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("TenantSubscription"));

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("SystemSettings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            #endregion
        }
        
        private void SetLibConfiguration(ref Permission pages)
        {
            // Library
            var generalSettings = pages.CreateChildPermission(LibPermissions.GeneralSettings, L("Library"));

            var rfidType = generalSettings.CreateChildPermission(LibPermissions.RfidType, L("RfidType"));
            rfidType.CreateChildPermission(LibPermissions.RfidType_Create, L("Create"));
            rfidType.CreateChildPermission(LibPermissions.RfidType_Edit, L("Edit"));
            rfidType.CreateChildPermission(LibPermissions.RfidType_Delete, L("Delete"));
            
            var technician = generalSettings.CreateChildPermission(LibPermissions.Technician, L("Technician"));
            technician.CreateChildPermission(LibPermissions.Technician_Create, L("Create"));
            technician.CreateChildPermission(LibPermissions.Technician_Edit, L("Edit"));
            technician.CreateChildPermission(LibPermissions.Technician_Delete, L("Delete"));
            
            var treasurer = generalSettings.CreateChildPermission(LibPermissions.Treasurer, L("Treasurer"));
            treasurer.CreateChildPermission(LibPermissions.Treasurer_Create, L("Create"));
            treasurer.CreateChildPermission(LibPermissions.Treasurer_Edit, L("Edit"));
            treasurer.CreateChildPermission(LibPermissions.Treasurer_Delete, L("Delete"));

            var networkProvider = generalSettings.CreateChildPermission(LibPermissions.NetworkProvider, L("NetworkProvider"));
            networkProvider.CreateChildPermission(LibPermissions.NetworkProvider_Create, L("Create"));
            networkProvider.CreateChildPermission(LibPermissions.NetworkProvider_Edit, L("Edit"));
            networkProvider.CreateChildPermission(LibPermissions.NetworkProvider_Delete, L("Delete"));
            
            var device = generalSettings.CreateChildPermission(LibPermissions.Device, L("Device"));
            device.CreateChildPermission(LibPermissions.Device_Create, L("Create"));
            device.CreateChildPermission(LibPermissions.Device_Edit, L("Edit"));
            device.CreateChildPermission(LibPermissions.Device_Delete, L("Delete"));
            
            // Transport
            var transports = pages.CreateChildPermission(LibPermissions.Transport, L("Transport"));

            var driver = transports.CreateChildPermission(LibPermissions.Driver, L("Driver"));
            driver.CreateChildPermission(LibPermissions.Driver_Create, L("Create"));
            driver.CreateChildPermission(LibPermissions.Driver_Edit, L("Edit"));
            driver.CreateChildPermission(LibPermissions.Driver_Delete, L("Delete"));
            
            var carType = transports.CreateChildPermission(LibPermissions.CarType, L("CarType"));
            carType.CreateChildPermission(LibPermissions.CarType_Create, L("Create"));
            carType.CreateChildPermission(LibPermissions.CarType_Edit, L("Edit"));
            carType.CreateChildPermission(LibPermissions.CarType_Delete, L("Delete"));
            
            var carGroup = transports.CreateChildPermission(LibPermissions.CarGroup, L("CarGroup"));
            carGroup.CreateChildPermission(LibPermissions.CarGroup_Create, L("Create"));
            carGroup.CreateChildPermission(LibPermissions.CarGroup_Edit, L("Edit"));
            carGroup.CreateChildPermission(LibPermissions.CarGroup_Delete, L("Delete"));
            
            var car = transports.CreateChildPermission(LibPermissions.Car, L("Car"));
            car.CreateChildPermission(LibPermissions.Car_Create, L("Create"));
            car.CreateChildPermission(LibPermissions.Car_Edit, L("Edit"));
            car.CreateChildPermission(LibPermissions.Car_Delete, L("Delete"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConst.LocalizationSourceName);
        }
    }
}
