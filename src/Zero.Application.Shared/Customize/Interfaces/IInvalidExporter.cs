using System.Collections.Generic;
using Zero.Dto;

namespace Zero.Interfaces
{
    public interface IInvalidExporter<T>
    {
        FileDto ExportToFile(List<T> invalidObjs, string fileName);
    }
}
