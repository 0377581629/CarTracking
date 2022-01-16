using Zero.Extensions;

namespace Zero
{
    public class ZeroEnums
    {
        public enum EmailTemplateType
        {
            [StringValue("EmailTemplateType_NewTenant")] NewTenant = 1,
            [StringValue("EmailTemplateType_UserActiveEmail")] UserActiveEmail = 2,
            [StringValue("EmailTemplateType_UserResetPassword")] UserResetPassword = 3,
            [StringValue("EmailTemplateType_SecurityCode")] UserTwoFactorSecurityCode = 4
        }
        
        public enum DefaultStatus
        {
            [StringValue("Draft")] Draft = 1,
            [StringValue("WaitingForApproval")] WaitToApprove = 2,
            [StringValue("Approved")] Approve = 3,
            [StringValue("Return")] Return = 4,
            [StringValue("Lock")] Lock = 5
        }

        public enum DefaultProcessStatus
        {
            [StringValue("Waiting")] Waiting = 1,
            [StringValue("Running")] Running = 2,
            [StringValue("Stopped")] Stopped = 3
        }

        public enum DataType
        {
            Default = 0,
            Number = 1,
            Decimal = 2,
            DateTime = 3
        }

        public enum Gender
        {
            Male = 0,
            Female = 1
        }

        public enum FileType
        {
            Root = 0,
            Image = 1,
            Audio = 2,
            Video = 3,
            Office = 4,
            Compress = 5
        }

        public enum ImportProcess
        {
            Start = 1,
            Success = 2,
            Fail = 3,
            StartReadFile = 4,
            EndReadFile = 5,
            HasInvalidObjs = 6,
            Empty = 7,
        }
    }
}