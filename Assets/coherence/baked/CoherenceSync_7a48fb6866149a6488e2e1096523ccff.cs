// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Coherence.Toolkit;
    using Coherence.Toolkit.Bindings;
    using Coherence.Entities;
    using Coherence.ProtocolDef;
    using Coherence.Brook;
    using Coherence.Toolkit.Bindings.ValueBindings;
    using Coherence.Toolkit.Bindings.TransformBindings;
    using Coherence.Connection;
    using Coherence.SimulationFrame;
    using Coherence.Interpolation;
    using Coherence.Log;
    using Logger = Coherence.Log.Logger;
    using UnityEngine.Scripting;
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_03b69c4690834be8903da26c87bd2fd6 : PositionBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(WorldPosition);
        public override string CoherenceComponentName => "WorldPosition";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(coherenceSync.coherencePosition); }
            set { coherenceSync.coherencePosition = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((WorldPosition)coherenceComponent).value;
            if (!coherenceSync.HasParentWithCoherenceSync) { value += floatingOriginDelta; }

            var simFrame = ((WorldPosition)coherenceComponent).valueSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (WorldPosition)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.value = Value;
            }
            else
            {
                update.value = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.valueSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new WorldPosition();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_408d81f5a8384299a63b7f52f4fda451 : RotationBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(WorldOrientation);
        public override string CoherenceComponentName => "WorldOrientation";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Quaternion Value
        {
            get { return (UnityEngine.Quaternion)(coherenceSync.coherenceRotation); }
            set { coherenceSync.coherenceRotation = (UnityEngine.Quaternion)(value); }
        }

        protected override (UnityEngine.Quaternion value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((WorldOrientation)coherenceComponent).value;

            var simFrame = ((WorldOrientation)coherenceComponent).valueSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (WorldOrientation)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.value = Value;
            }
            else
            {
                update.value = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.valueSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new WorldOrientation();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_f85bc0bf0b3f430f9634026e6f307542 : ScaleBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(GenericScale);
        public override string CoherenceComponentName => "GenericScale";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(coherenceSync.coherenceLocalScale); }
            set { coherenceSync.coherenceLocalScale = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((GenericScale)coherenceComponent).value;

            var simFrame = ((GenericScale)coherenceComponent).valueSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (GenericScale)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.value = Value;
            }
            else
            {
                update.value = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.valueSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new GenericScale();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_11faa060080d4cbfa8ba34428fc7bccb : StringBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_7a48fb6866149a6488e2e1096523ccff_5153324027185580596);
        public override string CoherenceComponentName => "_7a48fb6866149a6488e2e1096523ccff_5153324027185580596";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.String Value
        {
            get { return (System.String)(CastedUnityComponent.path); }
            set { CastedUnityComponent.path = (System.String)(value); }
        }

        protected override (System.String value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_7a48fb6866149a6488e2e1096523ccff_5153324027185580596)coherenceComponent).path;

            var simFrame = ((_7a48fb6866149a6488e2e1096523ccff_5153324027185580596)coherenceComponent).pathSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_7a48fb6866149a6488e2e1096523ccff_5153324027185580596)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.path = Value;
            }
            else
            {
                update.path = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.pathSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _7a48fb6866149a6488e2e1096523ccff_5153324027185580596();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_e7fc31fa4db341d6ad9c18d9955972eb : IntBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_7a48fb6866149a6488e2e1096523ccff_5153324027185580596);
        public override string CoherenceComponentName => "_7a48fb6866149a6488e2e1096523ccff_5153324027185580596";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Int32 Value
        {
            get { return (System.Int32)(CastedUnityComponent.pathDirtyCounter); }
            set { CastedUnityComponent.pathDirtyCounter = (System.Int32)(value); }
        }

        protected override (System.Int32 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_7a48fb6866149a6488e2e1096523ccff_5153324027185580596)coherenceComponent).pathDirtyCounter;

            var simFrame = ((_7a48fb6866149a6488e2e1096523ccff_5153324027185580596)coherenceComponent).pathDirtyCounterSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_7a48fb6866149a6488e2e1096523ccff_5153324027185580596)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.pathDirtyCounter = Value;
            }
            else
            {
                update.pathDirtyCounter = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.pathDirtyCounterSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _7a48fb6866149a6488e2e1096523ccff_5153324027185580596();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_988f93baebfe4d5e9566aeeb0120e389 : BoolBinding
    {   
        private global::Chest_Armor CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Chest_Armor)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_7a48fb6866149a6488e2e1096523ccff_5843287501086471510);
        public override string CoherenceComponentName => "_7a48fb6866149a6488e2e1096523ccff_5843287501086471510";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.m_IsHeld); }
            set { CastedUnityComponent.m_IsHeld = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_7a48fb6866149a6488e2e1096523ccff_5843287501086471510)coherenceComponent).m_IsHeld;

            var simFrame = ((_7a48fb6866149a6488e2e1096523ccff_5843287501086471510)coherenceComponent).m_IsHeldSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_7a48fb6866149a6488e2e1096523ccff_5843287501086471510)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.m_IsHeld = Value;
            }
            else
            {
                update.m_IsHeld = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.m_IsHeldSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _7a48fb6866149a6488e2e1096523ccff_5843287501086471510();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_a62dbcc6ca6643f28ffeb92e3c0b7f46 : BoolBinding
    {   
        private global::Chest_Armor CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Chest_Armor)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_7a48fb6866149a6488e2e1096523ccff_5843287501086471510);
        public override string CoherenceComponentName => "_7a48fb6866149a6488e2e1096523ccff_5843287501086471510";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.IsNPCHeld); }
            set { CastedUnityComponent.IsNPCHeld = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_7a48fb6866149a6488e2e1096523ccff_5843287501086471510)coherenceComponent).IsNPCHeld;

            var simFrame = ((_7a48fb6866149a6488e2e1096523ccff_5843287501086471510)coherenceComponent).IsNPCHeldSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_7a48fb6866149a6488e2e1096523ccff_5843287501086471510)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.IsNPCHeld = Value;
            }
            else
            {
                update.IsNPCHeld = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.IsNPCHeldSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _7a48fb6866149a6488e2e1096523ccff_5843287501086471510();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_3e1461d49f9d42d78b0fe11a94c706aa : BoolBinding
    {   
        private global::UnityEngine.BoxCollider CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.BoxCollider)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_7a48fb6866149a6488e2e1096523ccff_3206994536959658864);
        public override string CoherenceComponentName => "_7a48fb6866149a6488e2e1096523ccff_3206994536959658864";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.enabled); }
            set { CastedUnityComponent.enabled = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_7a48fb6866149a6488e2e1096523ccff_3206994536959658864)coherenceComponent).enabled;

            var simFrame = ((_7a48fb6866149a6488e2e1096523ccff_3206994536959658864)coherenceComponent).enabledSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_7a48fb6866149a6488e2e1096523ccff_3206994536959658864)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.enabled = Value;
            }
            else
            {
                update.enabled = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.enabledSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _7a48fb6866149a6488e2e1096523ccff_3206994536959658864();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_7a48fb6866149a6488e2e1096523ccff_0e36ceae8dbd4a2e95648760ad1fca2e : BoolBinding
    {   
        private global::UnityEngine.Rigidbody CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Rigidbody)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_7a48fb6866149a6488e2e1096523ccff_7591037996061170254);
        public override string CoherenceComponentName => "_7a48fb6866149a6488e2e1096523ccff_7591037996061170254";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.isKinematic); }
            set { CastedUnityComponent.isKinematic = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_7a48fb6866149a6488e2e1096523ccff_7591037996061170254)coherenceComponent).isKinematic;

            var simFrame = ((_7a48fb6866149a6488e2e1096523ccff_7591037996061170254)coherenceComponent).isKinematicSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_7a48fb6866149a6488e2e1096523ccff_7591037996061170254)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.isKinematic = Value;
            }
            else
            {
                update.isKinematic = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.isKinematicSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _7a48fb6866149a6488e2e1096523ccff_7591037996061170254();
        }    
    }

    [UnityEngine.Scripting.Preserve]
    public class CoherenceSync_7a48fb6866149a6488e2e1096523ccff : CoherenceSyncBaked
    {
        private Entity entityId;
        private Logger logger = Coherence.Log.Log.GetLogger<CoherenceSync_7a48fb6866149a6488e2e1096523ccff>();
        
        private global::Grabbable _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c_CommandTarget;
        private global::Grabbable _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11_CommandTarget;
        
        
        private IClient client;
        private CoherenceBridge bridge;
        
        private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
        {
            ["03b69c4690834be8903da26c87bd2fd6"] = new Binding_7a48fb6866149a6488e2e1096523ccff_03b69c4690834be8903da26c87bd2fd6(),
            ["408d81f5a8384299a63b7f52f4fda451"] = new Binding_7a48fb6866149a6488e2e1096523ccff_408d81f5a8384299a63b7f52f4fda451(),
            ["f85bc0bf0b3f430f9634026e6f307542"] = new Binding_7a48fb6866149a6488e2e1096523ccff_f85bc0bf0b3f430f9634026e6f307542(),
            ["11faa060080d4cbfa8ba34428fc7bccb"] = new Binding_7a48fb6866149a6488e2e1096523ccff_11faa060080d4cbfa8ba34428fc7bccb(),
            ["e7fc31fa4db341d6ad9c18d9955972eb"] = new Binding_7a48fb6866149a6488e2e1096523ccff_e7fc31fa4db341d6ad9c18d9955972eb(),
            ["988f93baebfe4d5e9566aeeb0120e389"] = new Binding_7a48fb6866149a6488e2e1096523ccff_988f93baebfe4d5e9566aeeb0120e389(),
            ["a62dbcc6ca6643f28ffeb92e3c0b7f46"] = new Binding_7a48fb6866149a6488e2e1096523ccff_a62dbcc6ca6643f28ffeb92e3c0b7f46(),
            ["3e1461d49f9d42d78b0fe11a94c706aa"] = new Binding_7a48fb6866149a6488e2e1096523ccff_3e1461d49f9d42d78b0fe11a94c706aa(),
            ["0e36ceae8dbd4a2e95648760ad1fca2e"] = new Binding_7a48fb6866149a6488e2e1096523ccff_0e36ceae8dbd4a2e95648760ad1fca2e(),
        };
        
        private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings = new Dictionary<string, Action<CommandBinding, CommandsHandler>>();
        
        public CoherenceSync_7a48fb6866149a6488e2e1096523ccff()
        {
            bakedCommandBindings.Add("4986cfec4cc84eb1bfabd72178c7e14c", BakeCommandBinding__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c);
            bakedCommandBindings.Add("1fef2472c6cc42c7adb6c06ec6d89f11", BakeCommandBinding__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11);
        }
        
        public override Binding BakeValueBinding(Binding valueBinding)
        {
            if (bakedValueBindings.TryGetValue(valueBinding.guid, out var bakedBinding))
            {
                valueBinding.CloneTo(bakedBinding);
                return bakedBinding;
            }
            
            return null;
        }
        
        public override void BakeCommandBinding(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            if (bakedCommandBindings.TryGetValue(commandBinding.guid, out var commandBindingBaker))
            {
                commandBindingBaker.Invoke(commandBinding, commandsHandler);
            }
        }
    
        private void BakeCommandBinding__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c_CommandTarget = (global::Grabbable)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Grabbable.EnableComponent", "(System.Boolean)", SendCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c, ReceiveLocalCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c, MessageTarget.All, _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c_CommandTarget, false);
        }
        
        private void SendCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c();
            
            int i = 0;
            command.enable = (System.Boolean)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c();
            
            int i = 0;
            command.enable = (System.Boolean)args[i++];
            
            ReceiveCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c(command);
        }

        private void ReceiveCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c(_7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c command)
        {
            var target = _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c_CommandTarget;
            
            target.EnableComponent((System.Boolean)(command.enable));
        }
    
        private void BakeCommandBinding__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11_CommandTarget = (global::Grabbable)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Grabbable.DestroyGrabbable", "()", SendCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11, ReceiveLocalCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11, MessageTarget.All, _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11_CommandTarget, false);
        }
        
        private void SendCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11();
            
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11();
            
            
            ReceiveCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11(command);
        }

        private void ReceiveCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11(_7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11 command)
        {
            var target = _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11_CommandTarget;
            
            target.DestroyGrabbable();
        }
        
        public override void ReceiveCommand(IEntityCommand command)
        {
            switch (command)
            {
                case _7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c castedCommand:
                    ReceiveCommand__7a48fb6866149a6488e2e1096523ccff_4986cfec4cc84eb1bfabd72178c7e14c(castedCommand);
                    break;
                case _7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11 castedCommand:
                    ReceiveCommand__7a48fb6866149a6488e2e1096523ccff_1fef2472c6cc42c7adb6c06ec6d89f11(castedCommand);
                    break;
                default:
                    logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncReceiveCommandUnhandled,
                        $"CoherenceSync_7a48fb6866149a6488e2e1096523ccff Unhandled command: {command.GetType()}.");
                    break;
            }
        }
        
        public override List<ICoherenceComponentData> CreateEntity(bool usesLodsAtRuntime, string archetypeName, AbsoluteSimulationFrame simFrame)
        {
            if (!usesLodsAtRuntime)
            {
                return null;
            }
            
            if (Archetypes.IndexForName.TryGetValue(archetypeName, out int archetypeIndex))
            {
                var components = new List<ICoherenceComponentData>()
                {
                    new ArchetypeComponent
                    {
                        index = archetypeIndex,
                        indexSimulationFrame = simFrame,
                        FieldsMask = 0b1
                    }
                };

                return components;
            }
    
            logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncCreateEntityMissingArchetype,
                $"Unable to find archetype {archetypeName} in dictionary. Please, bake manually (coherence > Bake)");
            
            return null;
        }
        
        public override void Dispose()
        {
        }
        
        public override void Initialize(Entity entityId, CoherenceBridge bridge, IClient client, CoherenceInput input, Logger logger)
        {
            this.logger = logger.With<CoherenceSync_7a48fb6866149a6488e2e1096523ccff>();
            this.bridge = bridge;
            this.entityId = entityId;
            this.client = client;        
        }
    }
}
