using System;
using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Driver
{
    public class DriverDto: FullAuditedEntityDto
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public int? RfidTypeId { get; set; }
        
        public string RfidTypeCardNumber { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public string Avatar { get; set; }
        
        public string Email { get; set; }
        
        public bool IsStopWorking { get; set; }
        
        public bool Gender { get; set; }
        
        public DateTime DoB { get; set; }
        
        public int BloodType { get; set; }
        
        public string IdNumber { get; set; }

        public string DrivingLicense { get; set; }
        
        public DateTime DrivingLicenseStartDate { get; set; }
        
        public DateTime DrivingLicenseEndDate { get; set; }
        
        public string DriverNumber { get; set; }
    }
}