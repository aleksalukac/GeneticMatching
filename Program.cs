using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetic01
{
    class Program
    {
        const int n = 10;
        const int numberOfGenerations = 40;
        const int unitsPerGeneration = 100;

        const int oldUnits = 30;
        const int newUnits = 40;
        const int mutatedUnits = 30;


        const int favorite = 3;
        const int leastFavorite = 3;

        int[] boysPositiveGrader = { 300, 200, 100 };
        int[] boysNegativeGrader = { -200, -100, -50 };

        int[] girlsPositiveGrader = { 500, 400, 300 };
        int[] girlsNegativeGrader = { -500, -400, -300 };

        int[,] boysFavorite = new int[n, favorite];
        int[,] boysLeastFavorite = new int[n, leastFavorite];
        int[,] girlsFavorite = new int[n, favorite];
        int[,] girlsLeastFavorite = new int[n, leastFavorite];

        int[] matches = new int[n];

        void randomGenerateFavorites(Random rnd)
        {
            //boys
            for (int i = 0; i < n; i++)
            {
                int[] fav = { -1, -1, -1 };
                for (int j = 0; j < favorite; j++)
                {
                    int k = rnd.Next(0, n);
                    while (fav.Contains(k))
                    {
                        k = rnd.Next(0, n);
                    }

                    boysFavorite[i, j] = k;
                    fav[j] = k;
                }
            }

            for (int i = 0; i < n; i++)
            {
                int[] fav = { -1, -1, -1 };
                for (int j = 0; j < favorite; j++)
                {
                    int k = rnd.Next(0, n);
                    while (fav.Contains(k) || boysFavorite[i, 0] == k || boysFavorite[i, 1] == k || boysFavorite[i, 2] == k)
                    {
                        k = rnd.Next(0, n);
                    }

                    boysLeastFavorite[i, j] = k;
                    fav[j] = k;
                }
            }

            //girls

            for (int i = 0; i < n; i++)
            {
                int[] fav = { -1, -1, -1 };
                for (int j = 0; j < favorite; j++)
                {
                    int k = rnd.Next(0, n);
                    while (fav.Contains(k))
                    {
                        k = rnd.Next(0, n);
                    }

                    girlsFavorite[i, j] = k;
                    fav[j] = k;
                }
            }

            for (int i = 0; i < n; i++)
            {
                int[] fav = { -1, -1, -1 };
                for (int j = 0; j < favorite; j++)
                {
                    int k = rnd.Next(0, n);
                    while (fav.Contains(k) || girlsFavorite[i, 0] == k || girlsFavorite[i, 1] == k || girlsFavorite[i, 2] == k)
                    {
                        k = rnd.Next(0, n);
                    }

                    girlsLeastFavorite[i, j] = k;
                    fav[j] = k;
                }
            }
        }

        int[] generateUnit(Random rnd)
        {
            int[] randomArray = Enumerable.Repeat(-1, n).ToArray();

            for (int i = 0; i < n; i++)
            {
                int k = rnd.Next(0, n);
                while (randomArray.Contains(k))
                    k = rnd.Next(0, n);
                randomArray[i] = k;
            }
            return randomArray;
        }

        int gradeUnit(int[] unit)
        {
            int sum = 0;
            for (int i = 0; i < n; i++)
            {
                int lady = unit[i];
                bool stop = false;

                for (int j = 0; j < favorite; j++)
                {
                    if (boysFavorite[i, j] == lady)
                    {
                        sum += boysPositiveGrader[j];
                        stop = true;
                        break;
                    }
                }

                for (int j = 0; j < favorite && !stop; j++)
                {
                    if (boysLeastFavorite[i, j] == lady)
                    {
                        sum += boysNegativeGrader[j];
                        break;
                    }
                }

                stop = false;

                for (int j = 0; j < favorite; j++)
                {
                    if (girlsFavorite[lady, j] == i)
                    {
                        sum += girlsPositiveGrader[j];
                        stop = true;
                        break;
                    }
                }

                for (int j = 0; j < favorite && !stop; j++)
                {
                    if (girlsLeastFavorite[lady, j] == i)
                    {
                        sum += girlsNegativeGrader[j];
                        break;
                    }
                }
            }
            return sum;
        }

        int[] geneticMutation(int[] unit, Random rnd)
        {
            int numberOfMutations = rnd.Next(1, 6);

            var newUnit = (int[])unit.Clone();

            for (int i = 0; i < numberOfMutations; i++)
            {
                int k = rnd.Next(0, 10);
                int j = rnd.Next(0, 10);
                while (j == k)
                    j = rnd.Next(0, 10);

                int swap = newUnit[k];
                newUnit[k] = newUnit[j];
                newUnit[j] = swap;
            }

            return newUnit;
        }

        void sortList(List<int[]> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (gradeUnit(list[j]) > gradeUnit(list[i]))
                    {
                        var s = list[i];
                        list[i] = list[j];
                        list[j] = s;
                    }
                }
            }
        }

        void Genetic()
        {
            Random rnd = new Random();
            //generate favorite girls for each boy and vice versa
            randomGenerateFavorites(rnd);

            List<int[]> generation = new List<int[]>();

            //testing
            /*int[] grade1 = new int[100];
            int[] grade2 = new int[100];*/

            for (int i = 0; i < unitsPerGeneration; i++)
            {
                var a = generateUnit(rnd);
                generation.Add(a);
                //grade1[i] = gradeUnit(a);
            }


            for (int genSeries = 0; genSeries < numberOfGenerations; genSeries++)
            {
                sortList(generation);
                Console.WriteLine("The best from generation " + genSeries.ToString() + " has a score of: " + gradeUnit(generation[0]).ToString());

                /*for (int i = 0; i < unitsPerGeneration; i++)
                {
                    grade2[i] = gradeUnit(generation[i]);
                }*/

                for (int i = oldUnits; i < oldUnits + mutatedUnits; i++)
                {
                    generation[i] = geneticMutation(generation[i - oldUnits], rnd);
                }

                for (int i = oldUnits + mutatedUnits; i < oldUnits + mutatedUnits + newUnits; i++)
                {
                    generation[i] = generateUnit(rnd);
                }
            }
        }

        static void Main(string[] args)
        {
            //Finding the perfect match for a group of girls and boys. Every boy has P favorite girls and Q least favorite girls and vice versa.
            Program program = new Program();
            program.Genetic();
        }
    }
}
