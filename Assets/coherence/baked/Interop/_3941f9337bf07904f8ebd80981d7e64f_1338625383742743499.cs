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

    public struct _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public ByteArray text;
            [FieldOffset(16)]
            public System.Byte enabled;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499.textMask;
            textSimulationFrame = frame;
            FieldsMask |= _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499.enabledMask;
            enabledSimulationFrame = frame;
        }

        public static unsafe _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 17) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 17) " +
                    "for component with ID 177");
            }

            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 177");
            }

            var orig = new _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499();

            var comp = (Interop*)data;

            orig.text = comp->text.Data != null ? System.Text.Encoding.UTF8.GetString((byte*)comp->text.Data, (int)comp->text.Length) : null;
            orig.enabled = comp->enabled != 0;

            return orig;
        }


        public static uint textMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame textSimulationFrame;
        public System.String text;
        public static uint enabledMask => 0b00000000000000000000000000000010;
        public AbsoluteSimulationFrame enabledSimulationFrame;
        public System.Boolean enabled;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 177;
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
            var other = (_3941f9337bf07904f8ebd80981d7e64f_1338625383742743499)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.textSimulationFrame = other.textSimulationFrame;
                this.text = other.text;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.enabledSimulationFrame = other.enabledSimulationFrame;
                this.enabled = other.enabled;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_3941f9337bf07904f8ebd80981d7e64f_1338625383742743499 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 2);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.text;



                bitStream.WriteShortString(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.enabled;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(2);
            }

            var val = new _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499();
            if (bitStream.ReadMask())
            {

                val.text = bitStream.ReadShortString();
                val.FieldsMask |= _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499.textMask;
            }
            if (bitStream.ReadMask())
            {

                val.enabled = bitStream.ReadBool();
                val.FieldsMask |= _3941f9337bf07904f8ebd80981d7e64f_1338625383742743499.enabledMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }


        public override string ToString()
        {
            return $"_3941f9337bf07904f8ebd80981d7e64f_1338625383742743499(" +
                $" text: { this.text }" +
                $" enabled: { this.enabled }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(2, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(2, '0') })";
        }
    }

}
