using System;

namespace Zero.Web.Areas.Reporting.Models
{
    public class ViewReportModel
    {
        public string ReportLeftHeader { get; set; }
        
        public string ReportRightHeader { get; set; }
        
        public string ReportName { get; set; }
        public string ServiceName { get; set; }
        public string EndPoint { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}