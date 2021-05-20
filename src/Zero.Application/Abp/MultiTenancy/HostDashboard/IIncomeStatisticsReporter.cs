using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zero.MultiTenancy.HostDashboard.Dto;

namespace Zero.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}