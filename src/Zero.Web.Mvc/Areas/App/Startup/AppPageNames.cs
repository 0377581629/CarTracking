namespace Zero.Web.Areas.App.Startup
{
    public class AppPageNames
    {
        public static class Common
        {
            public const string Dashboard = "Dashboard";
            public const string EmailTemplate = "Administration.EmailTemplate";
            
            public const string Administration = "Administration";
            public const string Roles = "Administration.Roles";
            public const string Users = "Administration.Users";
            public const string AuditLogs = "Administration.AuditLogs";
            public const string OrganizationUnits = "Administration.OrganizationUnits";
            public const string Languages = "Administration.Languages";
            public const string DemoUiComponents = "Administration.DemoUiComponents";
            public const string UiCustomization = "Administration.UiCustomization";
            public const string WebhookSubscriptions = "Administration.WebhookSubscriptions";
            public const string DynamicProperties = "Administration.DynamicProperties";
            public const string DynamicEntityProperties = "Administration.DynamicEntityProperties";
            
            public const string FilesManager = "Administration.FilesManager";
            public const string Settings = "Administration.Settings";
        }

        public static class Host
        {
            public const string Tenants = "Tenants";
            public const string Editions = "Editions";
            public const string Maintenance = "Administration.Maintenance";
            public const string Settings = "Administration.Settings.Host";
            
            public const string DashboardWidget = "DashboardWidget";
            public const string CurrencyRate = "CurrencyRate";
            
            public const string HangfireDashboard = "HangfireDashboard";
        }

        public static class Tenant
        {
            public const string Settings = "Administration.Settings.Tenant";
            public const string SubscriptionManagement = "Administration.SubscriptionManagement.Tenant";
        }
        
        public static class Cms
        {
            public const string Settings = "Cms.Settings";
            
            public const string ImageBlockGroup = "Cms.ImageBlockGroup";
            
            public const string ImageBlock = "Cms.ImageBlock";
            
            public const string Widget = "Cms.Widget";
            
            public const string Page = "Cms.Page";
            
            public const string PageLayout = "Cms.PageLayout";
            
            public const string PageTheme = "Cms.PageTheme";
            
            public const string Tags = "Cms.Tags";

            public const string MenuGroup = "Cms.MenuGroup";
            
            public const string Menu = "Cms.Menu";
            
            public const string Category = "Cms.Category";

            public const string Post = "Cms.Post";
        }
        
        public static class Lib
        {
            #region Basic

            public const string GeneralSettings = "Lib.GeneralSetting";

            public const string RfidType = "Lib.RfidType";
            
            public const string Technician = "Lib.Technician";
            
            public const string Treasurer = "Lib.Treasurer";
            
            public const string NetworkProvider = "Lib.NetworkProvider";
            
            public const string Device = "Lib.Device";
            
            public const string ManagementUnit = "Lib.ManagementUnit";

            #endregion

            #region Transport

            public const string Transport = "Lib.Transport";
            
            public const string Driver = "Lib.Driver";
            
            public const string CarType = "Lib.CarType";
            
            public const string CarGroup = "Lib.CarGroup";
            
            public const string Car = "Lib.Car";
            
            public const string PointType = "Lib.PointType";
            
            public const string Point = "Lib.Point";
            
            public const string Route = "Lib.Route";
            
            public const string AssignmentRoute = "Lib.AssignmentRoute";

            #endregion
        }
    }
}
