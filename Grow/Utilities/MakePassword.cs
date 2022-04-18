using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Utilities
{
    public class MakePassword
    {
        public static string Generate(int NumberOfCharacters = 8)
        {
            //I have used code like this to generate random product numbers so
            //figured it would do here as well.
            var random = new Random();
            //By definition, these are the allowed characters as specified in Startup.cs
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            var charsCount = 63;//24 + 24 + 10 + 5
            var password = new char[NumberOfCharacters];
            for (int i = 0; i < NumberOfCharacters; i++)
            {
                password[i] = chars[random.Next(charsCount)];
            }
            return new string(password);
        }
    }
}
