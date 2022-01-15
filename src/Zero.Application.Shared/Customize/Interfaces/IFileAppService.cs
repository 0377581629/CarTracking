using System.Threading.Tasks;
using Abp.Application.Services;

namespace Zero
{
    public interface IFileAppService : IApplicationService
    {
	    string PhysRootPath();
	    
	    string FullFilePath(string input);
	    
	    Task<string> FileFolder(string extentPath = null);
	    
	    Task<string> RootFileServerBucketName();
	    
	    Task<string> SavePath(string extentPath = null);
	    
	    string PhysicalPath(string input);
	    
	    string RelativePath(string input);
	    
	    void Copy(string fromPath, string toPath);
	    
	    string NewFileName(string fileName);
	    
	    string NewGuidFileName(string fileName);
	    
	    Task<string> SaveFile(string fileName, byte[] data);
    }
}