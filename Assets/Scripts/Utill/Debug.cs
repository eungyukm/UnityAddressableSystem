using UnityEngine;

public static class Debug
{
    public static bool isLogVisable = false;
    public static bool isLogWarningVisable = true;
    
    public static void Log(string message)
    {
        if (isLogVisable)
        {
            UnityEngine.Debug.Log(message);
        }

    }
    
    public static void Log(string name , string message)
    {
        if (isLogVisable)
        {
            UnityEngine.Debug.Log($"{name} : {message}");
        }

    }

    public static void LogWarning(string message)
    {
        if (isLogWarningVisable)
        {
            UnityEngine.Debug.LogWarning(message);         
        }
    }
    
    public static void LogWarning(string name , string message)
    {
        if (isLogWarningVisable)
        {
            UnityEngine.Debug.Log($"{name} : {message}");        
        }
    }
}
