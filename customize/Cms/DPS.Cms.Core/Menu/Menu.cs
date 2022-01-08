using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Extensions;
using JetBrains.Annotations;
using Zero;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Menu
{
    [Table("Cms_Menu")]
    public class Menu: SimpleEntity, IMayHaveTenant
    {
        #region Constructor
        
        public Menu()
        {
            
        }

        public Menu(string name, int? parentId = null)
        {
            Name = name;
            ParentId = parentId;
        }
        
        #endregion
        
        public int? TenantId { get; set; }
        
        public int MenuGroupId { get; set; }
        
        [ForeignKey("MenuGroupId")]
        public virtual MenuGroup MenuGroup { get; set; }
        
        public int? ParentId { get; set; }
        
        [ForeignKey("ParentId")]
        [CanBeNull]
        public virtual Menu Parent { get; set; }

        public string Url { get; set; }
        
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