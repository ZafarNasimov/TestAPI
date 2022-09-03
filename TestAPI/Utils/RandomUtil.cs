using System;
using System.Text;

namespace TestAPI.Utils
{
    public class RandomUtil
    {
        public string RandomString(int size = 15)
        {
            Random _random = new Random();
            var builder = new StringBuilder(size);

            char offset = 'a';
            const int lettersOffset = 26;

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }
            return builder.ToString();
        }
    }
}
