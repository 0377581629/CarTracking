using System.Collections.Generic;
using Zero.Customize.Dto.Dashboard.Config;

namespace Zero.Customize.Dto.Dashboard.Function
{
    public class SavePageInput
    {
        public string DashboardName { get; set; }

        public string Application { get; set; }

        public List<Page> Pages { get; set; }
    }
}
