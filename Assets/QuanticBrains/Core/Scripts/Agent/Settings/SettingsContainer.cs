using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using Unity.MLAgents;
using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.Settings
{
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [DisableInInlineEditors]
    public class SettingsContainer : ScriptableObject
    {
        [TabGroup("Settings")]
        [Tooltip("Increase or decrease the scene gravity. Use ~3x to make things less floaty")]
        public float gravityMultiplier = 1.0f;
        [TabGroup("Settings")]
        [Tooltip("The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) are performed.")]
        public float fixedDeltaTime = .02f;
        [TabGroup("Settings")]
        [Tooltip("The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) will be performed only for this duration of time per frame.")]
        public float maximumDeltaTime = 1.0f / 3.0f;
        [TabGroup("Settings")]
        [Tooltip("The maximum time a frame can spend on particle updates. If the frame takes longer than this, then updates are split into multiple smaller updates.")]
        public float maximumParticleDeltaTime = 0.03f;
        [TabGroup("Settings")]
        [Tooltip("Determines how accurately Rigidbody joints and collision contacts are resolved. (default 6). Must be positive.")]
        public int solverIterations = 6;
        [TabGroup("Settings")]
        [Tooltip("Affects how accurately the Rigidbody joints and collision contacts are resolved. (default 1). Must be positive.")]
        public int solverVelocityIterations = 1;
        [TabGroup("Settings")]
        [Tooltip("The maximum default velocity needed to move a Rigidbody's collider out of another collider's surface penetration. (default 10). Must be positive.")]
        public int maxDepenetrationVelocity = 10;
        [TabGroup("Settings")]
        [Tooltip("Determines whether the garbage collector should reuse only a single instance of a Collision type for all collision callbacks. Reduces Garbage.")]
        public bool reuseCollisionCallbacks = true;
        [TabGroup("Settings")]
        [Tooltip("Whether or not to automatically sync transform changes with the physics system whenever a Transform component changes.")]
        public bool autoSyncTransforms = false;

        // Original values
        [TabGroup("Default")]
        public Vector3 originalGravity = new Vector3(0, -9.81f, 0);
        [TabGroup("Default")]
        [Tooltip("The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) are performed.")]
        public float originalFixedDeltaTime = .02f;
        [TabGroup("Default")]
        [Tooltip("The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) will be performed only for this duration of time per frame.")]
        public float originalMaximumDeltaTime = 1.0f / 3.0f;
        [TabGroup("Default")]
        [Tooltip("The maximum time a frame can spend on particle updates. If the frame takes longer than this, then updates are split into multiple smaller updates.")]
        public float originalMaximumParticleDeltaTime = 0.03f;
        [TabGroup("Default")]
        [Tooltip("Determines how accurately Rigidbody joints and collision contacts are resolved. (default 6). Must be positive.")]
        public int originalSolverIterations = 6;
        [TabGroup("Default")]
        [Tooltip("Affects how accurately the Rigidbody joints and collision contacts are resolved. (default 1). Must be positive.")]
        public int originalSolverVelocityIterations = 1;
        [TabGroup("Default")]
        [Tooltip("The maximum default velocity needed to move a Rigidbody's collider out of another collider's surface penetration. (default 10). Must be positive.")]
        public float originalMaxDepenetrationVelocity = 10;
        [TabGroup("Default")]
        [Tooltip("Determines whether the garbage collector should reuse only a single instance of a Collision type for all collision callbacks. Reduces Garbage.")]
        public bool originalReuseCollisionCallbacks = true;
        [TabGroup("Default")]
        [Tooltip("Whether or not to automatically sync transform changes with the physics system whenever a Transform component changes.")]
        public bool originalAutoSyncTransforms = false;

        public void Initialize(MonoBehaviour mb)
        {
            // Override
            Physics.gravity                         = originalGravity * gravityMultiplier;
            Time.fixedDeltaTime                     = fixedDeltaTime;
            Time.maximumDeltaTime                   = maximumDeltaTime;
            Time.maximumParticleDeltaTime           = maximumParticleDeltaTime;
            Physics.defaultSolverIterations         = solverIterations;
            Physics.defaultSolverVelocityIterations = solverVelocityIterations;
            Physics.defaultMaxDepenetrationVelocity = maxDepenetrationVelocity;
            Physics.reuseCollisionCallbacks         = reuseCollisionCallbacks;
            Physics.autoSyncTransforms              = autoSyncTransforms;

            // Make sure the Academy singleton is initialized first, since it will create the SideChannels.
            Academy.Instance.EnvironmentParameters.RegisterCallback("gravity", f => { Physics.gravity = new Vector3(0, -f, 0); });
            mb.OnDestroyAsObservable().Subscribe(_ => Reset()).AddTo(mb);
        }

        private void Reset()
        {
            Physics.gravity                         = originalGravity;
            Time.fixedDeltaTime                     = originalFixedDeltaTime;
            Time.maximumDeltaTime                   = originalMaximumDeltaTime;
            Time.maximumParticleDeltaTime           = originalMaximumParticleDeltaTime;
            Physics.defaultSolverIterations         = originalSolverIterations;
            Physics.defaultSolverVelocityIterations = originalSolverVelocityIterations;
            Physics.defaultMaxDepenetrationVelocity = originalMaxDepenetrationVelocity;
            Physics.reuseCollisionCallbacks         = originalReuseCollisionCallbacks;
            Physics.autoSyncTransforms              = originalAutoSyncTransforms;
        }
    }
}
