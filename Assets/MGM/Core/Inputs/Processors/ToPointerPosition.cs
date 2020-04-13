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
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        value.x = (value.x + 1) * (Screen.width / 2);
        value.y = (value.y + 1) * (Screen.height / 2);
        return value;
    }
}

