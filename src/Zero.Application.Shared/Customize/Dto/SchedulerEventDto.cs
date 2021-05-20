using System;
using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto
{
	public class SchedulerEventDto : FullAuditedEntityDto
	{
		public int? TenantId { get; set; }
		
		public string TaskId { get; set; }

		public string Title { get; set; }
        
		public DateTime? Start { get; set; }
        
		public DateTime? End { get; set; }
        
		public string Description { get; set; }
        
		public string RecurrenceId { get; set; }
        
		public string RecurrenceRule { get; set; }
        
		public string RecurrenceException { get; set; }
        
		public bool IsAllDay { get; set; }
		
		public bool ReadOnly { get; set; }
		
		public string ColorInScheduler { get; set; }
	}
}