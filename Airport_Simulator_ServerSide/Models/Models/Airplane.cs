using DLL.Enums;
using DLL.ExtensionMethods;
using DLL.Interfaces;
#region using
using Models.Logic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace Models.Models
{
    public class Airplane : IAirplane
    {
        #region Properties

        #region Important Data
        public int AirplaneId { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public Guid SerialNumber { get; set; } = Guid.NewGuid();
        public AirplanesCompenies CompanyName { get; set; } = AirplanesCompenies.ElAl;

        public Countries Destenation { get; set; } = Countries.Israel;
        public Countries Origin { get; set; } = Countries.Israel;

        public bool? IsLanding { get; set; } = true;

        #endregion

        #region General Data
        public int MaxPassengers { get; set; }
        public int CrewMembers { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int CurrentWeight { get; set; }
        public int MaxWeight { get; set; }
        public int MaxSpeed { get; set; }
        public FuelState CurrentFuelState { get; set; } = FuelState.Medium;
        #endregion

        #region Table Connections
        [ForeignKey("LegId")]
        public virtual Leg CurrentLeg { get; set; } = new Leg();
        #endregion

        #endregion

        #region Fields
        [NotMapped]
        static int number;
        #endregion

        #region Contructors
        public Airplane() => Number = ++number;
        #endregion

        #region Methods
        /// <summary>
        /// Advanced this <see cref="Airplane"/> to the next leg
        /// </summary>
        /// <param name="allLegs"></param>
        /// <returns>The next leg <see cref="Leg"/> or null if there isn't</returns>
        /// 
        public Leg? NextLeg(IEnumerable<Leg> allLegs)
        {
            var nextLeg = this.NextLegLogic(CurrentLeg!.Number, allLegs);
            if (!nextLeg.IsNull() && nextLeg!.Number != CurrentLeg.Number)
            {
                if (nextLeg.IsEmpty)
                {
                    CurrentLeg.IsEmpty = true;
                    CurrentLeg = nextLeg!;
                    CurrentLeg.IsEmpty = false;
                    return nextLeg;
                }
                return CurrentLeg;
            }
            return null;
        }
        #endregion
    }
}
