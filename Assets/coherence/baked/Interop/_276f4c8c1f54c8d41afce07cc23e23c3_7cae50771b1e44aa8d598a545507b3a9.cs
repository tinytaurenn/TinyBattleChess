// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
    using Coherence.ProtocolDef;
    using Coherence.Serializer;
    using Coherence.Brook;
    using Coherence.Entities;
    using Coherence.Log;
    using Coherence.Core;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public struct _276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9 : IEntityCommand
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Int32 DirectionNESO;
            [FieldOffset(4)]
            public Entity sync;
            [FieldOffset(8)]
            public System.Int32 damage;
            [FieldOffset(12)]
            public System.Int32 damageType;
            [FieldOffset(16)]
            public System.Int32 weaponType;
            [FieldOffset(20)]
            public Vector3 attackerPos;
        }

        public static unsafe _276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9 FromInterop(System.IntPtr data, System.Int32 dataSize) 
        {
            if (dataSize != 32) {
                throw new System.Exception($"Given data size is not equal to the struct size. ({dataSize} != 32) " +
                    "for command with ID 32");
            }

            var orig = new _276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9();
            var comp = (Interop*)data;
            orig.DirectionNESO = comp->DirectionNESO;
            orig.sync = comp->sync;
            orig.damage = comp->damage;
            orig.damageType = comp->damageType;
            orig.weaponType = comp->weaponType;
            orig.attackerPos = comp->attackerPos;
            return orig;
        }

        public System.Int32 DirectionNESO;
        public Entity sync;
        public System.Int32 damage;
        public System.Int32 damageType;
        public System.Int32 weaponType;
        public Vector3 attackerPos;
        
        public Entity Entity { get; set; }
        public Coherence.ChannelID ChannelID { get; set; }
        public MessageTarget Routing { get; set; }
        public uint Sender { get; set; }
        public uint GetComponentType() => 32;
        
        public IEntityMessage Clone()
        {
            // This is a struct, so we can safely return
            // a struct copy.
            return this;
        }
        
        public IEntityMapper.Error MapToAbsolute(IEntityMapper mapper, Coherence.Log.Logger logger)
        {
            var err = mapper.MapToAbsoluteEntity(Entity, false, out var absoluteEntity);
            if (err != IEntityMapper.Error.None)
            {
                return err;
            }
            Entity = absoluteEntity;
            err = mapper.MapToAbsoluteEntity(sync, false, out absoluteEntity);
            if (err != IEntityMapper.Error.None)
            {
                return err;
            }
            this.sync = absoluteEntity;
            
            return IEntityMapper.Error.None;
        }
        
        public IEntityMapper.Error MapToRelative(IEntityMapper mapper, Coherence.Log.Logger logger)
        {
            var err = mapper.MapToRelativeEntity(Entity, false, out var relativeEntity);
            if (err != IEntityMapper.Error.None)
            {
                return err;
            }
            Entity = relativeEntity;
            err = mapper.MapToRelativeEntity(sync, false, out relativeEntity);
            if (err != IEntityMapper.Error.None)
            {
                return err;
            }
            this.sync = relativeEntity;
            
            return IEntityMapper.Error.None;
        }

        public HashSet<Entity> GetEntityRefs() {
            return new HashSet<Entity> {
                this.sync,
            };
        }

        public void NullEntityRefs(Entity entity) {
            if (this.sync == entity) {
                this.sync = Entity.InvalidRelative;
            }
        }
        
        public _276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9(
        Entity entity,
        System.Int32 DirectionNESO,
        Entity sync,
        System.Int32 damage,
        System.Int32 damageType,
        System.Int32 weaponType,
        Vector3 attackerPos
)
        {
            Entity = entity;
            ChannelID = Coherence.ChannelID.Default;
            Routing = MessageTarget.All;
            Sender = 0;
            
            this.DirectionNESO = DirectionNESO; 
            this.sync = sync; 
            this.damage = damage; 
            this.damageType = damageType; 
            this.weaponType = weaponType; 
            this.attackerPos = attackerPos; 
        }
        
        public static void Serialize(_276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9 commandData, IOutProtocolBitStream bitStream)
        {
            bitStream.WriteIntegerRange(commandData.DirectionNESO, 32, -2147483648);
            bitStream.WriteEntity(commandData.sync);
            bitStream.WriteIntegerRange(commandData.damage, 32, -2147483648);
            bitStream.WriteIntegerRange(commandData.damageType, 32, -2147483648);
            bitStream.WriteIntegerRange(commandData.weaponType, 32, -2147483648);
            var converted_attackerPos = commandData.attackerPos.ToCoreVector3();
            bitStream.WriteVector3(converted_attackerPos, FloatMeta.NoCompression());
        }
        
        public static _276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9 Deserialize(IInProtocolBitStream bitStream, Entity entity, MessageTarget target)
        {
            var dataDirectionNESO = bitStream.ReadIntegerRange(32, -2147483648);
            var datasync = bitStream.ReadEntity();
            var datadamage = bitStream.ReadIntegerRange(32, -2147483648);
            var datadamageType = bitStream.ReadIntegerRange(32, -2147483648);
            var dataweaponType = bitStream.ReadIntegerRange(32, -2147483648);
            var converted_attackerPos = bitStream.ReadVector3(FloatMeta.NoCompression());
            var dataattackerPos = converted_attackerPos.ToUnityVector3();
    
            return new _276f4c8c1f54c8d41afce07cc23e23c3_7cae50771b1e44aa8d598a545507b3a9()
            {
                Entity = entity,
                Routing = target,
                DirectionNESO = dataDirectionNESO,
                sync = datasync,
                damage = datadamage,
                damageType = datadamageType,
                weaponType = dataweaponType,
                attackerPos = dataattackerPos
            };   
        }
    }

}
