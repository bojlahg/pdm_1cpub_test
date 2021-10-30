using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExt
{
    public static void Shuffle(this System.Array arr)
    {
        // перемешиваем массив
        if(arr.Length < 2)
        {
            return;
        }
        int shuffleCount = arr.Length * 2, doneCount = 0;
        while(doneCount < shuffleCount)
        {
            int idx1 = Random.Range(0, arr.Length), idx2 = Random.Range(0, arr.Length);
            if(idx1 != idx2)
            {
                object tmp = arr.GetValue(idx1);
                arr.SetValue(arr.GetValue(idx2), idx1);
                arr.SetValue(tmp, idx2);
                ++doneCount;
            }
        }
    }
}
