using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class ToPointerPosition : InputProcessor<Vector2>
{

#if UNITY_EDITOR
    static ToPointerPosition()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<ToPointerPosition>();
    }


    Camera m_Camera;

    public override Vector2 Process(Vector2 value, InputControl control)
    {
        if (m_Camera == null) m_Camera = Camera.main;
        value.x = (value.x + 1) * m_Camera.scaledPixelWidth / 2;
        value.y = (value.y + 1) * m_Camera.scaledPixelHeight / 2;
        return value;
    }
}