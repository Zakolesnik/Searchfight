using System.Linq;

namespace Searchfight.Tests
{
    public static class Extensions
    {
        public static int IndexOfMaxElement(this ulong[][] values)
        {
            ulong max = 0;
            int idxOfMaxElement = 0;
            foreach (var rez in values)
            {
                if (rez.Max() > max)
                {
                    max = rez.Max();
                    idxOfMaxElement = rez.ToList().IndexOf(max);
                }
            }
            return idxOfMaxElement;
        }
    }
}