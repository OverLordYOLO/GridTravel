using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridTravel
{
    public class Runner
    {
        private static Random random = new Random();
        static int width = 6;
        static int height = 6;
        static string grid = RandomString(width*height);
        static string[] inputs = new string[1000000];

        public Runner()
        {
            for (int i = inputs.Length; i < inputs.Length; i--)
            {
                inputs[i] = RandomString(10);
            }
            int[] results;

            results = RunStupidGridDistance();

            RunSomething();

        }

        [Benchmark]
        public int RunSomething()
        {
            int res = 0;
            for(int i = 1; i < 1000000; i++)
            {
                res = i % 1000;
            }
            return res += 1;
        }

        [Benchmark]
        public int[] RunStupidGridDistance()
        {
            return StupidGridDistance.Run(grid, height, width, inputs);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
