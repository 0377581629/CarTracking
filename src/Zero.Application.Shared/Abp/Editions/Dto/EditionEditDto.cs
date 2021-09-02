using System.ComponentModel.DataAnnotations;

namespace Zero.Editions.Dto
{
    public class EditionEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public int? ExpiringEditionId { get; set; }
        
        #region Customize
        public bool IsDefault { get; set; }
        #endregion
    }
}