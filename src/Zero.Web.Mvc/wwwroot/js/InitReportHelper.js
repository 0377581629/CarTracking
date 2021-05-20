var reportHelper = reportHelper || {};

(function () {
    $(function () {
        $.fn.exists = function () {
            return this.length !== 0;
        }
        reportHelper.InitReportViewer = function(reportWrapper, reportName) {
            reportWrapper
                .telerik_ReportViewer({
                    serviceUrl: "/api/reports/",
                    templateUrl: "/3rds/telerik/reportViewerTemplate/telerikReportViewerTemplate-15.0.21.120.html",
                    viewMode: telerikReportViewer.ViewModes.PRINT_PREVIEW,
                    scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
                    scale: 1.0,
                    enableAccessibility: false
                });
            return reportWrapper.data("telerik_ReportViewer");
        }
    });
})();