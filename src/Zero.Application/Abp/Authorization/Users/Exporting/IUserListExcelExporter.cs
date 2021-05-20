using System.Collections.Generic;
using Zero.Authorization.Users.Dto;
using Zero.Dto;

namespace Zero.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}