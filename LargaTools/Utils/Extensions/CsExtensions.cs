using System.Collections.Generic;

public static class CsExtensions
{
    public static void Deconstruct<TK, TV>(this KeyValuePair<TK, TV> keyValuePair, out TK key, out TV value)
    {
        key = keyValuePair.Key;
        value = keyValuePair.Value;
    }
}