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
    public class Binding_28fbed09d7fea464681134cd159332ad_0e075063fb18424282823e79e81dd576 : PositionBinding
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
    public class Binding_28fbed09d7fea464681134cd159332ad_58c2a149a2914fb8a73bfb1ef0160c5e : RotationBinding
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
    public class Binding_28fbed09d7fea464681134cd159332ad_77d4e83346c24cc5b4469ee50e1f7aff : ScaleBinding
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
    public class Binding_28fbed09d7fea464681134cd159332ad_c0c8579c969640d991b0f90e6df02026 : StringBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_28fbed09d7fea464681134cd159332ad_2187496778573307055);
        public override string CoherenceComponentName => "_28fbed09d7fea464681134cd159332ad_2187496778573307055";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.String Value
        {
            get { return (System.String)(CastedUnityComponent.path); }
            set { CastedUnityComponent.path = (System.String)(value); }
        }

        protected override (System.String value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_28fbed09d7fea464681134cd159332ad_2187496778573307055)coherenceComponent).path;

            var simFrame = ((_28fbed09d7fea464681134cd159332ad_2187496778573307055)coherenceComponent).pathSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_28fbed09d7fea464681134cd159332ad_2187496778573307055)coherenceComponent;
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
            return new _28fbed09d7fea464681134cd159332ad_2187496778573307055();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_28fbed09d7fea464681134cd159332ad_d8d34e5ca56e47ec8b7976a942b9256d : IntBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_28fbed09d7fea464681134cd159332ad_2187496778573307055);
        public override string CoherenceComponentName => "_28fbed09d7fea464681134cd159332ad_2187496778573307055";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Int32 Value
        {
            get { return (System.Int32)(CastedUnityComponent.pathDirtyCounter); }
            set { CastedUnityComponent.pathDirtyCounter = (System.Int32)(value); }
        }

        protected override (System.Int32 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_28fbed09d7fea464681134cd159332ad_2187496778573307055)coherenceComponent).pathDirtyCounter;

            var simFrame = ((_28fbed09d7fea464681134cd159332ad_2187496778573307055)coherenceComponent).pathDirtyCounterSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_28fbed09d7fea464681134cd159332ad_2187496778573307055)coherenceComponent;
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
            return new _28fbed09d7fea464681134cd159332ad_2187496778573307055();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_28fbed09d7fea464681134cd159332ad_76cca3f4f88744268881834be5238550 : BoolBinding
    {   
        private global::UnityEngine.Rigidbody CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Rigidbody)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_28fbed09d7fea464681134cd159332ad_1064604958396770310);
        public override string CoherenceComponentName => "_28fbed09d7fea464681134cd159332ad_1064604958396770310";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.isKinematic); }
            set { CastedUnityComponent.isKinematic = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_28fbed09d7fea464681134cd159332ad_1064604958396770310)coherenceComponent).isKinematic;

            var simFrame = ((_28fbed09d7fea464681134cd159332ad_1064604958396770310)coherenceComponent).isKinematicSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_28fbed09d7fea464681134cd159332ad_1064604958396770310)coherenceComponent;
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
            return new _28fbed09d7fea464681134cd159332ad_1064604958396770310();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_28fbed09d7fea464681134cd159332ad_556dd49c13a7499493ad88b9cd522da6 : BoolBinding
    {   
        private global::UnityEngine.BoxCollider CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.BoxCollider)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_28fbed09d7fea464681134cd159332ad_1985906079244697212);
        public override string CoherenceComponentName => "_28fbed09d7fea464681134cd159332ad_1985906079244697212";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.enabled); }
            set { CastedUnityComponent.enabled = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_28fbed09d7fea464681134cd159332ad_1985906079244697212)coherenceComponent).enabled;

            var simFrame = ((_28fbed09d7fea464681134cd159332ad_1985906079244697212)coherenceComponent).enabledSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_28fbed09d7fea464681134cd159332ad_1985906079244697212)coherenceComponent;
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
            return new _28fbed09d7fea464681134cd159332ad_1985906079244697212();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_28fbed09d7fea464681134cd159332ad_fc2d362490f947fb890c61f4f995ec08 : BoolBinding
    {   
        private global::MeleeWeapon CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::MeleeWeapon)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_28fbed09d7fea464681134cd159332ad_10605204515277231747);
        public override string CoherenceComponentName => "_28fbed09d7fea464681134cd159332ad_10605204515277231747";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.m_IsHeld); }
            set { CastedUnityComponent.m_IsHeld = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_28fbed09d7fea464681134cd159332ad_10605204515277231747)coherenceComponent).m_IsHeld;

            var simFrame = ((_28fbed09d7fea464681134cd159332ad_10605204515277231747)coherenceComponent).m_IsHeldSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_28fbed09d7fea464681134cd159332ad_10605204515277231747)coherenceComponent;
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
            return new _28fbed09d7fea464681134cd159332ad_10605204515277231747();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_28fbed09d7fea464681134cd159332ad_c0a12d121de14e4e8290f33a3192e182 : BoolBinding
    {   
        private global::MeleeWeapon CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::MeleeWeapon)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_28fbed09d7fea464681134cd159332ad_10605204515277231747);
        public override string CoherenceComponentName => "_28fbed09d7fea464681134cd159332ad_10605204515277231747";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.IsNPCHeld); }
            set { CastedUnityComponent.IsNPCHeld = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_28fbed09d7fea464681134cd159332ad_10605204515277231747)coherenceComponent).IsNPCHeld;

            var simFrame = ((_28fbed09d7fea464681134cd159332ad_10605204515277231747)coherenceComponent).IsNPCHeldSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_28fbed09d7fea464681134cd159332ad_10605204515277231747)coherenceComponent;
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
            return new _28fbed09d7fea464681134cd159332ad_10605204515277231747();
        }    
    }

    [UnityEngine.Scripting.Preserve]
    public class CoherenceSync_28fbed09d7fea464681134cd159332ad : CoherenceSyncBaked
    {
        private Entity entityId;
        private Logger logger = Coherence.Log.Log.GetLogger<CoherenceSync_28fbed09d7fea464681134cd159332ad>();
        
        private global::BasicWeapon _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1_CommandTarget;
        private global::BasicWeapon _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13_CommandTarget;
        private global::Grabbable _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1_CommandTarget;
        private global::Grabbable _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845_CommandTarget;
        
        
        private IClient client;
        private CoherenceBridge bridge;
        
        private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
        {
            ["0e075063fb18424282823e79e81dd576"] = new Binding_28fbed09d7fea464681134cd159332ad_0e075063fb18424282823e79e81dd576(),
            ["58c2a149a2914fb8a73bfb1ef0160c5e"] = new Binding_28fbed09d7fea464681134cd159332ad_58c2a149a2914fb8a73bfb1ef0160c5e(),
            ["77d4e83346c24cc5b4469ee50e1f7aff"] = new Binding_28fbed09d7fea464681134cd159332ad_77d4e83346c24cc5b4469ee50e1f7aff(),
            ["c0c8579c969640d991b0f90e6df02026"] = new Binding_28fbed09d7fea464681134cd159332ad_c0c8579c969640d991b0f90e6df02026(),
            ["d8d34e5ca56e47ec8b7976a942b9256d"] = new Binding_28fbed09d7fea464681134cd159332ad_d8d34e5ca56e47ec8b7976a942b9256d(),
            ["76cca3f4f88744268881834be5238550"] = new Binding_28fbed09d7fea464681134cd159332ad_76cca3f4f88744268881834be5238550(),
            ["556dd49c13a7499493ad88b9cd522da6"] = new Binding_28fbed09d7fea464681134cd159332ad_556dd49c13a7499493ad88b9cd522da6(),
            ["fc2d362490f947fb890c61f4f995ec08"] = new Binding_28fbed09d7fea464681134cd159332ad_fc2d362490f947fb890c61f4f995ec08(),
            ["c0a12d121de14e4e8290f33a3192e182"] = new Binding_28fbed09d7fea464681134cd159332ad_c0a12d121de14e4e8290f33a3192e182(),
        };
        
        private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings = new Dictionary<string, Action<CommandBinding, CommandsHandler>>();
        
        public CoherenceSync_28fbed09d7fea464681134cd159332ad()
        {
            bakedCommandBindings.Add("e03d06e78c53453bbda79474fc01aca1", BakeCommandBinding__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1);
            bakedCommandBindings.Add("dfb618dd8c58424c8cec7115f6ab3e13", BakeCommandBinding__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13);
            bakedCommandBindings.Add("6e69be271d504db1a9580f5f95194fb1", BakeCommandBinding__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1);
            bakedCommandBindings.Add("5ee4e2f3fe6e4aec944eacb3571db845", BakeCommandBinding__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845);
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
    
        private void BakeCommandBinding__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1_CommandTarget = (global::BasicWeapon)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("BasicWeapon.SyncHitSound", "(System.Int32)", SendCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1, ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1, MessageTarget.All, _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1_CommandTarget, false);
        }
        
        private void SendCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1();
            
            int i = 0;
            command.index = (System.Int32)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1();
            
            int i = 0;
            command.index = (System.Int32)args[i++];
            
            ReceiveCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1(command);
        }

        private void ReceiveCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1(_28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1 command)
        {
            var target = _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1_CommandTarget;
            
            target.SyncHitSound((System.Int32)(command.index));
        }
    
        private void BakeCommandBinding__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13_CommandTarget = (global::BasicWeapon)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("BasicWeapon.PlayParryFX", "(System.Int32)", SendCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13, ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13, MessageTarget.All, _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13_CommandTarget, false);
        }
        
        private void SendCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13();
            
            int i = 0;
            command.choiceIndex = (System.Int32)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13();
            
            int i = 0;
            command.choiceIndex = (System.Int32)args[i++];
            
            ReceiveCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13(command);
        }

        private void ReceiveCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13(_28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13 command)
        {
            var target = _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13_CommandTarget;
            
            target.PlayParryFX((System.Int32)(command.choiceIndex));
        }
    
        private void BakeCommandBinding__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1_CommandTarget = (global::Grabbable)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Grabbable.EnableComponent", "(System.Boolean)", SendCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1, ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1, MessageTarget.All, _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1_CommandTarget, false);
        }
        
        private void SendCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1();
            
            int i = 0;
            command.enable = (System.Boolean)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1();
            
            int i = 0;
            command.enable = (System.Boolean)args[i++];
            
            ReceiveCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1(command);
        }

        private void ReceiveCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1(_28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1 command)
        {
            var target = _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1_CommandTarget;
            
            target.EnableComponent((System.Boolean)(command.enable));
        }
    
        private void BakeCommandBinding__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845_CommandTarget = (global::Grabbable)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Grabbable.DestroyGrabbable", "()", SendCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845, ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845, MessageTarget.All, _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845_CommandTarget, false);
        }
        
        private void SendCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845();
            
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845();
            
            
            ReceiveCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845(command);
        }

        private void ReceiveCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845(_28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845 command)
        {
            var target = _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845_CommandTarget;
            
            target.DestroyGrabbable();
        }
        
        public override void ReceiveCommand(IEntityCommand command)
        {
            switch (command)
            {
                case _28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1 castedCommand:
                    ReceiveCommand__28fbed09d7fea464681134cd159332ad_e03d06e78c53453bbda79474fc01aca1(castedCommand);
                    break;
                case _28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13 castedCommand:
                    ReceiveCommand__28fbed09d7fea464681134cd159332ad_dfb618dd8c58424c8cec7115f6ab3e13(castedCommand);
                    break;
                case _28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1 castedCommand:
                    ReceiveCommand__28fbed09d7fea464681134cd159332ad_6e69be271d504db1a9580f5f95194fb1(castedCommand);
                    break;
                case _28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845 castedCommand:
                    ReceiveCommand__28fbed09d7fea464681134cd159332ad_5ee4e2f3fe6e4aec944eacb3571db845(castedCommand);
                    break;
                default:
                    logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncReceiveCommandUnhandled,
                        $"CoherenceSync_28fbed09d7fea464681134cd159332ad Unhandled command: {command.GetType()}.");
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
            this.logger = logger.With<CoherenceSync_28fbed09d7fea464681134cd159332ad>();
            this.bridge = bridge;
            this.entityId = entityId;
            this.client = client;        
        }
    }
}
