using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Zero.Abp.Authorization.Accounting;
using Zero.MultiTenancy.Accounting;
using Zero.Web.Areas.App.Models.Accounting;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    public class UserInvoiceController : ZeroControllerBase
    {
        private readonly IUserInvoiceAppService _invoiceAppService;

        public UserInvoiceController(IUserInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(long paymentId)
        {
            var invoice = await _invoiceAppService.GetInvoiceInfo(new EntityDto<long>(paymentId));
            var model = new UserInvoiceViewModel
            {
                Invoice = invoice
            };

            return View(model);
        }
    }
}