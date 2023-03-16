using Microsoft.AspNetCore.SignalR;
using System.Net;
using Models.Models;
#region using
using DLL.ExtensionMethods;
#endregion

namespace Airport_Simulator_API
{
    /// <summary>
    /// The SignalR Hubs API enables connected clients to call methods on the server.
    /// The server defines methods that are called from the client and the client defines methods that are called from the server.
    /// SignalR takes care of everything required to make real-time client-to-server and server-to-client communication possible.
    /// </summary>
    public class ConnectionHub : Hub
    {
        #region Fields
        HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:7247") };
        #endregion

        #region Methods

        #region Public methods
        /// <summary>
        /// Get <paramref name="airplane"/> and make it move to the next leg
        /// </summary>
        /// <param name="airplane"></param>
        /// <returns></returns>
        public async Task SendNextLeg(Airplane? airplane)
        {
            try
            {
                //wait the time current leg asked for
                await Task.Delay((int)(airplane!.CurrentLeg.Duration * 1000)).ContinueWith(async task =>
                {
                    var response = await NextLegAgain(airplane);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        airplane = await response.Content.ReadFromJsonAsync<Airplane>();
                    }
                });
                if (!airplane.IsNull()) await SendAllClientsAsync("ReceiveAirplane", airplane);
            }
            catch (Exception ex)
            {
                ex.PrintError();
            }
        }

        /// <summary>
        /// Make first airplane from the air to land at the airport
        /// </summary>
        /// <returns></returns>
        public async Task SendFirstAirplane() => await client.DeleteAsync("AirportAPI");

        /// <summary>
        /// Send <paramref name="airplane"/> which took off the airport
        /// </summary>
        /// <param name="airplane"></param>
        /// <returns></returns>
        public async Task SendDeleteAirplane(int id)
        {
            var finalResponse = await client.DeleteAsync($"AirportAPI/{id}");
            if (finalResponse.StatusCode == HttpStatusCode.OK)
                await SendAllClientsAsync("ReceiveDeleteAirplane", id);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Advance <paramref name="airplane"/> to the next leg
        /// </summary>
        /// <param name="airplane"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> NextLegAgain(Airplane airplane)
        {
            var response = await client.PutAsync($"AirportAPI/nextLeg/{airplane.AirplaneId}", null);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                //If can't move the next leg  wait a little bit and than try again
                _ = Task.Delay(100).ContinueWith(async task =>
                {
                    return await NextLegAgain(airplane);
                });
            }
            return response;
        }

        /// <summary>
        /// Send an <paramref name="airplane"/> to all clients 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="airplane"></param>
        /// <returns></returns>
        private async Task SendAllClientsAsync(string str, Airplane airplane) => await Clients.All.SendAsync(str, airplane);
        /// <summary>
        /// Send an <paramref name="id"/> to all clients 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task SendAllClientsAsync(string str, int id) => await Clients.All.SendAsync(str, id);
        #endregion

        #endregion
    }
}