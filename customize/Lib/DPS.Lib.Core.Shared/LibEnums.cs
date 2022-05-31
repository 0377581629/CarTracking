using Zero.Extensions;

namespace DPS.Lib.Core.Shared
{
    public class LibEnums
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
        
        public enum CardType
        {
            [StringValue("CardType_Treasurer")]
            Treasurer = 1,

            [StringValue("CardType_Driver")] 
            Driver = 2,

            [StringValue("CardType_Car")]
            Car = 3,
        }
        
        public enum BloodType
        {
            [StringValue("BloodType_APlus")]
            APlus = 1,

            [StringValue("BloodType_ASub")] 
            ASub = 2,
            
            [StringValue("BloodType_BPlus")]
            BPlus = 3,

            [StringValue("BloodType_BSub")] 
            BSub = 4,
            
            [StringValue("BloodType_OPlus")]
            OPlus = 5,

            [StringValue("BloodType_OSub")] 
            OSub = 6,
            
            [StringValue("BloodType_AbPlus")]
            AbPlus = 7,

            [StringValue("BloodType_AbSub")] 
            AbSub = 8,
        }
        
        public enum FuelType
        {
            [StringValue("FuelType_Gasoline")]
            Gasoline = 1,

            [StringValue("FuelType_Oil")] 
            Oil = 2,

            [StringValue("FuelType_Electricity")]
            Electricity = 3,
        }
    }
}