using System;

namespace Zero.Dto
{
    public class ImportFromFileDto
    {
        /// <summary>
        /// Can be set when reading data from excel or when importing user
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported => string.IsNullOrEmpty(Exception);
    }
}