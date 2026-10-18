using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations.Demo
{
    public class ZombieDemoController : MonoBehaviour
    {
        public ZombieController[] zombieControllers;
        public Transform initialTarget;

        public AudioClip[] zombieAwakeAudios;
        
        private float _minReviveTime = 1.0f;
        private float _maxReviveTime = 5.0f;

        
        private void Start()
        {
            foreach (var zombieController in zombieControllers)
            {
                zombieController.SetTarget(initialTarget);
                zombieController.onDie.AddListener(OnZombieDie);
            }
        }

        private void OnZombieDie(ZombieController zombie)
        { 
            //revive after a while
            Observable
                .Timer(TimeSpan.FromSeconds(Random.Range(_minReviveTime, _maxReviveTime)))
                .Subscribe(_ =>
                {
                    AudioSource.PlayClipAtPoint(
                            zombieAwakeAudios[Random.Range(0, zombieAwakeAudios.Length)],
                            zombie.transform.position
                        );
                    zombie.Revive();
                })
                .AddTo(this);
        }
    }
}
