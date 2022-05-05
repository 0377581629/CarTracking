namespace Zero.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";
        
        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

        #region Abp Customize
        public const string Dashboard = "Pages.Dashboard";
        
        public const string DashboardWidget = "Pages.DashboardWidget";
        public const string DashboardWidget_Create = "Pages.DashboardWidget.Create";
        public const string DashboardWidget_Edit = "Pages.DashboardWidget.Edit";
        public const string DashboardWidget_Delete = "Pages.DashboardWidget.Delete";
        
        public const string Pages_EmailTemplates = "Pages.EmailTemplates";
        public const string Pages_EmailTemplates_Create = "Pages.EmailTemplates.Create";
        public const string Pages_EmailTemplates_Edit = "Pages.EmailTemplates.Edit";
        public const string Pages_EmailTemplates_Delete = "Pages.EmailTemplates.Delete";

        public const string CurrencyRate = "Pages.CurrencyRate";
        public const string CurrencyRate_Create = "Pages.CurrencyRate.Create";
        public const string CurrencyRate_Edit = "Pages.CurrencyRate.Edit";
        public const string CurrencyRate_Delete = "Pages.CurrencyRate.Delete";
        
        #endregion
    }
    
    public static class CommonPermissions
    {
        public const string Settings = "Administration.Settings";
    }
    
    public static class CmsPermissions
    {
        public static string Settings = "Cms.Settings";
        
        public const string ImageBlockGroup = "Cms.ImageBlockGroup";
        public const string ImageBlockGroup_Create = "Cms.ImageBlockGroup.Create";
        public const string ImageBlockGroup_Edit = "Cms.ImageBlockGroup.Edit";
        public const string ImageBlockGroup_Delete = "Cms.ImageBlockGroup.Delete";
        
        public const string ImageBlock = "Cms.ImageBlock";
        public const string ImageBlock_Create = "Cms.ImageBlock.Create";
        public const string ImageBlock_Edit = "Cms.ImageBlock.Edit";
        public const string ImageBlock_Delete = "Cms.ImageBlock.Delete";
        
        public const string Page = "Cms.Page";
        public const string Page_Create = "Cms.Page.Create";
        public const string Page_Edit = "Cms.Page.Edit";
        public const string Page_Delete = "Cms.Page.Delete";
        
        public const string PageLayout = "Cms.PageLayout";
        public const string PageLayout_Create = "Cms.PageLayout.Create";
        public const string PageLayout_Edit = "Cms.PageLayout.Edit";
        public const string PageLayout_Delete = "Cms.PageLayout.Delete";
        
        public const string PageTheme = "Cms.PageTheme";
        public const string PageTheme_Create = "Cms.PageTheme.Create";
        public const string PageTheme_Edit = "Cms.PageTheme.Edit";
        public const string PageTheme_Delete = "Cms.PageTheme.Delete";
        
        public const string Widget = "Cms.Widget";
        public const string Widget_Create = "Cms.Widget.Create";
        public const string Widget_Edit = "Cms.Widget.Edit";
        public const string Widget_Delete = "Cms.Widget.Delete";
        
        public const string Category = "Cms.Category";
        public const string Category_Create = "Cms.Category.Create";
        public const string Category_Edit = "Cms.Category.Edit";
        public const string Category_Delete = "Cms.Category.Delete";
        
        public const string Post = "Cms.Post";
        public const string Post_Publish = "Cms.Post.Publish";
        public const string Post_Create = "Cms.Post.Create";
        public const string Post_Edit = "Cms.Post.Edit";
        public const string Post_Delete = "Cms.Post.Delete";
        
        public const string Tags = "Cms.Tags";
        public const string Tags_Create = "Cms.Tags.Create";
        public const string Tags_Edit = "Cms.Tags.Edit";
        public const string Tags_Delete = "Cms.Tags.Delete";
        
        public const string MenuGroup = "Cms.MenuGroup";
        public const string MenuGroup_Create = "Cms.MenuGroup.Create";
        public const string MenuGroup_Edit = "Cms.MenuGroup.Edit";
        public const string MenuGroup_Delete = "Cms.MenuGroup.Delete";
        
        public const string Menu = "Cms.Menu";
        public const string Menu_Create = "Cms.Menu.Create";
        public const string Menu_Edit = "Cms.Menu.Edit";
        public const string Menu_Delete = "Cms.Menu.Delete";
    }
    
    public static class LibPermissions
    {
        public const string GeneralSettings = "Lib.GeneralSetting";
        
        #region Basic

        public const string Categories = "Lib.Categories";
        
        public const string RfidType = "Lib.RfidType";
        public const string RfidType_Create = "Lib.RfidType.Create";
        public const string RfidType_Edit = "Lib.RfidType.Edit";
        public const string RfidType_Delete = "Lib.RfidType.Delete";

        #endregion
    }
}