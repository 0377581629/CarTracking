namespace DPS.Cms.Core.Shared
{
    public interface IHaveSeoInfo
    {
        public bool TitleDefault { get; set; }
        public string Title { get; set; }
        
        public bool DescriptionDefault { get; set; }
        public string Description { get; set; }
        
        public bool KeywordDefault { get; set; }
        public string Keyword { get; set; }
        
        public bool AuthorDefault { get; set; }
        public string Author { get; set; }
    }
}