using System.Collections.Generic;
using Zero.Caching.Dto;

namespace Zero.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}