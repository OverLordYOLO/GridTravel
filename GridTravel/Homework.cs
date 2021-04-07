using System;
using System.Collections.Generic;
using System.Linq;

namespace GridTravel
{

    public class Homework
    {
        public static void Main()
        {
            int width = Int32.Parse(Console.ReadLine());
            _ = Console.ReadLine(); // height
            string gridText = Console.ReadLine();
            var grid = gridText.ToList();
            var input = Console.ReadLine().ToList();

            int numberOfStrokes = 0;
            var commonLetters = grid.Intersect(input).ToList();
            if (commonLetters.Count > 0)
            {
                input.RemoveAll(x => !commonLetters.Contains(x));
                var occurrences = CountOccurences(grid, commonLetters);
                var pathMap = CreatePathMap(width, input, occurrences);
                var shortestPath = FindShortestPath_PullBack(input, occurrences, pathMap);
                numberOfStrokes = shortestPath + input.Count();
            }
            Console.WriteLine(numberOfStrokes);
        }

        private static int FindShortestPath_PullBack(List<char> word, Dictionary<char, List<int>> occurences, Dictionary<string, int> pathMap)
        {
            /*
            4) Pull - back search
            Remember only distance of previous and current layer's nodes
            At the end->previous = current        P = previous; D = distance
                a) previous = last
                current = pairs[last - 1]
                for each currentNode:
                    a) save min(Pi+Di)
            */
            var wordLength = word.Count() - 1;

            var distances = new List<int>();
            for (int i = 0; i < occurences[word[wordLength]].Count; i++)
                distances.Add(0);

            List<int> currentLayer;
            int pos = 0;
            for (int i = wordLength - 1; i >= 0; i--)
            {
                currentLayer = occurences[word[i]];
                for (int j = currentLayer.Count - 1; j >= 0; j--)
                {
                    currentLayer[j] = distances.Aggregate(Int32.MaxValue,
                        (shortest, next) =>
                        {
                            var distance = pathMap[CreateKeyForMap(word[i], word[i + 1], j, pos++)] + next;
                            return Math.Min(distance, shortest);
                        });
                    pos = 0;
                }
                distances = currentLayer; // check if it won't get erased in the next cycle
            }
            return distances.Aggregate(Int32.MaxValue,
                        (shortest, next) =>
                        {
                            var distance = pathMap[CreateKeyForMap("xx", word[0], 0, pos++)] + next;
                            return Math.Min(distance, shortest);
                        });

        }

        private static Dictionary<string, int> CreatePathMap(int width, List<char> word, Dictionary<char, List<int>> occurrences)
        {
            //3) Create path map - dictionary < pair, distance >
            //a) add distance of the first letter
            //for each pair:
            //    a) check if pair exists - skip if yes
            //    b) create dict entry
            var pathMap = new Dictionary<string, int>();

            var firstLetter = occurrences[word[0]];
            for (int i = 0; i < firstLetter.Count; i++)
                pathMap.Add(CreateKeyForMap("xx", word[0], 0, i), CountDistance(width, 0, firstLetter[i]));

            for (int i = 0; i < word.Count()-1; i++)
            {
                if (!pathMap.ContainsKey(CreateKeyForMap(word[i], word[i+1], 0, 0)))
                {
                    var firstLetterOccurence = occurrences[word[i]];
                    var secondLetterOccurence = occurrences[word[i+1]];
                    for (int j = 0; j < firstLetterOccurence.Count; j++)
                    {
                        for (int k = 0; k < secondLetterOccurence.Count; k++)
                        {
                            pathMap.Add(CreateKeyForMap(word[i], word[i+1], j, k), CountDistance(width, firstLetterOccurence[j], secondLetterOccurence[k]));
                        }
                    }
                }
            }

            return pathMap;
        }
        private static string CreateKeyForMap(char firstLetter, char secondLetter, int occurenceA, int occurenceB)
        {
            return $"{firstLetter}{secondLetter}-{occurenceA}-{occurenceB}";
        }
        private static string CreateKeyForMap(string firstLetter, char secondLetter, int occurenceA, int occurenceB)
        {
            return $"{firstLetter}{secondLetter}-{occurenceA}-{occurenceB}";
        }

        private static int CountDistance(int width, int firstPos, int secondPos)
        {
            int firstPosY = firstPos / width;
            int firstPosX = firstPos - (firstPosY * width);
            int secondPosY = secondPos / width;
            int secondPosX = secondPos - (secondPosY * width);
            return Math.Abs(firstPosX - secondPosX) + Math.Abs(firstPosY - secondPosY);
        }

        private static string[] CreatePairs(string word)
        {
            var pairs = new string[word.Length - 1];
            for (int i = 0; i < word.Length - 1; i++)
            {
                pairs[i] = word.Substring(i, 2);
            }
            return pairs;
        }

        private static Dictionary<char, List<int>> CountOccurences(List<char> grid, List<char> commonLetters)
        {
            var occurences = new Dictionary<char, List<int>>();
            commonLetters.ForEach(x => occurences.Add(x, new List<int>()));

            for (int i = 0; i < grid.Count(); i++)
            {
                occurences[grid[i]].Add(i);
            }
            return occurences;
        }
    }
}