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


    private Camera PlayerCamera(int deviceId)
    {
        return PlayerManagement.playerDataPerevice.Find(x => x.deviceId == deviceId).playerCamera;
    }

    public override Vector2 Process(Vector2 value, InputControl control)
    {
        Camera m_Camera = PlayerCamera(control.device.deviceId);
        value.x = (value.x + 1) * (m_Camera.pixelRect.min.x + m_Camera.pixelRect.width / 2);
        value.y = (value.y + 1) * (m_Camera.pixelRect.min.y + m_Camera.pixelRect.height / 2);
        return value;
    }
}
