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
        value.x = (value.x + 1) * (Screen.width);
        value.y = (value.y + 1) * (Screen.height);        
        return value;
    }
}

