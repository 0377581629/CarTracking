namespace Zero.Customize.Dto.Dashboard.Function
{
    public class AddWidgetInput
    {
        public string WidgetId { get; set; }

        public string PageId { get; set; }

        public byte Width { get; set; }

        public byte Height { get; set; }
    }
}