using System;

namespace Zero.KendoUI
{
    public partial class KendoModels
    {
        public class SchedulerEvent
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            
            public bool IsAllDay { get; set; }
            
            public object RecurrenceId { get; set; }
            public string RecurrenceRule { get; set; }
            public string RecurrenceException { get; set; }
        }
    }
}