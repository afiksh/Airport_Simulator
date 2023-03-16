#region using
using Airplane_Production;
using DLL.Enums;
using DLL.ExtensionMethods;
using System.Net;
using System.Net.Http.Json;
#endregion

#region Fields
HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:7247") };

#region Enum arrays
AirplanesCompenies[] airplanesCompenies = Enum.GetValues<AirplanesCompenies>();
Countries[] countries = Enum.GetValues<Countries>();
FuelState[] fuelState = Enum.GetValues<FuelState>();
#endregion
#endregion

#region Timer
System.Timers.Timer timer = new System.Timers.Timer(Logic.RandomNumInt(1000, 5000));
timer.Elapsed += (s, e) => PostAirplan();
timer.Start();
#endregion

#region Methods
async void PostAirplan()
{
    var airplane = new AirplaneDto
    {
        CompanyName = airplanesCompenies[Logic.RandomNumInt(airplanesCompenies.Length - 1)],
        Origin = countries[Logic.RandomNumInt(1, countries.Length - 1)],
        CurrentFuelState = fuelState[Logic.RandomNumInt(1, fuelState.Length - 1)]
    };
    try
    {
        var response = await client.PostAsJsonAsync("AirportAPI", airplane);
        if (response.StatusCode == HttpStatusCode.OK) airplane.PrintLeg();
        else Console.WriteLine(response.StatusCode.ToString());
    }
    catch (Exception ex)
    {
        ex.PrintError();
    }
    timer.Interval = Logic.RandomNumInt(1000, 5000);
}

#endregion

Console.ReadLine();