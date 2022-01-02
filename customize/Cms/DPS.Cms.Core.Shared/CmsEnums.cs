using Zero.Extensions;

namespace DPS.Cms.Core.Shared
{
    public class CmsEnums
    {
        public enum PageLayout
        {
            [StringValue("PageLayout_SidebarLeft")]
            SidebarLeft = 1,

            [StringValue("PageLayout_SidebarRight")]
            SidebarRight = 2,
            
            [StringValue("PageLayout_FullWidth")]
            FullWidth = 3,
            
            [StringValue("PageLayout_ThreeColumns")]
            ThreeColumns = 4,
        }

        public enum WidgetContentType
        {
            [StringValue("WidgetContentType_FixedContent")]
            FixedContent = 1,
                
            [StringValue("WidgetContentType_Service")]
            Service = 2,

            [StringValue("WidgetContentType_ServiceType")]
            ServiceType = 3,
            
            [StringValue("WidgetContentType_ServiceCategory")]
            ServiceCategory = 4,
            
            [StringValue("WidgetContentType_ServiceArticle")]
            ServiceArticle = 5,
            
            [StringValue("WidgetContentType_ReviewPost")]
            ReviewPost = 6,
                
            [StringValue("WidgetContentType_ImageBlock")]
            ImageBlock = 7,
            
            [StringValue("WidgetContentType_CustomContent")]
            CustomContent = 8,
            
            [StringValue("WidgetContentType_ServicePropertyGroup")]
            ServicePropertyGroup = 9,
            
            [StringValue("WidgetContentType_ServiceBrand")]
            ServiceBrand = 10,
            
            [StringValue("WidgetContentType_ServiceVendor")]
            ServiceVendor = 11,
            
            [StringValue("WidgetContentType_MenuGroup")]
            MenuGroup = 12,
        }
    }
}