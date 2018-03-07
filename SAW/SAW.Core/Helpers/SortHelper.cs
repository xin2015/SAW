using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Core.Helpers
{
    public class SortHelper
    {
        /// <summary>
        /// 冒泡法
        /// </summary>
        /// <param name="list"></param>
        public static void BubbleSort(List<int> list)
        {
            int temp;
            for (int i = list.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 冒泡法（每次小循环结束会判断是否已经完成排序，完成则提前结束）
        /// </summary>
        /// <param name="list"></param>
        public static void BubbleSortWithCheck(List<int> list)
        {
            int temp;
            bool sort;
            for (int i = list.Count - 1; i > 0; i--)
            {
                sort = false;
                for (int j = 0; j < i; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                        sort = true;
                    }
                }
                if (!sort)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 选择排序
        /// </summary>
        /// <param name="list"></param>
        public static void SelectionSort(List<int> list)
        {
            int min, minIndex;
            for (int i = 0; i < list.Count; i++)
            {
                min = list[i];
                minIndex = i;
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[j] < min)
                    {
                        min = list[j];
                        minIndex = j;
                    }
                }
                list[minIndex] = list[i];
                list[i] = min;
            }
        }

        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="list"></param>
        public static void InsertionSort(List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                int current = list[i];
                int j = i - 1;
                for (; j >= 0; j--)
                {
                    if (list[j] > current)
                    {
                        list[j + 1] = list[j];
                    }
                    else
                    {
                        break;
                    }
                }
                list[j + 1] = current;
            }
        }
    }
}
