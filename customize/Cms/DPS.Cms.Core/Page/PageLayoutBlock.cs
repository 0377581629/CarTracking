using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Extensions;
using JetBrains.Annotations;
using Zero;

namespace DPS.Cms.Core.Page
{
    [Table("Cms_Page_Layout_Block")]
    public class PageLayoutBlock : Entity
    {
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string UniqueId { get; set; }
        
        public int PageLayoutId { get; set; }
        
        [ForeignKey("PageLayoutId")]
        public virtual PageLayout PageLayout { get; set; }
        
        public int ColumnCount { get; set; }
        
        public bool WrapInRow { get; set; }
        
        public int Order { get; set; }

        public int? ParentLayoutBlockId { get; set; }
        
        [ForeignKey("ParentLayoutBlockId")]
        [CanBeNull]
        public virtual PageLayoutBlock ParentLayoutBlock { get; set; }

        public string ParentColumnUniqueId { get; set; }

        #region Block Attribute
        
        public string Col1Id { get; set; }
        
        public string Col1UniqueId { get; set; }
        
        public string Col1Class { get; set; }
        
        public string Col2Id { get; set; }
        
        public string Col2UniqueId { get; set; }
        
        public string Col2Class { get; set; }
        
        public string Col3Id { get; set; }
        
        public string Col3UniqueId { get; set; }
        
        public string Col3Class { get; set; }
        
        public string Col4Id { get; set; }
        
        public string Col4UniqueId { get; set; }
        
        public string Col4Class { get; set; }
        
        #endregion
        
        #region Static methods

        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers
                .Select(number => number.ToString(new string('0', ZeroConst.CodeUnitLength)))
                .JoinAsString(".");
        }

        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            return code.Length == parentCode.Length ? null : code.Substring(parentCode.Length + 1);
        }

        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splitCode = code.Split('.');
            return splitCode[^1];
        }

        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splitCode = code.Split('.');
            return splitCode.Length == 1 ? null : splitCode.Take(splitCode.Length - 1).JoinAsString(".");
        }

        #endregion
    }
}