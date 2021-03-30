using System;
using System.Collections.Generic;
using System.Linq;

namespace GridTravel
{
    public class PullBackGridDistance
    {
        /*
        1) Count occurences of letters from the word in the grid
        2) Create array with char doubles - pairs[n-1]; where n = number of chars
        3) Create path map - dictionary<pair, distance>
            for each pair:
                a) check if pair exists - skip if yes
                b) create dict entry
        4) Pull-back search
        Remember only distance of previous and current layer's nodes
        At the end -> previous = current        P = previous; D = distance
            a) previous = last
                current = pairs[last-1]
            for each currentNode:
                a) save min(Pi+Di)
        */
        public static int[] Run(string inGrid, int inHeight, int inWidth, string[] inInputs)
        {
            var results = new int[inInputs.Length];
            int width = inWidth;
            _ = inHeight;
            string gridText = inGrid;
            var inputs = inInputs;
            var grid = gridText.ToArray();



            for (int i = 0; i < inputs.Length; i++)
            {
                var input = inputs[i];
                //   1) Count occurences of letters from the word in the grid
                var occurrences = CountOccurences(grid, input.ToCharArray(), out string filteredWord);

                //   2) Create array with char doubles -pairs[n - 1]; where n = number of chars
                var pairs = CreatePairs(filteredWord);
                //   3) Create path map - dictionary < pair, distance >
                var pathMap = CreatePathMap(width, filteredWord, occurrences, pairs);
                //   4) Pull - back search
                var shortestPath = FindShortestPath_PullBack(filteredWord, occurrences, pathMap);

                int numberOfStrokes = shortestPath + filteredWord.Length;

                results[i] = numberOfStrokes;
            }
            return results;
        }

        private static int FindShortestPath_PullBack(string word, Dictionary<char, List<int>> occurences, Dictionary<string, int> pathMap)
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
            var wordLength = word.Length - 1;

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
                            var distance = pathMap[CreateKeyForMap("xxx", word[0], 0, pos++)] + next;
                            return Math.Min(distance, shortest);
                        });

        }

        private static Dictionary<string, int> CreatePathMap(int width, string word, Dictionary<char, List<int>> occurrences, string[] pairs)
        {
            //3) Create path map - dictionary < pair, distance >
            //a) add distance of the first letter
            //for each pair:
            //    a) check if pair exists - skip if yes
            //    b) create dict entry
            var pathMap = new Dictionary<string, int>();

            var firstLetter = occurrences[word[0]];
            for (int i = 0; i < firstLetter.Count; i++)
                pathMap.Add(CreateKeyForMap("xxx", word[0], 0, i), CountDistance(width, 0, firstLetter[i]));

            foreach (var pair in pairs)
            {
                if (!pathMap.ContainsKey(pair))
                {
                    var firstPair = occurrences[pair[0]];
                    var secondPair = occurrences[pair[1]];
                    for (int i = 0; i < firstPair.Count; i++)
                    {
                        for (int j = 0; j < secondPair.Count; j++)
                        {
                            pathMap.Add(CreateKeyForMap(pair, i, j), CountDistance(width, firstPair[i], secondPair[j]));
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

        private static string CreateKeyForMap(string pair, int occurenceA, int occurenceB)
        {
            return $"{pair}-{occurenceA}-{occurenceB}";
        }

        private static int CountDistance(int width, int firstPos, int secondPos)
        {
            int firstPosX = firstPos % width;
            int firstPosY = firstPos / width;
            int secondPosX = secondPos % width;
            int secondPosY = secondPos / width;
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

        private static Dictionary<char, List<int>> CountOccurences(char[] grid, char[] word, out string filteredWord)
        {
            var occurences = new Dictionary<char, List<int>>();
            for (int i = 0; i < grid.Length; i++)
            {
                var currentLetter = grid[i];
                if (word.Contains(currentLetter))
                {
                    if (occurences.ContainsKey(currentLetter))
                    {
                        occurences[currentLetter].Add(i);
                    }
                    else
                    {
                        occurences.Add(currentLetter, new List<int> { i });
                    }
                }
            }
            filteredWord = word.Where(x => occurences.ContainsKey(x)).ToString();
            return occurences;
        }
    }
}
