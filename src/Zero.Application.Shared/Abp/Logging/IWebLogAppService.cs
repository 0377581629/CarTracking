using Abp.Application.Services;
using Zero.Dto;
using Zero.Logging.Dto;

namespace Zero.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
