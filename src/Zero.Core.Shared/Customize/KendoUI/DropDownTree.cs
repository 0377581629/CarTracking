namespace Zero.KendoUI
{
    public partial class KendoModels
    {
        public class DropDownTreeItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Expanded { get; set; }
            public bool HasChildren { get; set; }
        }
    }
}