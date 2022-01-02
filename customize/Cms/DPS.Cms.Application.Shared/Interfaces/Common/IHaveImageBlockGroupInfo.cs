namespace DPS.Cms.Application.Shared.Interfaces.Common
{
    public interface IHaveImageBlockGroupInfo
    {
        public int ImageBlockGroupId { get; set; }
		
        public string ImageBlockGroupCode { get; set; }
		
        public string ImageBlockGroupName { get; set; }
		
        public string ImageBlockGroupSlug { get; set; }
		
        public string ImageBlockGroupUrl { get; set; }
		
        public string ImageBlockGroupImage { get; set; }
    }
}