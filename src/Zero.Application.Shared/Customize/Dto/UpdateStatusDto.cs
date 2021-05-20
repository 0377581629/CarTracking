using Abp.Application.Services.Dto;

namespace Zero.Dto
{
    public class  UpdateStatusDto : EntityDto<int?>
    {
	    public int Status { get; set; }
    }
}