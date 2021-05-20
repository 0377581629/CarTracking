using System.Collections.Generic;
using Zero.Authorization.Users.Importing.Dto;
using Zero.Dto;

namespace Zero.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
