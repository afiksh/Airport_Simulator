#region using
using DLL.Enums;
#endregion

namespace DLL.Interfaces
{
    /// <summary>
    /// Interface for basic airplane
    /// </summary>
    public interface IAirplane
    {
        #region Properties
        int AirplaneId { get; set; }
        int Number { get; set; }
        Guid SerialNumber { get; }
        AirplanesCompenies CompanyName { get; set; }
        bool? IsLanding { get; set; }
        Countries Destenation { get; set; }
        Countries Origin { get; set; }
        #endregion
    }
}
