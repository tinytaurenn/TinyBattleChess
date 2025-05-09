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

    public struct _5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f : IEntityCommand
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public Vector3 pos;
        }

        public static unsafe _5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f FromInterop(System.IntPtr data, System.Int32 dataSize) 
        {
            if (dataSize != 12) {
                throw new System.Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for command with ID 83");
            }

            var orig = new _5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f();
            var comp = (Interop*)data;
            orig.pos = comp->pos;
            return orig;
        }

        public Vector3 pos;
        
        public Entity Entity { get; set; }
        public Coherence.ChannelID ChannelID { get; set; }
        public MessageTarget Routing { get; set; }
        public uint Sender { get; set; }
        public uint GetComponentType() => 83;
        
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
            return IEntityMapper.Error.None;
        }

        public HashSet<Entity> GetEntityRefs() {
            return default;
        }

        public void NullEntityRefs(Entity entity) {
        }
        
        public _5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f(
        Entity entity,
        Vector3 pos
)
        {
            Entity = entity;
            ChannelID = Coherence.ChannelID.Default;
            Routing = MessageTarget.All;
            Sender = 0;
            
            this.pos = pos; 
        }
        
        public static void Serialize(_5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f commandData, IOutProtocolBitStream bitStream)
        {
            var converted_pos = commandData.pos.ToCoreVector3();
            bitStream.WriteVector3(converted_pos, FloatMeta.NoCompression());
        }
        
        public static _5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f Deserialize(IInProtocolBitStream bitStream, Entity entity, MessageTarget target)
        {
            var converted_pos = bitStream.ReadVector3(FloatMeta.NoCompression());
            var datapos = converted_pos.ToUnityVector3();
    
            return new _5d6316af86183e242bf98f8a058fd8db_933039bfe4f14e489686412a9c5aba4f()
            {
                Entity = entity,
                Routing = target,
                pos = datapos
            };   
        }
    }

}
