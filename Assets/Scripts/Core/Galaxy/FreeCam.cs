using System;
using UnityEngine;

namespace Core.Galaxy
{
    [Serializable]
    public class FreeCam : MonoBehaviour
    {
        [Header("Constants")]

        //unity controls and constants input
        public float AccelerationMod;
        public float XAxisSensitivity;
        public float YAxisSensitivity;
        public float DecelerationMod;

        [Space]

        [Range(0, 89)] public float MaxXAngle = 60f;

        [Space]

        public float MaximumMovementSpeed = 1f;
    
        private Vector3 _moveSpeed;


        private void Start()
        {
            _moveSpeed = Vector3.zero;

        }

        // Update is called once per frame
        private void Update()
        {
            if (PlayerDataManager.Instance.RunHandler.IsPaused) return;
            
            HandleMouseRotation();

            var acceleration = HandleKeyInput();

            _moveSpeed += acceleration;

            HandleDeceleration(acceleration);

            // clamp the move speed
            if (_moveSpeed.magnitude > MaximumMovementSpeed)
            {
                _moveSpeed = _moveSpeed.normalized * MaximumMovementSpeed;
            }

            transform.Translate(_moveSpeed);
        }

        private Vector3 HandleKeyInput()
        {
            var acceleration = Vector3.zero;

            //key input detection
            if (InputService.GetAxisIsActive(KAction.Vertical))
            {
                acceleration.z += InputService.GetAxisRaw(KAction.Vertical);
            }

            if (InputService.GetAxisIsActive(KAction.Horizontal))
            {
                acceleration.x += InputService.GetAxisRaw(KAction.Horizontal);
            }

            if (InputService.GetAxisIsActive(KAction.GalaxyVertical))
            {
                acceleration.y -= InputService.GetAxisRaw(KAction.GalaxyVertical);
            }

            return acceleration.normalized * AccelerationMod;
        }

        private float _rotationX;

        private void HandleMouseRotation()
        {
            //mouse input
            var rotationHorizontal = XAxisSensitivity * Input.GetAxis("Mouse X");
            var rotationVertical = YAxisSensitivity * Input.GetAxis("Mouse Y");

            //applying mouse rotation
            // always rotate Y in global world space to avoid gimbal lock
            transform.Rotate(Vector3.up * rotationHorizontal, Space.World);

            var rotationY = transform.localEulerAngles.y;

            _rotationX += rotationVertical;
            _rotationX = Mathf.Clamp(_rotationX, -MaxXAngle, MaxXAngle);

            transform.localEulerAngles = new Vector3(-_rotationX, rotationY, 0);
        }

        private void HandleDeceleration(Vector3 acceleration)
        {
            //deceleration functionality
            if (Mathf.Approximately(Mathf.Abs(acceleration.x), 0))
            {
                if (Mathf.Abs(_moveSpeed.x) < DecelerationMod)
                {
                    _moveSpeed.x = 0;
                }
                else
                {
                    _moveSpeed.x -= DecelerationMod * Mathf.Sign(_moveSpeed.x);
                }
            }

            if (Mathf.Approximately(Mathf.Abs(acceleration.y), 0))
            {
                if (Mathf.Abs(_moveSpeed.y) < DecelerationMod)
                {
                    _moveSpeed.y = 0;
                }
                else
                {
                    _moveSpeed.y -= DecelerationMod * Mathf.Sign(_moveSpeed.y);
                }
            }

            if (Mathf.Approximately(Mathf.Abs(acceleration.z), 0))
            {
                if (Mathf.Abs(_moveSpeed.z) < DecelerationMod)
                {
                    _moveSpeed.z = 0;
                }
                else
                {
                    _moveSpeed.z -= DecelerationMod * Mathf.Sign(_moveSpeed.z);
                }
            }
        }

    }
}