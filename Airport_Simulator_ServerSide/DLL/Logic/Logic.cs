#region using
using DLL.Interfaces;
#endregion

namespace DLL.ExtensionMethods
{
    public static class Logic
    {
        #region Fields
        private static Mutex mutex = new Mutex();
        private static Random rnd = new Random();
        #endregion

        #region Extension Methods
        /// <summary>
        /// Check if an <paramref name="obj"/> is null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>if null than true, else false</returns>
        public static bool IsNull(this object? obj) => obj == null;

        /// <summary>
        /// Print <paramref name="airplane"/> details when created
        /// </summary>
        /// <param name="airplane"></param>
        /// <param name="legNumber"></param>
        public static void PrintLeg(this IAirplane airplane, int legNumber = 1)
        {
            mutex.WaitOne();
            Console.Write($"{DateTime.Now.ToString("hh:mm:ss")}: {airplane.SerialNumber}" +
                $" - number: {airplane.Number}" +
                $" - by {airplane.CompanyName.ToString()} " +
                $" - from {airplane.Origin.ToString()}" +
                $" - to {airplane.Destenation.ToString()}");
            Logic.ChangeForegroundColor(ConsoleColor.Green);
            Console.WriteLine($" Enter to leg number {legNumber}.\n");
            Logic.ChangeForegroundColor();
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Print <paramref name="ex"/> message
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="num"></param>
        /// <param name="airplane"></param>
        public static void PrintError(this Exception ex, int? num = null, IAirplane? airplane = null)
        {
            mutex.WaitOne();
            Logic.ChangeForegroundColor(ConsoleColor.Red);
            Console.WriteLine($"Error: {ex.Message} {num} {airplane?.AirplaneId} {airplane?.Number}");
            Logic.ChangeForegroundColor();
            mutex.ReleaseMutex();
        }
        #endregion

        #region Public Methods
        /// <param name="finalNum"></param>
        /// <returns>Random number between 0 to <paramref name="finalNum"/> included</returns>
        public static int RandomNumInt(int finalNum) => rnd.Next(0, finalNum + 1);

        /// <param name="firstNum"></param>
        /// <param name="finalNum"></param>
        /// <returns>Random number between <paramref name="firstNum"/> to <paramref name="finalNum"/> included</returns>
        public static int RandomNumInt(int firstNum, int finalNum) => rnd.Next(firstNum, finalNum + 1);
        #endregion

        #region Private Methods
        private static void ChangeForegroundColor(ConsoleColor color = ConsoleColor.White) => Console.ForegroundColor = color;
        #endregion
    }
}
