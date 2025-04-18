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
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_89dfd80bf73a4c589db9b215a469339d : PositionBinding
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
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_27ab0be9c8774a8f8c9b8ba0662a491e : RotationBinding
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
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_2d17d1d9dad243f48be342aa582dbf27 : ScaleBinding
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
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_9eac22263c114e4c87cb3b5f2fc13812 : BoolBinding
    {   
        private global::UnityEngine.BoxCollider CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.BoxCollider)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_2c19a04c42faa6b4398e588b58d31f5f_6415732900446197949);
        public override string CoherenceComponentName => "_2c19a04c42faa6b4398e588b58d31f5f_6415732900446197949";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.enabled); }
            set { CastedUnityComponent.enabled = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_2c19a04c42faa6b4398e588b58d31f5f_6415732900446197949)coherenceComponent).enabled;

            var simFrame = ((_2c19a04c42faa6b4398e588b58d31f5f_6415732900446197949)coherenceComponent).enabledSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_2c19a04c42faa6b4398e588b58d31f5f_6415732900446197949)coherenceComponent;
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
            return new _2c19a04c42faa6b4398e588b58d31f5f_6415732900446197949();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_712d914db9d64f4d87af6635b029b4f2 : BoolBinding
    {   
        private global::UnityEngine.Rigidbody CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Rigidbody)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_2c19a04c42faa6b4398e588b58d31f5f_3384389735842056370);
        public override string CoherenceComponentName => "_2c19a04c42faa6b4398e588b58d31f5f_3384389735842056370";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.isKinematic); }
            set { CastedUnityComponent.isKinematic = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_2c19a04c42faa6b4398e588b58d31f5f_3384389735842056370)coherenceComponent).isKinematic;

            var simFrame = ((_2c19a04c42faa6b4398e588b58d31f5f_3384389735842056370)coherenceComponent).isKinematicSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_2c19a04c42faa6b4398e588b58d31f5f_3384389735842056370)coherenceComponent;
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
            return new _2c19a04c42faa6b4398e588b58d31f5f_3384389735842056370();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_2190773f691946bcb59fbab24d613243 : StringBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607);
        public override string CoherenceComponentName => "_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.String Value
        {
            get { return (System.String)(CastedUnityComponent.path); }
            set { CastedUnityComponent.path = (System.String)(value); }
        }

        protected override (System.String value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607)coherenceComponent).path;

            var simFrame = ((_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607)coherenceComponent).pathSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607)coherenceComponent;
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
            return new _2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_7a3fdf9d48b64e9795214fcb05cb70f8 : IntBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607);
        public override string CoherenceComponentName => "_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Int32 Value
        {
            get { return (System.Int32)(CastedUnityComponent.pathDirtyCounter); }
            set { CastedUnityComponent.pathDirtyCounter = (System.Int32)(value); }
        }

        protected override (System.Int32 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607)coherenceComponent).pathDirtyCounter;

            var simFrame = ((_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607)coherenceComponent).pathDirtyCounterSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607)coherenceComponent;
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
            return new _2c19a04c42faa6b4398e588b58d31f5f_17001797403845922607();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_8e1f963aa62d4bb3a52b0bfb3c3c14a9 : BoolBinding
    {   
        private global::ShieldWeapon CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::ShieldWeapon)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928);
        public override string CoherenceComponentName => "_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.m_IsHeld); }
            set { CastedUnityComponent.m_IsHeld = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928)coherenceComponent).m_IsHeld;

            var simFrame = ((_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928)coherenceComponent).m_IsHeldSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928)coherenceComponent;
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
            return new _2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_2c19a04c42faa6b4398e588b58d31f5f_0f58eed1f55a47a38dc0354dd3ca9589 : BoolBinding
    {   
        private global::ShieldWeapon CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::ShieldWeapon)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928);
        public override string CoherenceComponentName => "_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.IsNPCHeld); }
            set { CastedUnityComponent.IsNPCHeld = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928)coherenceComponent).IsNPCHeld;

            var simFrame = ((_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928)coherenceComponent).IsNPCHeldSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928)coherenceComponent;
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
            return new _2c19a04c42faa6b4398e588b58d31f5f_11115768030746041928();
        }    
    }

    [UnityEngine.Scripting.Preserve]
    public class CoherenceSync_2c19a04c42faa6b4398e588b58d31f5f : CoherenceSyncBaked
    {
        private Entity entityId;
        private Logger logger = Coherence.Log.Log.GetLogger<CoherenceSync_2c19a04c42faa6b4398e588b58d31f5f>();
        
        private global::BasicWeapon _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c_CommandTarget;
        private global::BasicWeapon _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb_CommandTarget;
        private global::Grabbable _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785_CommandTarget;
        private global::Grabbable _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4_CommandTarget;
        
        
        private IClient client;
        private CoherenceBridge bridge;
        
        private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
        {
            ["89dfd80bf73a4c589db9b215a469339d"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_89dfd80bf73a4c589db9b215a469339d(),
            ["27ab0be9c8774a8f8c9b8ba0662a491e"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_27ab0be9c8774a8f8c9b8ba0662a491e(),
            ["2d17d1d9dad243f48be342aa582dbf27"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_2d17d1d9dad243f48be342aa582dbf27(),
            ["9eac22263c114e4c87cb3b5f2fc13812"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_9eac22263c114e4c87cb3b5f2fc13812(),
            ["712d914db9d64f4d87af6635b029b4f2"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_712d914db9d64f4d87af6635b029b4f2(),
            ["2190773f691946bcb59fbab24d613243"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_2190773f691946bcb59fbab24d613243(),
            ["7a3fdf9d48b64e9795214fcb05cb70f8"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_7a3fdf9d48b64e9795214fcb05cb70f8(),
            ["8e1f963aa62d4bb3a52b0bfb3c3c14a9"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_8e1f963aa62d4bb3a52b0bfb3c3c14a9(),
            ["0f58eed1f55a47a38dc0354dd3ca9589"] = new Binding_2c19a04c42faa6b4398e588b58d31f5f_0f58eed1f55a47a38dc0354dd3ca9589(),
        };
        
        private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings = new Dictionary<string, Action<CommandBinding, CommandsHandler>>();
        
        public CoherenceSync_2c19a04c42faa6b4398e588b58d31f5f()
        {
            bakedCommandBindings.Add("cd4f976528974ea08a88cc166afba53c", BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c);
            bakedCommandBindings.Add("98e4d64c0cd14791a30552087866e8eb", BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb);
            bakedCommandBindings.Add("687f0df4b5af4859b2ce0f55b7ed8785", BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785);
            bakedCommandBindings.Add("8ca844bd9f6e4783afcdb34ed5e1dae4", BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4);
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
    
        private void BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c_CommandTarget = (global::BasicWeapon)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("BasicWeapon.SyncHitSound", "(System.Int32)", SendCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c, ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c, MessageTarget.All, _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c_CommandTarget, false);
        }
        
        private void SendCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c();
            
            int i = 0;
            command.index = (System.Int32)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c();
            
            int i = 0;
            command.index = (System.Int32)args[i++];
            
            ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c(command);
        }

        private void ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c(_2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c command)
        {
            var target = _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c_CommandTarget;
            
            target.SyncHitSound((System.Int32)(command.index));
        }
    
        private void BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb_CommandTarget = (global::BasicWeapon)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("BasicWeapon.PlayParryFX", "(System.Int32)", SendCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb, ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb, MessageTarget.All, _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb_CommandTarget, false);
        }
        
        private void SendCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb();
            
            int i = 0;
            command.choiceIndex = (System.Int32)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb();
            
            int i = 0;
            command.choiceIndex = (System.Int32)args[i++];
            
            ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb(command);
        }

        private void ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb(_2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb command)
        {
            var target = _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb_CommandTarget;
            
            target.PlayParryFX((System.Int32)(command.choiceIndex));
        }
    
        private void BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785_CommandTarget = (global::Grabbable)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Grabbable.EnableComponent", "(System.Boolean)", SendCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785, ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785, MessageTarget.All, _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785_CommandTarget, false);
        }
        
        private void SendCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785();
            
            int i = 0;
            command.enable = (System.Boolean)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785();
            
            int i = 0;
            command.enable = (System.Boolean)args[i++];
            
            ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785(command);
        }

        private void ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785(_2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785 command)
        {
            var target = _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785_CommandTarget;
            
            target.EnableComponent((System.Boolean)(command.enable));
        }
    
        private void BakeCommandBinding__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4_CommandTarget = (global::Grabbable)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Grabbable.DestroyGrabbable", "()", SendCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4, ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4, MessageTarget.All, _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4_CommandTarget, false);
        }
        
        private void SendCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4();
            
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4();
            
            
            ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4(command);
        }

        private void ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4(_2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4 command)
        {
            var target = _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4_CommandTarget;
            
            target.DestroyGrabbable();
        }
        
        public override void ReceiveCommand(IEntityCommand command)
        {
            switch (command)
            {
                case _2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c castedCommand:
                    ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_cd4f976528974ea08a88cc166afba53c(castedCommand);
                    break;
                case _2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb castedCommand:
                    ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_98e4d64c0cd14791a30552087866e8eb(castedCommand);
                    break;
                case _2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785 castedCommand:
                    ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_687f0df4b5af4859b2ce0f55b7ed8785(castedCommand);
                    break;
                case _2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4 castedCommand:
                    ReceiveCommand__2c19a04c42faa6b4398e588b58d31f5f_8ca844bd9f6e4783afcdb34ed5e1dae4(castedCommand);
                    break;
                default:
                    logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncReceiveCommandUnhandled,
                        $"CoherenceSync_2c19a04c42faa6b4398e588b58d31f5f Unhandled command: {command.GetType()}.");
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
            this.logger = logger.With<CoherenceSync_2c19a04c42faa6b4398e588b58d31f5f>();
            this.bridge = bridge;
            this.entityId = entityId;
            this.client = client;        
        }
    }
}
