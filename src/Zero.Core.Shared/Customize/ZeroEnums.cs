using Zero.Extensions;

namespace Zero
{
    public class ZeroEnums
    {
        public enum EmailTemplateType
        {
            // [StringValue("EmailTemplateType_NewTenant")] NewTenant = 1,
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

        public enum DefaultAgeRange
        {
            [StringValue("AgeLess18")] Small = 1,
            [StringValue("Age1825")] Young = 2,
            [StringValue("Age2645")] Middle = 3,
            [StringValue("AgeOver45")] Old = 4
        }
        
        public enum PlanTrainingStatus
        {
            [StringValue("Draft")] Draft = 1,
            [StringValue("WaitingForApproval")] WaitToApprove = 2,
            [StringValue("Approved")] Approve = 3,
            [StringValue("Return")] Return = 4,
            [StringValue("Marked")] Marked = 5,
            [StringValue("Lock")] Lock = 6
        }

        public enum ExamStatus
        {
            [StringValue("Draft")] Draft = 1,
            [StringValue("Ready")] Ready = 2,
            [StringValue("TheEnd")] TheEnd = 3,
            [StringValue("Marked")] Marked = 4,
            [StringValue("Lock")] Lock = 5
        }
     
        public enum OrganizationSelectModal
        {
            [StringValue("WorkGroup")] WorkGroup = 1,
            [StringValue("WorkDepartment")] WorkDepartment = 2,
            [StringValue("WorkPosition")] WorkPosition = 3,
            [StringValue("WorkParty")] WorkParty = 4,
            [StringValue("WorkTeam")] WorkTeam = 5,
            [StringValue("WorkAddress")] WorkAddress = 6,
            [StringValue("WorkContent")] WorkContent = 7
        }

        #region Report

        public enum ReportPageSize
        {
            A4 = 0,
            A3 = 1,
            A5 = 2
        }

        public enum ReportOrientation
        {
            Portrait = 0,
            LandScape = 1
        }

        public enum ReportLogoPosition
        {
            Left = 0,
            Right = 1
        }

        public enum ReportTextHAlign
        {
            Left = 0,
            Center = 1,
            Right = 2
        }

        public enum ReportTextVAlign
        {
            Top = 0,
            Middle = 1,
            Bottom = 2
        }

        #endregion

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
            Image = 1,
            Audio = 2,
            Video = 3,
            Office = 4,
            Compress = 5
        }

        public enum FileTypeByApp
        {
            Word = 1,
            Excel = 2,
            PowerPoint = 3,
            Pdf = 4,
            RTF = 5,
            TXT = 6
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