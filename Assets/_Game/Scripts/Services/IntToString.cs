using System.Collections.Generic;

public static class IntToString
{
    private static Dictionary<int, string> intToStringDict = new();
    
    public static string Get(int i)
    {
        return intToStringDict[i];
    }
    
    static IntToString()
    {
        for (int i = 0; i < 100000; i++)
        {
            intToStringDict.Add(i, i.ToString());
        }
    }
}