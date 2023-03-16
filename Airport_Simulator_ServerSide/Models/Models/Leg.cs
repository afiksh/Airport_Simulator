#region using
using DLL.Enums;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Models.Models
{
    public class Leg
    {
        #region Properties
        public int LegId { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public double Duration { get; set; } = 2;
        [Required]
        public LegType Type { get; set; } = LegType.None;
        public bool IsEmpty { get; set; } = true;
        #endregion
    }
}