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

    public struct _d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59 : IEntityCommand
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Int32 choiceIndex;
        }

        public static unsafe _d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59 FromInterop(System.IntPtr data, System.Int32 dataSize) 
        {
            if (dataSize != 4) {
                throw new System.Exception($"Given data size is not equal to the struct size. ({dataSize} != 4) " +
                    "for command with ID 134");
            }

            var orig = new _d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59();
            var comp = (Interop*)data;
            orig.choiceIndex = comp->choiceIndex;
            return orig;
        }

        public System.Int32 choiceIndex;
        
        public Entity Entity { get; set; }
        public Coherence.ChannelID ChannelID { get; set; }
        public MessageTarget Routing { get; set; }
        public uint Sender { get; set; }
        public uint GetComponentType() => 134;
        
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
        
        public _d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59(
        Entity entity,
        System.Int32 choiceIndex
)
        {
            Entity = entity;
            ChannelID = Coherence.ChannelID.Default;
            Routing = MessageTarget.All;
            Sender = 0;
            
            this.choiceIndex = choiceIndex; 
        }
        
        public static void Serialize(_d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59 commandData, IOutProtocolBitStream bitStream)
        {
            bitStream.WriteIntegerRange(commandData.choiceIndex, 32, -2147483648);
        }
        
        public static _d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59 Deserialize(IInProtocolBitStream bitStream, Entity entity, MessageTarget target)
        {
            var datachoiceIndex = bitStream.ReadIntegerRange(32, -2147483648);
    
            return new _d9c58831288052b4bb0dfa57ee24e75e_a1306575801b4bc8bbc106c5c843cc59()
            {
                Entity = entity,
                Routing = target,
                choiceIndex = datachoiceIndex
            };   
        }
    }

}
