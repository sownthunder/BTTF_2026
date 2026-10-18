using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations.Demo
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController characterController;
        public float speed = 12f;
        public Transform groundCheck;
        public float gravity = 20.0f;
        public LayerMask groundMask;
        public float jumpHeight = 3.0f;
        public GameObject bulletPrefab;

        public Transform weaponMuzzle;

        private Vector3 _moveVelocity;
        private Vector3 _verticalVelocity;
        private float _groundCheckRadius = 0.4f;
        private bool _isGrounded;

        void Update()
        {
            //check ground
            _isGrounded = Physics.CheckSphere(groundCheck.position, _groundCheckRadius, groundMask);
            if (_isGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = -2f;
            }
            
            //MOVE
            _moveVelocity = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * speed * Time.deltaTime;
            characterController.Move(_moveVelocity);
            
            //jump
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }                        
            
            //fall
            _verticalVelocity.y += gravity * Time.deltaTime;

            //Fall/Jump
            characterController.Move(_verticalVelocity * Time.deltaTime);

            //fire
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet");
                if (bullet != null)
                {
                    bullet.transform.position = weaponMuzzle.position;
                    bullet.transform.rotation = weaponMuzzle.rotation;
                    bullet.SetActive(true);
                }
                //Instantiate(bulletPrefab, weaponMuzzle.position, weaponMuzzle.rotation);
            }

        }
    }
}
