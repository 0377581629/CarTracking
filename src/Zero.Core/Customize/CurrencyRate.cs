using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Zero.Customize
{
    [Table("AbpCurrencyRates")]
    public class CurrencyRate : Entity
    {
        public DateTime Date { get; set; }
        
        public string SourceCurrency { get; set; }
        
        public string TargetCurrency { get; set; }
        
        public double Rate { get; set; }
    }
}