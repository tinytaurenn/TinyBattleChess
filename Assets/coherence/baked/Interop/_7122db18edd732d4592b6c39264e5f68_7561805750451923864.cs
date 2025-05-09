// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using Coherence.ProtocolDef;
    using Coherence.Serializer;
    using Coherence.SimulationFrame;
    using Coherence.Entities;
    using Coherence.Utils;
    using Coherence.Brook;
    using Coherence.Core;
    using Logger = Coherence.Log.Logger;
    using UnityEngine;
    using Coherence.Toolkit;

    public struct _7122db18edd732d4592b6c39264e5f68_7561805750451923864 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Byte m_IsHeld;
            [FieldOffset(1)]
            public System.Byte IsNPCHeld;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _7122db18edd732d4592b6c39264e5f68_7561805750451923864.m_IsHeldMask;
            m_IsHeldSimulationFrame = frame;
            FieldsMask |= _7122db18edd732d4592b6c39264e5f68_7561805750451923864.IsNPCHeldMask;
            IsNPCHeldSimulationFrame = frame;
        }

        public static unsafe _7122db18edd732d4592b6c39264e5f68_7561805750451923864 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 2) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 2) " +
                    "for component with ID 213");
            }

            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 213");
            }

            var orig = new _7122db18edd732d4592b6c39264e5f68_7561805750451923864();

            var comp = (Interop*)data;

            orig.m_IsHeld = comp->m_IsHeld != 0;
            orig.IsNPCHeld = comp->IsNPCHeld != 0;

            return orig;
        }


        public static uint m_IsHeldMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame m_IsHeldSimulationFrame;
        public System.Boolean m_IsHeld;
        public static uint IsNPCHeldMask => 0b00000000000000000000000000000010;
        public AbsoluteSimulationFrame IsNPCHeldSimulationFrame;
        public System.Boolean IsNPCHeld;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 213;
        public int PriorityLevel() => 100;
        public const int order = 0;
        public uint InitialFieldsMask() => 0b00000000000000000000000000000011;
        public bool HasFields() => true;
        public bool HasRefFields() => false;


        public long[] GetSimulationFrames() {
            return null;
        }

        public int GetFieldCount() => 2;


        
        public HashSet<Entity> GetEntityRefs()
        {
            return default;
        }

        public uint ReplaceReferences(Entity fromEntity, Entity toEntity)
        {
            return 0;
        }
        
        public IEntityMapper.Error MapToAbsolute(IEntityMapper mapper)
        {
            return IEntityMapper.Error.None;
        }

        public IEntityMapper.Error MapToRelative(IEntityMapper mapper)
        {
            return IEntityMapper.Error.None;
        }

        public ICoherenceComponentData Clone() => this;
        public int GetComponentOrder() => order;
        public bool IsSendOrdered() => false;


        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;


            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_7122db18edd732d4592b6c39264e5f68_7561805750451923864)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.m_IsHeldSimulationFrame = other.m_IsHeldSimulationFrame;
                this.m_IsHeld = other.m_IsHeld;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.IsNPCHeldSimulationFrame = other.IsNPCHeldSimulationFrame;
                this.IsNPCHeld = other.IsNPCHeld;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_7122db18edd732d4592b6c39264e5f68_7561805750451923864 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 2);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.m_IsHeld;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.IsNPCHeld;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _7122db18edd732d4592b6c39264e5f68_7561805750451923864 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(2);
            }

            var val = new _7122db18edd732d4592b6c39264e5f68_7561805750451923864();
            if (bitStream.ReadMask())
            {

                val.m_IsHeld = bitStream.ReadBool();
                val.FieldsMask |= _7122db18edd732d4592b6c39264e5f68_7561805750451923864.m_IsHeldMask;
            }
            if (bitStream.ReadMask())
            {

                val.IsNPCHeld = bitStream.ReadBool();
                val.FieldsMask |= _7122db18edd732d4592b6c39264e5f68_7561805750451923864.IsNPCHeldMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }


        public override string ToString()
        {
            return $"_7122db18edd732d4592b6c39264e5f68_7561805750451923864(" +
                $" m_IsHeld: { this.m_IsHeld }" +
                $" IsNPCHeld: { this.IsNPCHeld }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(2, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(2, '0') })";
        }
    }

}
