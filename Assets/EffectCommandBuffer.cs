using System;
using Unity.Collections;

namespace Wayn.Mgm.Effects
{/*
    public struct EffectCommandMap : IDisposable
    {

        private NativeMultiHashMap<ulong, EffectCommand> EffectCommands;
        
        public void Dispose()
        {
            if (!EffectCommands.IsCreated) return;

            EffectCommands.Dispose();
        }

        public void Clear()
        {
            EffectCommands.Clear();
        }


        public NativeMultiHashMap<ulong, EffectCommand>.ParallelWriter GetWriter(int size)
        {
            if (EffectCommands.IsCreated) EffectCommands.Dispose();
            EffectCommands = new NativeMultiHashMap<ulong, EffectCommand>(size, Allocator.TempJob);
            return EffectCommands.AsParallelWriter();
        }

        public NativeMultiHashMap<ulong, EffectCommand>.Enumerator GetCommandToExecute(ulong EffectTypeId)
        {
            return EffectCommands.GetValuesForKey(EffectTypeId);
        }

    }
*/}
