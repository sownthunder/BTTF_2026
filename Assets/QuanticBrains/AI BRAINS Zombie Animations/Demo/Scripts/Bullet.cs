using System;
using System.Collections;
using QuanticBrains.AI_BRAINS_Zombie_Animations;
using UnityEngine;


namespace QuanticBrains.Product.AI_BRAINS_Zombie_Animations.Demo
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 1.0f;
        public float hitRadius = 0.3f; 
        public float hitForce = 1.0f;
        public float velocityToDoDamage = 5.0f;
        
        public AudioClip shoot;
        public AudioClip hit;
        
        private Rigidbody _rb;
        private Coroutine _coroutine;
        
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            if (Time.timeSinceLevelLoad > 1f)
                AudioSource.PlayClipAtPoint(shoot, Camera.main.transform.position, 0.6f);
            
            _rb.velocity = Vector3.zero;

            _rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
            
            //disable after a while
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(DisableAfter(5f));
        }

        private void OnCollisionEnter(Collision collision)
        {
            //don't do damage if too slow
            if (_rb.velocity.magnitude < velocityToDoDamage) return;
            
            ZombieController zombie = collision.collider.GetComponentInParent<ZombieController>();
            if(zombie != null)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, hitRadius);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null && rb.gameObject.layer != this.gameObject.layer)
                    {
                        rb.AddExplosionForce(hitForce, transform.position, hitRadius, 2.0F, ForceMode.Impulse);
                    }
                }
                AudioSource.PlayClipAtPoint(hit, Camera.main.transform.position, 0.6f);
                zombie.Kill();
            }
        }

        IEnumerator DisableAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            this.gameObject.SetActive(false);
        }
    }
}
