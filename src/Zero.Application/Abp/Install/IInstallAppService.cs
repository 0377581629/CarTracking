using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.Install.Dto;

namespace Zero.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}