using System.Net;
using System.Net.Mail;
using Abp.Dependency;
using Abp.Net.Mail;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace ZERO.Web.Controllers
{
    [Route("api/reports")]
    [DontWrapResult]
    public class ReportsController : ReportsControllerBase, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration, IEmailSender emailSender)
            : base(reportServiceConfiguration)
        {
            _emailSender = emailSender;
        }

        protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        {
            _emailSender.SendAsync(mailMessage);
            return HttpStatusCode.OK;
        }
    }
}