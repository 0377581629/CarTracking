using System;

namespace Zero.Extensions
{
    /// <summary>
    /// This attribute is used to track invalid properties
    /// show when export invalid file.
    /// </summary>
    public class InvalidExportAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string Name { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a InvalidExportAttribute Attribute
        /// </summary>
        /// <param name="name"></param>
        public InvalidExportAttribute(string name) {
            Name = name;
        }

        public InvalidExportAttribute() { }
        #endregion
    }
}