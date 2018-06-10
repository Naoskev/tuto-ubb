using UnityEngine;

public static class Logger{

    public static void LogError(string message){
        Debug.LogError(message);
    }

    public static void LogInfo(string message){
        Debug.Log(message);
    }
}