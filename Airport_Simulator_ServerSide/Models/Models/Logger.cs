#region using
using System.ComponentModel.DataAnnotations;
#endregion

namespace Models.Models
{
    public class Logger
    {
        #region Properties
        public int Id { get; set; }
        [Required]
        public virtual Airplane Airplane { get; set; }
        public virtual Leg? Leg { get; set; }
        [Required]
        public DateTime In { get; set; }
        public DateTime? Out { get; set; }
        #endregion
    }
}