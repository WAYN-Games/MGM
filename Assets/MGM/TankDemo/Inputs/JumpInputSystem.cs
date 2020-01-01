using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpInputSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private bool m_jumpTrigger;
    private bool m_lastFrameJumpTrigger;

    private MouvementControls m_MouvementControls;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<MovementDirection>(),
                ComponentType.ReadOnly<PlayerControled>()
            }
        };
        m_Query = GetEntityQuery(query);
        m_MouvementControls = new MouvementControls();
        m_MouvementControls.Mouvement.Jump.performed += UpdateJumpTrigger;

    }


    protected override void OnStartRunning()
    {
        m_MouvementControls.Mouvement.Jump.Enable();
        base.OnStartRunning();
    }

    protected override void OnStopRunning()
    {
        m_MouvementControls.Mouvement.Jump.Disable();
        base.OnStopRunning();

    }

    protected override void OnDestroy()
    {
        m_MouvementControls.Mouvement.Jump.Disable();
        base.OnDestroy();
    }



    [BurstCompile]
    struct MovementInputSystemJob : IJobChunk
    {
        public bool JumpInputTrigger;
        public ArchetypeChunkComponentType<JumpTrigger> JumpTrigger;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkJumpTriggers = chunk.GetNativeArray(JumpTrigger);

            for (var i = 0; i < chunk.Count; i++)
            {
                chunkJumpTriggers[i] = new JumpTrigger
                {
                    Value = JumpInputTrigger
                };

            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        // Check the input changed.
        if (m_lastFrameJumpTrigger.Equals(m_jumpTrigger)) return inputDependencies;

        // Cache the input for net frame comaprison.
        m_lastFrameJumpTrigger = m_jumpTrigger;

        // Input changed so we schedule the job.
        var job = new MovementInputSystemJob()
        {
            JumpInputTrigger = m_jumpTrigger,
            JumpTrigger = GetArchetypeChunkComponentType<JumpTrigger>(false)
        };
        return  job.Schedule(m_Query,inputDependencies);
    }
    
    private void UpdateJumpTrigger(InputAction.CallbackContext ctx)
    {
        m_jumpTrigger = ctx.ReadValue<float>()>0;
    }
}