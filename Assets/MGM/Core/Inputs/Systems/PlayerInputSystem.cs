using Unity.Entities;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InputSystemGroup))]
public abstract class PlayerInputSystem : JobComponentSystem
{
    protected InputAction m_Action;

    public void SetInputAction(InputAction action)
    {   
        m_Action = action;
        m_Action.performed += ActionPerformed;
    }

    internal abstract void ActionPerformed(InputAction.CallbackContext ctx);


    protected override void OnStartRunning()
    {
        m_Action.Enable();
        base.OnStartRunning();
    }

    protected override void OnStopRunning()
    {
        m_Action.Disable();
        base.OnStopRunning();

    }

    protected override void OnDestroy()
    {
        m_Action.Disable();
        base.OnDestroy();
    }

}