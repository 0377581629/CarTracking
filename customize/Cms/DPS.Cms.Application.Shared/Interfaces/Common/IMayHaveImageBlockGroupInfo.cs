namespace DPS.Cms.Application.Shared.Interfaces.Common
{
    public interface IMayHaveImageBlockGroupInfo
    {
        public int? ImageBlockGroupId { get; set; }
		
        public string ImageBlockGroupCode { get; set; }
		
        public string ImageBlockGroupName { get; set; }
    }
}