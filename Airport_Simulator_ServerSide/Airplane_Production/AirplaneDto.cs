#region using
using DLL.Enums;
using DLL.Interfaces;
#endregion

namespace Airplane_Production
{
    /// <summary>
    /// Partial Airplane class- just to make the things needed
    /// </summary>
    public class AirplaneDto : IAirplane
    {
        #region Properties
        public int AirplaneId { get; set; }
        public int Number { get; set; }
        public Guid SerialNumber { get; } = Guid.NewGuid();
        public AirplanesCompenies CompanyName { get; set; } = AirplanesCompenies.ElAl;
        public FuelState CurrentFuelState { get; set; } = FuelState.Medium;
        public Countries Destenation { get; set; } = Countries.Israel;
        public Countries Origin { get; set; } = Countries.Israel;

        public bool? IsLanding { get; set; } = true;
        #endregion

        #region Fields
        static int number;
        #endregion

        #region Contructors
        public AirplaneDto() => Number = ++number;
        #endregion
    }
}
