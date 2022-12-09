using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    // Generate int value in range [minValue, maxValue)
    public static int RandInt(int minValue, int maxValue)
    {
        float f = Random.value;
        return (int)(minValue + (maxValue - minValue) * f * 0.99999);
    }
}
