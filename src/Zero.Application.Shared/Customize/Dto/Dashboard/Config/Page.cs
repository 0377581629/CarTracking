using System;
using System.Collections.Generic;
using Zero.Customize.Dto.Dashboard.DashboardWidget;

namespace Zero.Customize.Dto.Dashboard.Config
{
    public class Page
    {
        public string Id { get; set; }

        //Page name is not a localized string. because every user define their page with the page name they want
        public string Name { get; set; }

        public List<DashboardWidgetDto> Widgets { get; set; }

        public Page()
        {
            Id = "Page" + Guid.NewGuid().ToString().Replace("-", "");
        }

        public Page(string id)
        {
            Id = id;
        }
    }
}
