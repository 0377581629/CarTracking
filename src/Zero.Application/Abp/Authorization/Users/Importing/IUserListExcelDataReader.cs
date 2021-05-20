using System.Collections.Generic;
using Zero.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Zero.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
