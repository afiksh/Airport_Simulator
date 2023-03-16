#region using
using Models.Models;
#endregion

namespace Models.Logic
{
    /// <summary>
    /// Logic for move <see cref="Airplane"/> between legs
    /// </summary>
    public static class LegLogic
    {
        /// <summary>
        /// Decides what is the next leg of <paramref name="airplane"/> by <paramref name="currentLeg"/>
        /// </summary>
        /// <param name="airplane"></param>
        /// <param name="currentLeg"></param>
        /// <param name="legs"></param>
        /// <returns></returns>
        public static Leg? NextLegLogic(this Airplane airplane, int currentLeg, IEnumerable<Leg> legs)
        {
            int num;
            switch (currentLeg)
            {
                case 1:
                    {
                        num = 2;
                        break;
                    }
                case 2:
                    {
                        num = 3;
                        break;
                    }
                case 3:
                    {
                        var partialLegs = legs.SkipWhile(l => l.Number < 5).TakeWhile(l => l.Number < 9);
                        if (partialLegs.All(l => !l.IsEmpty)) return null;
                        num = 4;
                        break;
                    }
                case 4:
                    {
                        if (airplane.IsLanding == true) num = 5;
                        else num = 9;
                        break;
                    }
                case 5:
                    {
                        var leg6 = legs.First(l => l.Number == 6);
                        if (leg6.IsEmpty) return leg6;
                        var leg7 = legs.First(l => l.Number == 7);
                        if (leg7.IsEmpty) return leg7;
                        num = 5;
                        break;
                    }
                case 6:
                    {
                        num = 8;
                        break;
                    }
                case 7:
                    {
                        num = 8;
                        break;
                    }
                case 8:
                    {
                        num = 4;
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }
            return legs.FirstOrDefault(l => l.Number == num);
        }

        /// <summary>
        /// <paramref name="airplane"/> start at the first leg- just start the landing proccess
        /// </summary>
        /// <param name="airplane"></param>
        public static void FirstLegLogic(this Airplane airplane) => airplane.CurrentLeg.IsEmpty = false;
    }
}