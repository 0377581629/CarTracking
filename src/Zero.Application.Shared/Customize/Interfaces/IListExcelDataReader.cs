using System.Collections.Generic;
using Abp.Dependency;
using Zero.Dto;

namespace Zero.Interfaces
{
    public interface IListExcelDataReader<T> : ITransientDependency
    {
        List<T> GetFromExcel(byte[] fileBytes);
    }
}