namespace DPS.Cms.Application.Shared.Interfaces.Common
{
    public interface IHaveMenuGroupInfo
    {
        public int MenuGroupId { get; set; }
		
        public string MenuGroupCode { get; set; }
		
        public string MenuGroupName { get; set; }
		
        public string MenuGroupSlug { get; set; }
		
        public string MenuGroupUrl { get; set; }
		
        public string MenuGroupImage { get; set; }
    }
}