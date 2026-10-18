using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations
{
    [RequireComponent(typeof(ZombieAgent))]
    public class ZombieController : MonoBehaviour
    {
        public LayerMask walkableLayerMask;
        public LayerMask obstaclesLayerMask;
        public float targetWalkingSpeed = 7.0f;
        
        public UnityEvent onStartWalking = new UnityEvent();
        public UnityEvent onFall= new UnityEvent();
        public UnityEvent<ZombieController> onDie = new UnityEvent<ZombieController>();
        public UnityEvent<ZombieController> onRevive = new UnityEvent<ZombieController>();
        public UnityEvent<GameObject> onReachedTarget = new UnityEvent<GameObject>();
        
        [HideInInspector]
        public bool IsEnabled => _zombieAgent.enabled;

        private ZombieAgent _zombieAgent;

        private void Awake()
        {
            _zombieAgent = GetComponent<ZombieAgent>();
            _zombieAgent.Initialize(walkableLayerMask, targetWalkingSpeed, obstaclesLayerMask); 
        }

        private void OnEnable()
        {
            _zombieAgent.OnReachedTarget    += _OnReachedTarget;
            _zombieAgent.OnStartWalking     += _OnStartWalking;
            _zombieAgent.OnFall             += _OnFall;
            _zombieAgent.OnDie              += _OnDie;
            _zombieAgent.OnRevive           += _OnRevive;
        }

        private void OnDisable()
        {
            _zombieAgent.OnReachedTarget    -= _OnReachedTarget;
            _zombieAgent.OnStartWalking     -= _OnStartWalking;
            _zombieAgent.OnFall             -= _OnFall;
            _zombieAgent.OnDie              -= _OnDie;
            _zombieAgent.OnRevive           -= _OnRevive;
        }

        private void _OnReachedTarget(GameObject go)
        {
            onReachedTarget?.Invoke(go);
        }

        private void _OnStartWalking()
        {
            onStartWalking?.Invoke();
        }

        private void _OnFall()
        {
            onFall?.Invoke();
        }
        
        private void _OnDie()
        {
            onDie?.Invoke(this);
        }

        private void _OnRevive()
        {
            onRevive?.Invoke(this);
        }        

        public void SetTarget(Transform newTarget)
        {
            _zombieAgent.SetTarget(newTarget);
        }
        
        public void SetTarget(Vector3 position)
        {
            _zombieAgent.SetTarget(position);
        }

        public void Kill()
        {
            _zombieAgent.Kill();
        }

        public void Revive()
        {
            _zombieAgent.Revive();
        }
    }
}
