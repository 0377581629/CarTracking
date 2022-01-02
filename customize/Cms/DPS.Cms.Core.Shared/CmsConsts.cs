namespace DPS.Cms.Core.Shared
{
    public class CmsConsts
    {
        
        /// <summary>
        /// Default min string required field length
        /// </summary>
        public const int MinStrLength = 1;
        
        /// <summary>
        /// Default max string required field length
        /// </summary>
        public const int MaxStrLength = 256;

        /// <summary>
        /// Default min name required field length
        /// </summary>
        public const int MinNameLength = 1;
        
        /// <summary>
        /// Default max name required field length
        /// </summary>
        public const int MaxNameLength = 512;
        
        /// <summary>
        /// Default min string required field length
        /// </summary>
        public const int MinCodeLength = 1;
        /// <summary>
        /// Use for nested object
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
        /// <summary>
        /// Use for nested object
        /// </summary>
        public const int CodeUnitLength = 5;
        /// <summary>
        /// Use for nested object
        /// </summary>
        private const int MaxDepth = 16;
    }
}
