namespace DPS.Cms.Application.Shared.Interfaces.Common
{
    public interface IMayHaveMenuGroupInfo
    {
        public int? MenuGroupId { get; set; }
		
        public string MenuGroupCode { get; set; }
		
        public string MenuGroupName { get; set; }
    }
}