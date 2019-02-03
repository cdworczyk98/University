using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Windows;

namespace CaveAI
{
    class Location
    {
        public int CaveNum { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public Location Parent { get; set; }
    }

    class CaveFile
    {
        public int CaveLength { get; set; }
        public string[] CaveInfo { get; set; }
        public string[] Coords { get; set; }
        public int[,] Directions { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CaveFile caveFile = ReadFileIn("generated100-1");

            Location current = null;
            var start = new Location { X = Convert.ToInt32(caveFile.Coords[0]), Y = Convert.ToInt32(caveFile.Coords[1]) , CaveNum = 1, G = 0};
            var target = new Location { X = Convert.ToInt32(caveFile.Coords[(caveFile.CaveLength*2) - 2]), Y = Convert.ToInt32(caveFile.Coords[(caveFile.CaveLength * 2) - 1]) , CaveNum = caveFile.CaveLength };
            var openList = new List<Location>();
            var closedList = new List<Location>();
            int g = 0;
            bool pathFound = false;
            string filename = "";

            filename = "generated100-1";
            Console.WriteLine("Number of caves: {0}", caveFile.CaveLength);
            Console.WriteLine("Starting cave {0} is at [{1},{2}]",start.CaveNum, start.X, start.Y);
            Console.WriteLine("End cave {0} is at [{1},{2}]",target.CaveNum, target.X, target.Y);

            // start by adding the original position to the open list
            start.F = ComputeHScore(start.X, start.Y, target.X, target.Y);
            openList.Add(start);

            while (openList.Count > 0)
            {
                // get the square with the lowest F score
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                // add the current square to the closed list
                closedList.Add(current);

                // remove it from the open list
                openList.Remove(current);

                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                {
                    pathFound = true;
                    PathFound(current, filename);
                    break;
                }

                var adjacentSquares = GetPossibleCaveDirections(current, caveFile.CaveLength, caveFile.Directions, caveFile.Coords);
                g++;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y) == null)
                    {
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;

                        if (openList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y) == null)
                        {
                            openList.Insert(0, adjacentSquare);
                        }
                        else
                        {
                            var openneighbor = openList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y);
                            if (adjacentSquare.G < openneighbor.G)
                            {
                                openneighbor.G = adjacentSquare.G;
                                openneighbor.Parent = adjacentSquare.Parent;
                            }
                        }
                    }

                    //// if this adjacent square is already in the closed list, ignore it
                    //if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                    //        && l.Y == adjacentSquare.Y) != null)
                    //    continue;

                    //// if it's not in the open list...
                    //if (openList.FirstOrDefault(l => l.X == adjacentSquare.X 
                    //    && l.Y == adjacentSquare.Y) == null)
                    //{
                    //    // compute its score, set the parent
                    //    adjacentSquare.G = g;
                    //    adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
                    //    adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                    //    adjacentSquare.Parent = current;

                    //    // and add it to the open list
                    //    openList.Insert(0, adjacentSquare);
                    //}
                    //else
                    //{
                    //    // test if using the current G score makes the adjacent square's F score
                    //    // lower, if yes update the parent because it means it's a better path
                    //    if (g + adjacentSquare.H < adjacentSquare.F)
                    //    {
                    //        adjacentSquare.G = g;
                    //        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                    //        adjacentSquare.Parent = current;
                    //    }
                    //}
                }
            }

            if(!pathFound)
            {
                Console.WriteLine("\nNo path found.\n");
                SaveFile(new List<int> { 0 }, filename);
            }
        }

        static void PathFound(Location current, string filename)
        {
            Console.WriteLine("\n---Cave Path---\n");

            int counter = 1;
            List<int> path = new List<int>();
            int cost = 0;

            while (current != null)
            {
                path.Add(current.CaveNum);
                cost += current.H;
                current = current.Parent;
            }

            for (int i = path.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("{0} - Cave {1}", counter, path[i]);
                counter++;
            }

            SaveFile(path, filename);
            Console.WriteLine("\n Cost: " + cost);
            Console.ReadKey();
        }

        static List<Location> GetPossibleCaveDirections(Location current, int length, int[,] directions, string[] coords)
        {
            var proposedLocations = new List<Location>() { };

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write(directions[i, j] + ",");
                }

                Console.Write("\n");
            }

            for (int i = 0; i < length; i++)
            {
                if (directions[i, current.CaveNum - 1] == 1)
                {        
                    proposedLocations.Add(new Location { X = Convert.ToInt32(coords[i*2]), Y = Convert.ToInt32(coords[(i*2)+1]), CaveNum = (i+1), G = current.G + 1, Parent = current });
                }
            }

            return proposedLocations;
        }

        static void SaveFile(List<int> output, string filename)
        {
            TextWriter tw = new StreamWriter(Directory.GetCurrentDirectory()+@"\"+filename+".csn");

            for (int i = output.Count - 1;  i >= 0; i--)
            {
                tw.Write(output[i].ToString() + " ");
            }

            tw.Close();
        }

        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }

        static CaveFile ReadFileIn(string filename)
        {
            CaveFile CaveFile = new CaveFile();

            string text = File.ReadAllText(GetFilePath(filename));

            CaveFile.CaveInfo = text.Split(new char[] { ',', ' ' });

            CaveFile.CaveLength = Convert.ToInt32(CaveFile.CaveInfo[0]);

            CaveFile.Coords = new string[CaveFile.CaveLength * 2];
            CaveFile.Directions = new int[CaveFile.CaveLength, CaveFile.CaveLength];

            int offset = 0;

            for (int i = 0; i < CaveFile.CaveLength; i++)
            {
                CaveFile.Coords[i + offset] = CaveFile.CaveInfo[i+1+offset];
                CaveFile.Coords[i+1 + offset] = CaveFile.CaveInfo[i + 2+offset];

                offset++;
            }

            for (int i = 0; i < CaveFile.CaveLength; i++)
            {
                for (int j = 0; j < CaveFile.CaveLength; j++)
                {
                    CaveFile.Directions[i, j] = Convert.ToInt16(CaveFile.CaveInfo[(j + (CaveFile.CaveLength * 2)+1) + (i * CaveFile.CaveLength)]);
                }
            }

            return CaveFile;
        }

        static string GetFilePath(string filename)
        {
            string path = Directory.GetCurrentDirectory();

            return path + @"\" + filename + ".cav" ;
        }
    }
}
