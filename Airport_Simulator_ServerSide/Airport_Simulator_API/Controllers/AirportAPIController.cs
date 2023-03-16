#region using
using DLL.ExtensionMethods;
using Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using DLL.Attributes;
using Microsoft.AspNetCore.SignalR.Client;
using DataBase.Repository;
#endregion

namespace AirportSimulator_FinalProject_ServerSide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportAPIController : ControllerBase
    {
        #region DI
        private readonly Queue<Airplane> _airAirplanes;
        private readonly HubConnection _hubConnection;
        private readonly IRepository<Airplane> _repository;
        #endregion

        #region Constructors
        public AirportAPIController(Queue<Airplane> airAirplanes, IMemoryCache cache, HubConnection hubConnection, IRepository<Airplane> repository)
        {
            _airAirplanes = airAirplanes;
            _repository = repository;

            #region HubConnection
            _hubConnection = hubConnection;
            if (_hubConnection.State != HubConnectionState.Connecting && _hubConnection.State != HubConnectionState.Connected)
            {
                try
                {
                    _hubConnection.StartAsync().Wait();
                }
                catch (Exception ex)
                {
                    ex.PrintError();
                }
            }
            #endregion

            #region cache
            try
            {

                if (!cache.TryGetValue("queue", out _airAirplanes) /*|| _airAirplanes.IsNull() */|| _airAirplanes.Count == 0)
                {
                    _airAirplanes = airAirplanes;
                    IEnumerable<Airplane> airplanes;
                    lock (_repository)
                    {
                        airplanes = _repository.GetAllInAir();
                    }
                    if (airplanes.Count() != 0) airplanes.ToList().ForEach(a => _airAirplanes.Enqueue(a));
                    cache.Set("queue", _airAirplanes, TimeSpan.FromMinutes(3));
                }
            }
            catch (Exception ex)
            {
                ex.PrintError(1);
            }
            #endregion
        }
        #endregion

        #region Http

        #region Get

        [HttpGet("all")]
        public ActionResult<IAsyncEnumerable<Airplane>> GetAll()
        {
            IEnumerable<Airplane>? airplanes;
            lock (_repository)
            {
                airplanes = _repository.GetAll();
            }
            if (airplanes.IsNull())
            {
                Console.WriteLine("Problem at GetAll");
                return BadRequest();
            }
            return Ok(airplanes);
        }

        [HttpGet("allAir")]
        public ActionResult<Queue<Airplane>> GetAllInAir() => _airAirplanes;

        [HttpGet("allActive")]
        public ActionResult<IEnumerable<Airplane>> GetAllActive()
        {
            IEnumerable<Airplane>? airplanes;
            lock (_repository)
            {
                airplanes = _repository.GetAllActive();
            }
            if (airplanes.IsNull())
            {
                Console.WriteLine("Problem at GetAllActive");
                return BadRequest();
            }
            return Ok(airplanes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Airplane?>> GetById(int id)
        {
            Task<Airplane?> taskAirplane;
            lock (_repository)
            {
                taskAirplane = _repository.GetById(id);
            }
            var airplane = await taskAirplane;
            if (airplane.IsNull())
            {
                Console.WriteLine("Problem at GetById");
                return BadRequest();
            }
            return Ok(airplane);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult<Airplane>> Post(Airplane? airplane)
        {
            GenerateAirplane(airplane!);
            Task<Airplane?> taskAirplane;
            lock (_repository)
            {
                taskAirplane = _repository.Post(airplane!);
            }
            airplane = await taskAirplane;
            if (airplane.IsNull())
            {
                Console.WriteLine("Problem at Post");
                return BadRequest();
            }
            _airAirplanes.Enqueue(airplane!);
            return Ok();
        }
        #endregion

        #region Delete

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Airplane>> Delete(int id)
        {
            Task<Airplane?> taskAirplane;
            lock (_repository)
            {
                taskAirplane = _repository.Delete(id);
            }
            var airplane = await taskAirplane;
            if (airplane.IsNull())
            {
                Console.WriteLine("Problem at Delete");
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        [AirplanesTrack("AirplaneTrack")]
        public async Task<ActionResult<Airplane>> DeleteFirstInAir()
        {
            var airplane = _airAirplanes.Dequeue();
            Task<Airplane?> taskAirplane;
            lock (_repository)
            {
                taskAirplane = _repository.DeleteFirstInAir(airplane);
            }
            airplane = await taskAirplane;
            if (airplane.IsNull())
            {
                Console.WriteLine("Problem at DeleteFirstInAir");
                _ = _hubConnection.SendAsync("SendFirstAirplane");
                return BadRequest();
            }
            _ = _hubConnection.SendAsync("SendNextLeg", airplane);
            return Ok();
        }

        #endregion

        #region Put
        [HttpPut("nextLeg/{id:int}")]
        [AirplanesTrack("AirplaneTrack")]
        public async Task<ActionResult<Airplane>> UpdateLeg(int id)
        {
            Task<Airplane?> taskAirplane;
            lock (_repository)
            {
                taskAirplane = _repository.UpdateLeg(id);
            }
            var airplane = await taskAirplane;
            if (airplane.IsNull())
            {
                Console.WriteLine("Problem at UpdateLeg");
                return BadRequest();
            }

            #region HubConnection
            if (airplane!.CurrentLeg.Number == 2) _ = _hubConnection.SendAsync("SendFirstAirplane");
            if (airplane!.CurrentLeg.Number == 9) _ = _hubConnection.SendAsync("SendDeleteAirplane", airplane!.AirplaneId);
            else _ = _hubConnection.SendAsync("SendNextLeg", airplane!);
            #endregion

            return Ok(airplane);
        }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// generate the given <paramref name="airplane"/> from the default (and dto)
        /// </summary>
        /// <param name="airplane"></param>
        /// <returns></returns>
        private Airplane GenerateAirplane(Airplane airplane)
        {
            airplane.CurrentWeight = Logic.RandomNumInt(170, 220);
            airplane.MaxWeight = Logic.RandomNumInt(221, 350);
            int maxPassengers = 300;
            int crewMembers = 6;
            double height = 63.7;
            double width = 6;
            int maxSpeed = 900;
            if (airplane.MaxWeight > 310)
            {
                maxPassengers += 100;
                crewMembers += 4;
                height += 10.2;
                width += 0.2;
                maxSpeed += 50;
            }
            else if (airplane.MaxWeight > 260)
            {
                maxPassengers += 50;
                crewMembers += 2;
                height += 2.8;
                width += 0.1;
                maxSpeed += 50;
            }
            airplane.MaxPassengers = maxPassengers;
            airplane.CrewMembers = crewMembers;
            airplane.Height = height;
            airplane.Width = width;
            airplane.MaxSpeed = maxSpeed;
            return airplane;
        }
        #endregion
    }
}