using System;
using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.CurrencyRate
{
	public class CurrencyRateDto : EntityDto
    {
	    public DateTime Date { get; set; }
        
	    public string SourceCurrency { get; set; }
        
	    public string TargetCurrency { get; set; }
        
	    public double Rate { get; set; }
    }
}