using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Extensions;
using DPS.Cms.Core.Shared;
using JetBrains.Annotations;
using Zero;
using Zero.Customize.Base;

namespace DPS.Cms.Core.Post
{
    [Table("Cms_Category")]
    public class Category : SimpleFullAuditedEntity, IHaveSeoInfo, IMayHaveTenant
    {
        #region Constructor
        
        public Category()
        {
            
        }

        public Category(string name, int? parentId = null)
        {
            Name = name;
            ParentId = parentId;
        }
        
        #endregion
        
        #region Properties
        public int? TenantId { get; set; }

        public string About { get; set; }
        
        public string Url { get; set; }
        
        public string Slug { get; set; }
        
        public string Image { get; set; }
        
        public int PostCount { get; set; }
        
        public int CommentCount { get; set; }
        
        public int ViewCount { get; set; }
        
        public int? ParentId { get; set; }
        
        [ForeignKey("ParentId")] 
        [CanBeNull]
        public virtual Category Parent { get; set; }

        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string CategoryCode { get; set; }
        #endregion
        
        #region SEO
        public bool TitleDefault { get; set; }
        
        public string Title { get; set; }
        
        public bool DescriptionDefault { get; set; }
        
        public string Description { get; set; }
        
        public bool KeywordDefault { get; set; }
        
        public string Keyword { get; set; }
        
        public bool AuthorDefault { get; set; }
        
        public string Author { get; set; }
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