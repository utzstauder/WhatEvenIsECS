﻿// https://forum.unity.com/threads/as-debug-log-is-a-problem-in-job-i-made-this-simple-trick.860464/
using Unity.Burst;
using UnityEngine;

public static class JobLogger
{
    [BurstDiscard] public static void Log(params object[] parts) => Debug.Log(AppendToString(parts));
    [BurstDiscard] public static void LogWarning(params object[] parts) => Debug.LogWarning(AppendToString(parts));
    [BurstDiscard] public static void LogErrot(params object[] parts) => Debug.LogError(AppendToString(parts));
    public static string AppendToString(params object[] parts)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0, len = parts.Length; i < len; i++) sb.Append(parts[i].ToString());
        return sb.ToString();
    }
}