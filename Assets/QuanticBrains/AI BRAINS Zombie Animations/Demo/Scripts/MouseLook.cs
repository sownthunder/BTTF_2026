using UnityEngine;

namespace QuanticBrains.AI_BRAINS_Zombie_Animations.Demo
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public Transform body;
        public float crouchDistance = -1.0f;

        private float _crouchSpeed = 20.0f;
        private float _xRotation = 0f;
        private Vector3 _startingEyePosition;

        private Vector3 _targetEyePosition;
        
        private float _timeSinceActivation = 1.0f;

        private void Start()
        {
            _startingEyePosition = transform.localPosition;
            _targetEyePosition = _startingEyePosition; 
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (_timeSinceActivation > 0)
            {
                _timeSinceActivation -= Time.deltaTime;
                return;
            }
            
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);


            if (!transform.localPosition.Equals(_targetEyePosition))
            {
                //lerp eye position 
                transform.localPosition = Vector3.Lerp(transform.localPosition, _targetEyePosition, _crouchSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.localPosition, _targetEyePosition) < 0.001f)
                {
                    transform.localPosition = _targetEyePosition;
                }
            }

            //set crouch
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                _targetEyePosition = _startingEyePosition;
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _targetEyePosition = _startingEyePosition + (Vector3.up * crouchDistance);
            }

        }
    }
}
