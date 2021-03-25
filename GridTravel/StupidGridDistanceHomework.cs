﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GridTravel
{
    public class StupidGridDistanceHomework
    {
        static void Main()
        {
            int width = Int32.Parse(Console.ReadLine());
            _ = Console.ReadLine(); // height
            string gridText = Console.ReadLine();
            var grid = CreateGrid(gridText);
            string input;
            while ((input = Console.ReadLine()) != String.Empty)
            {
                int shortestPathLength = FindShortestPath(grid, input, width);
                Console.WriteLine(shortestPathLength);
            }
        }

        private static int FindShortestPath(List<char> grid, string input, int width)
        {
            int len = 0;
            int posX = 0;
            int posY = 0;
            int newPos;
            foreach (char c in input)
            {
                // 4 X 3
                // a a a a
                // a a x a
                // a a a a

                //  pos = 6
                //  posX = pos % width
                //  posY = pos / width
                if ((newPos = grid.IndexOf(c)) != -1)
                {
                    int newPosX = newPos % width;
                    int newPosY = newPos / width;

                    len += DistanceBetweenPoints(posX, newPosX, posY, newPosY);
                    posX = newPosX;
                    posY = newPosY;
                }
            }

            return len;
        }

        private static int DistanceBetweenPoints(int posX, int newPosX, int posY, int newPosY)
        {
            return Math.Abs(posX - newPosX) + Math.Abs(posY - newPosY);
        }

        private static List<char> CreateGrid(string gridText)
        {
            return gridText.ToList<char>();
        }
    }
}
