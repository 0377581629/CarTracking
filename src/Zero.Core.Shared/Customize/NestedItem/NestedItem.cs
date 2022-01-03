using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Zero.Customize.NestedItem
{
    public class  NestedItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        
        [JsonProperty("parent_id")]
        public int? ParentId { get; set; }

        [CanBeNull] public List<NestedItem> Children { get; set; }
    }
}