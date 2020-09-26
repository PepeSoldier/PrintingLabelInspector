using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class KnapsackAlgorithm
    {
        public KnapsackAlgorithm()
        {
            SelectedItems = new List<int>();
        }
        
        //---------------------------------------------DYNAMIC-PROGRAMMING
        int numerOfItems; //number of items
        int maxWeight; //knapsack 
        public int[,] m { get; set; }
        public List<int> values; //values
        public List<int> weights; //weights
        
        public void Start_Method_DynamicProgramming(List<int> values, List<int> weights, int maxCapacity)
        {
            this.values = values;
            this.weights = weights;
            this.maxWeight = maxCapacity;

            numerOfItems = values.Count;
            m = new int[numerOfItems, maxWeight + 1];

            for (int weight1 = 0; weight1 < maxWeight; weight1++) {
                m[0, weight1] = 0;
            }

            for (int item = 1; item < numerOfItems; item++)
            {
                for (int c_max_weight = 1; c_max_weight <= maxWeight; c_max_weight++)
                {
                    if (weights[item] > c_max_weight)
                    {
                        m[item, c_max_weight] = m[item - 1, c_max_weight];
                    }
                    else
                    {
                        m[item, c_max_weight] = Math.Max(
                                            m[item - 1, c_max_weight], 
                                            m[item - 1, c_max_weight - weights[item]] + values[item]);
                    }
                }
            }
        }

        //---------------------------------------------DYNAMIC-PROGRAMMING
        public int MaxWeightCapacity { get; set; }
        public int SelectedValuesSum { get { return SelectedItems.Sum(); } }
        public int Unused { get { return MaxWeightCapacity - SelectedValuesSum; } }
        public int Loops { get; set; }
        public List<int> Items { get; set; }
        public List<int> SelectedItems { get; set; }

        private bool end = false;
        private int bestResult = 0;
        private int tempResult = 0;
        private List<int> tempSelectedItems;

        public void Start_Method_1(List<int> values, int maxCapacity, int scrapPerPiece)
        {
            Items = values.OrderByDescending(x => x).ToList();
            MaxWeightCapacity = maxCapacity;

            SelectedItems.Clear();
            tempSelectedItems = new List<int>();
            end = false;
            bestResult = 0;
            tempResult = 0;
            Loops = 0;

            if (!(Items.Count > 0)){ return; }
            else
            {
                tempSelectedItems.Add(Items[0]); //najdluzszy dodawany jest zawsze na początek
                tempResult = Items[0];
                SelectedItems = tempSelectedItems;
            }

            int j = 1;
            int k = 0;
            while (!end && j < Items.Count)
            {
                k = j + 1;
                while (!end && k < Items.Count)
                {
                    tempSelectedItems = new List<int>();
                    tempSelectedItems.Add(Items[0]);
                    tempSelectedItems.Add(Items[j]);
                    tempResult = tempSelectedItems.Sum();
                    tempResult += (tempSelectedItems.Count - 1) * scrapPerPiece;

                    if (tempResult > maxCapacity)
                    {
                        //fast skip to values suitable to remian capacity
                        int remainCapacity = maxCapacity - (tempResult - Items[j]);
                        j = SkipToValueLessThan(j, remainCapacity);
                        break;
                    }

                    tempResult += SumToEndOfArray(k, maxCapacity - tempResult, tempSelectedItems, scrapPerPiece);
                    SaveBestResultAndCheckIfEnd(maxCapacity);

                    //skip to next value suitable for pairing
                    if (!end)
                    {
                        //k = SkipToValueLessThan(k, tempSelectedItems[tempSelectedItems.Count - 1]) - 1;
                        int k_temp = SkipToValueLessThan(k, maxCapacity - ( tempSelectedItems[0] + tempSelectedItems[1] + Items.Min()) ) - 1;
                        k = k_temp > k ? k_temp : k;
                    }

                    k++;
                    Loops++;
                }
                j = SkipToValueLessThan(j, Items[j]);
            }
        }

        private int SkipToValueLessThan(int j, int value)
        {
            j = Items.FindIndex(x => x < value);
            if (j <= 0)
            {
                j = Items.Count;
            }

            return j;
        }

        private void SaveBestResultAndCheckIfEnd(int maxCapacity)
        {
            if (tempResult > bestResult)
            {
                bestResult = tempResult;
                SelectedItems = tempSelectedItems;
                end = (bestResult >= maxCapacity - 20);
            }
        }

        private int SumToEndOfArray(int i, int limit, List<int> selectedItems, int scrapPerPiece)
        {
            int sum = 0;
            while (i < Items.Count)
            {
                if (sum + Items[i] <= limit)
                {
                    selectedItems.Add(Items[i]);
                    sum += Items[i];
                    sum += scrapPerPiece;
                }
                else
                {
                    i = SkipToValueLessThan(i, limit-sum);
                }
                i++;
            }
            return sum;
        }
    }
    
    public class KnapsackItem
    {
        public int Weight { get; set; }
        public decimal Value { get; set; }
    }

}