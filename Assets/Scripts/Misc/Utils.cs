using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void RunAfterDelay(MonoBehaviour monoBehaviour, Action task, float delay)
    {
        monoBehaviour.StartCoroutine(RunAfterDelayRoutine(task, delay));
    }

    public static Vector2 GetMousePositionInWorld()
    {
        Vector2 mousePositionInScreen = Input.mousePosition;
        Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
        return mousePositionInWorld;
    }

    private static IEnumerator RunAfterDelayRoutine(Action task, float delay)
    {
        yield return new WaitForSeconds(delay);
        task.Invoke();
    }
    
}
