using UnityEngine;

public static class DebugFro
{
    public static bool isLogVisable = false;
    public static bool isLogWarningVisable = true;
    
    public static void Log(string message)
    {
        if (isLogVisable)
        {
            Debug.Log(message);
        }

    }
    
    public static void Log(string className, string message)
    {
        if (isLogVisable)
        {
            Debug.Log($"{className} : {message}");
        }

    }

    public static void LogWarning(string message)
    {
        if (isLogWarningVisable)
        {
            Debug.LogWarning(message);         
        }
    }
    
    public static void LogWarning(string className, string message)
    {
        if (isLogWarningVisable)
        {
            Debug.Log($"{className} : {message}");        
        }
    }
}
