using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Zero.Dto;

namespace Zero.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
