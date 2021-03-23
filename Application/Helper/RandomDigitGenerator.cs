using System;

namespace Application.Helper
{
    public class RandomDigitGenerator
    {
           public static string SixDigitNumber()
        {
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");
            return sixDigitNumber;
        }
    }
}