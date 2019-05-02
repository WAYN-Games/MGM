using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    float minFps = 999f;
    float maxFps = -1f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (Time.renderedFrameCount < 200) return;
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        if (fps < minFps) minFps = fps;
        if (fps > maxFps) maxFps = fps;


        string text = string.Format("{0:0.0} ms | {1:0.} fps < {2:0.} fps < {3:0.} fps ", msec , minFps ,fps, maxFps);
        GUI.Label(rect, text, style);
    }
}