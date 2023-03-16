#region using
using DLL.Enums;
using DLL.ExtensionMethods;
using Models.Logic;
using Models.Models;
using Microsoft.Extensions.Configuration;
#endregion

namespace DataBase.Repository
{
    public class Repository : IRepository<Airplane>
    {
        #region Fields
        private readonly DataContext _context;
        private IEnumerable<Leg> _legs;
        private readonly Countries[] countries = Enum.GetValues<Countries>();
        private static Mutex mutex = new Mutex();
        #endregion

        #region Consturctors
        public Repository(DataContext context)
        {
            _context = context;
            _legs = _context.Legs.AsEnumerable();
        }
        #endregion

        #region Methods

        #region Http methods

        #region Get
        public IEnumerable<Airplane>? GetAll()
        {
            try
            {
                mutex.WaitOne();
                var airplanes = _context.Airplanes.AsEnumerable();
                if (airplanes.Count() == 0) return null;
                return airplanes;
            }
            catch
            {
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public IEnumerable<Airplane>? GetAllActive()
        {
            try
            {
                mutex.WaitOne();
                var airplanes = _context.Airplanes.Where(a => !a.IsLanding.IsNull()).AsEnumerable();
                return airplanes;
            }
            catch
            {
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public List<Airplane> GetAllInAir()
        {
            mutex.WaitOne();
            var airplanes = _context.Airplanes.Where(a => a.CurrentLeg == _legs.First()).ToList();
            mutex.ReleaseMutex();
            return airplanes;
        }

        public async Task<Airplane?> GetById(int id)
        {
            try
            {
                mutex.WaitOne();
                var airplane = await _context.Airplanes.FindAsync(id);
                return airplane;
            }
            catch
            {
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        #endregion

        #region Post
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Airplane?> Post(Airplane airplane)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                mutex.WaitOne();
                var leg1 = _legs.First();
                airplane.CurrentLeg = leg1;
                _context.Airplanes.Add(airplane);
                _context.SaveChanges();
                return airplane;
            }
            catch (Exception ex)
            {
                ex.PrintError(5, airplane);
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        #endregion

        #region Delete
        public async Task<Airplane?> Delete(int id)
        {
            try
            {
                mutex.WaitOne();
                var airplane = await _context.Airplanes.FindAsync(id);
                if (airplane.IsNull()) return null;
                await UpdateLogger(airplane!);
                airplane!.IsLanding = null;
                airplane.CurrentLeg.IsEmpty = true;
                _context.SaveChanges();
                return airplane;
            }
            catch (Exception ex)
            {
                ex.PrintError(6);
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Airplane?> DeleteFirstInAir(Airplane airplane)
        {
            try
            {
                mutex.WaitOne();
                airplane.FirstLegLogic();
                AddLogger(airplane);
                _context.SaveChanges();
                FileTrackedAirplane(airplane);
                return airplane;
            }
            catch (Exception ex)
            {
                ex.PrintError(7);
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        #endregion

        #region Put
        public async Task<Airplane?> UpdateLeg(int id)
        {
            try
            {
                mutex.WaitOne();
                var airplane = await _context.Airplanes.FindAsync(id);
                var currentLeg = airplane!.CurrentLeg;
                var nextLeg = airplane.NextLeg(_legs);
                if (nextLeg.IsNull()) return null;
                if (nextLeg == currentLeg) return null;
                await UpdateLogger(airplane, nextLeg);
                _context.SaveChanges();
                FileTrackedAirplane(airplane);
                if (airplane.CurrentLeg.Number == 8) await UpdateDestenetion(id);
                return airplane;
            }
            catch
            {
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Update destenation and origion of an <see cref="Airplane"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<Airplane?> UpdateDestenetion(int id)
        {
            try
            {
                mutex.WaitOne();
                var airplane = await _context.Airplanes.FindAsync(id);
                airplane!.Origin = Countries.Israel;
                airplane.Destenation = countries[Logic.RandomNumInt(1, countries.Length - 1)];
                airplane.IsLanding = false;
                _context.SaveChanges();
                return airplane;
            }
            catch (Exception ex)
            {
                ex.PrintError(9);
                return null;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Write to <see cref="FileStream"/> an information about <paramref name="airplane"/>
        /// </summary>
        /// <param name="airplane"></param>
        private void FileTrackedAirplane(Airplane airplane)
        {
            try
            {
                using (FileStream fs = new FileStream($"{Environment.CurrentDirectory}\\AirplaneTrack.txt", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")}: {airplane.SerialNumber}" +
                                    $" - number: {airplane.Number}" +
                                    $" - by {airplane.CompanyName.ToString()} " +
                                    $" - from {airplane.Origin.ToString()}" +
                                    $" - to {airplane.Destenation.ToString()}" +
                                    $" Enter to leg number {airplane.CurrentLeg.Number}.");
                    }
                }
            }
            catch
            {
                FileTrackedAirplane(airplane);
            }
        }

        #region Logger Functions
        /// <summary>
        /// Write to the <see cref="Logger"/>
        /// </summary>
        /// <param name="airplane"></param>
        /// <param name="leg"></param>
        private async Task UpdateLogger(Airplane airplane, Leg? leg = null)
        {
            var before = _context.Loggers.FirstOrDefault(l => l.Airplane.AirplaneId == airplane.AirplaneId && l.Out == null);
            before!.Out = DateTime.Now;
            if (!leg.IsNull()) AddLogger(airplane, leg);
            _context.SaveChanges();
        }

        /// <summary>
        /// Add <paramref name="airplane"/> to the logger, 
        /// if <paramref name="leg"/> is null- <paramref name="airplane"/> came from air to the first leg
        /// </summary>
        /// <param name="airplane"></param>
        private void AddLogger(Airplane airplane, Leg? leg = null) =>
            _context.Loggers.Add(new Logger
            {
                Airplane = airplane,
                In = DateTime.Now,
                Leg = (leg.IsNull()) ? _legs.First() : leg
            });
        #endregion

        #endregion

        #endregion
    }
}
