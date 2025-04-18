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

    public struct _61db515efe9556c45853cdc384e6813b_26648702269276731 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Byte ShieldParry;
            [FieldOffset(1)]
            public System.Byte Parry;
            [FieldOffset(2)]
            public System.Byte Attacking;
            [FieldOffset(3)]
            public System.Int32 WeaponDirectionNESO;
            [FieldOffset(7)]
            public System.Byte Grounded;
            [FieldOffset(8)]
            public System.Single MoveSpeed;
            [FieldOffset(12)]
            public System.Byte Stunned;
            [FieldOffset(13)]
            public System.Byte Dead;
            [FieldOffset(14)]
            public System.Byte TwoHanded;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.ShieldParryMask;
            ShieldParrySimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.ParryMask;
            ParrySimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.AttackingMask;
            AttackingSimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.WeaponDirectionNESOMask;
            WeaponDirectionNESOSimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.GroundedMask;
            GroundedSimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.MoveSpeedMask;
            MoveSpeedSimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.StunnedMask;
            StunnedSimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.DeadMask;
            DeadSimulationFrame = frame;
            FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.TwoHandedMask;
            TwoHandedSimulationFrame = frame;
        }

        public static unsafe _61db515efe9556c45853cdc384e6813b_26648702269276731 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 15) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 15) " +
                    "for component with ID 206");
            }

            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 206");
            }

            var orig = new _61db515efe9556c45853cdc384e6813b_26648702269276731();

            var comp = (Interop*)data;

            orig.ShieldParry = comp->ShieldParry != 0;
            orig.Parry = comp->Parry != 0;
            orig.Attacking = comp->Attacking != 0;
            orig.WeaponDirectionNESO = comp->WeaponDirectionNESO;
            orig.Grounded = comp->Grounded != 0;
            orig.MoveSpeed = comp->MoveSpeed;
            orig.Stunned = comp->Stunned != 0;
            orig.Dead = comp->Dead != 0;
            orig.TwoHanded = comp->TwoHanded != 0;

            return orig;
        }


        public static uint ShieldParryMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame ShieldParrySimulationFrame;
        public System.Boolean ShieldParry;
        public static uint ParryMask => 0b00000000000000000000000000000010;
        public AbsoluteSimulationFrame ParrySimulationFrame;
        public System.Boolean Parry;
        public static uint AttackingMask => 0b00000000000000000000000000000100;
        public AbsoluteSimulationFrame AttackingSimulationFrame;
        public System.Boolean Attacking;
        public static uint WeaponDirectionNESOMask => 0b00000000000000000000000000001000;
        public AbsoluteSimulationFrame WeaponDirectionNESOSimulationFrame;
        public System.Int32 WeaponDirectionNESO;
        public static uint GroundedMask => 0b00000000000000000000000000010000;
        public AbsoluteSimulationFrame GroundedSimulationFrame;
        public System.Boolean Grounded;
        public static uint MoveSpeedMask => 0b00000000000000000000000000100000;
        public AbsoluteSimulationFrame MoveSpeedSimulationFrame;
        public System.Single MoveSpeed;
        public static uint StunnedMask => 0b00000000000000000000000001000000;
        public AbsoluteSimulationFrame StunnedSimulationFrame;
        public System.Boolean Stunned;
        public static uint DeadMask => 0b00000000000000000000000010000000;
        public AbsoluteSimulationFrame DeadSimulationFrame;
        public System.Boolean Dead;
        public static uint TwoHandedMask => 0b00000000000000000000000100000000;
        public AbsoluteSimulationFrame TwoHandedSimulationFrame;
        public System.Boolean TwoHanded;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 206;
        public int PriorityLevel() => 100;
        public const int order = 0;
        public uint InitialFieldsMask() => 0b00000000000000000000000111111111;
        public bool HasFields() => true;
        public bool HasRefFields() => false;


        public long[] GetSimulationFrames() {
            return null;
        }

        public int GetFieldCount() => 9;


        
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

        private static readonly System.Int32 _WeaponDirectionNESO_Min = -1;
        private static readonly System.Int32 _WeaponDirectionNESO_Max = 5;

        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;


            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_61db515efe9556c45853cdc384e6813b_26648702269276731)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.ShieldParrySimulationFrame = other.ShieldParrySimulationFrame;
                this.ShieldParry = other.ShieldParry;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.ParrySimulationFrame = other.ParrySimulationFrame;
                this.Parry = other.Parry;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.AttackingSimulationFrame = other.AttackingSimulationFrame;
                this.Attacking = other.Attacking;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.WeaponDirectionNESOSimulationFrame = other.WeaponDirectionNESOSimulationFrame;
                this.WeaponDirectionNESO = other.WeaponDirectionNESO;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.GroundedSimulationFrame = other.GroundedSimulationFrame;
                this.Grounded = other.Grounded;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.MoveSpeedSimulationFrame = other.MoveSpeedSimulationFrame;
                this.MoveSpeed = other.MoveSpeed;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.StunnedSimulationFrame = other.StunnedSimulationFrame;
                this.Stunned = other.Stunned;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.DeadSimulationFrame = other.DeadSimulationFrame;
                this.Dead = other.Dead;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.TwoHandedSimulationFrame = other.TwoHandedSimulationFrame;
                this.TwoHanded = other.TwoHanded;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_61db515efe9556c45853cdc384e6813b_26648702269276731 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 9);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.ShieldParry;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.Parry;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.Attacking;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {

                Coherence.Utils.Bounds.Check(data.WeaponDirectionNESO, _WeaponDirectionNESO_Min, _WeaponDirectionNESO_Max, "_61db515efe9556c45853cdc384e6813b_26648702269276731.WeaponDirectionNESO", logger);

                data.WeaponDirectionNESO = Coherence.Utils.Bounds.Clamp(data.WeaponDirectionNESO, _WeaponDirectionNESO_Min, _WeaponDirectionNESO_Max);

                var fieldValue = data.WeaponDirectionNESO;



                bitStream.WriteIntegerRange(fieldValue, 3, -1);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.Grounded;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.MoveSpeed;



                bitStream.WriteFloat(fieldValue, FloatMeta.NoCompression());
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.Stunned;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.Dead;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.TwoHanded;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _61db515efe9556c45853cdc384e6813b_26648702269276731 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(9);
            }

            var val = new _61db515efe9556c45853cdc384e6813b_26648702269276731();
            if (bitStream.ReadMask())
            {

                val.ShieldParry = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.ShieldParryMask;
            }
            if (bitStream.ReadMask())
            {

                val.Parry = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.ParryMask;
            }
            if (bitStream.ReadMask())
            {

                val.Attacking = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.AttackingMask;
            }
            if (bitStream.ReadMask())
            {

                val.WeaponDirectionNESO = bitStream.ReadIntegerRange(3, -1);
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.WeaponDirectionNESOMask;
            }
            if (bitStream.ReadMask())
            {

                val.Grounded = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.GroundedMask;
            }
            if (bitStream.ReadMask())
            {

                val.MoveSpeed = bitStream.ReadFloat(FloatMeta.NoCompression());
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.MoveSpeedMask;
            }
            if (bitStream.ReadMask())
            {

                val.Stunned = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.StunnedMask;
            }
            if (bitStream.ReadMask())
            {

                val.Dead = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.DeadMask;
            }
            if (bitStream.ReadMask())
            {

                val.TwoHanded = bitStream.ReadBool();
                val.FieldsMask |= _61db515efe9556c45853cdc384e6813b_26648702269276731.TwoHandedMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }


        public override string ToString()
        {
            return $"_61db515efe9556c45853cdc384e6813b_26648702269276731(" +
                $" ShieldParry: { this.ShieldParry }" +
                $" Parry: { this.Parry }" +
                $" Attacking: { this.Attacking }" +
                $" WeaponDirectionNESO: { this.WeaponDirectionNESO }" +
                $" Grounded: { this.Grounded }" +
                $" MoveSpeed: { this.MoveSpeed }" +
                $" Stunned: { this.Stunned }" +
                $" Dead: { this.Dead }" +
                $" TwoHanded: { this.TwoHanded }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(9, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(9, '0') })";
        }
    }

}
