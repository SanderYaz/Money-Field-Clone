using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed;
        private Rigidbody _rb;


        private PlayerController _controller;
        public Vector3 LastDirection { get; private set; }

        public bool IsMoving { get; private set; }
        public FloatingJoystick FloatingJoystick { get; private set; }

        public void Initialize(PlayerController playerController)
        {
            if (_rb == null || _rb != GetComponent<Rigidbody>())
                _rb = GetComponent<Rigidbody>();
            if (FloatingJoystick == null || FloatingJoystick != FloatingJoystick.instance)
                FloatingJoystick = FloatingJoystick.instance;
        
        
            _controller = playerController;
        }


        public void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            var direction = Vector3.forward * FloatingJoystick.Vertical + Vector3.right * FloatingJoystick.Horizontal;
            var velocity = new Vector3(direction.x, -0.1f, direction.z);
        
        
            LastDirection = direction != Vector3.zero ? direction : Vector3.zero;
            
            IsMoving = !(direction.magnitude <= 0.15f);
            if (IsMoving)
                _rb.velocity = velocity * speed * Time.fixedDeltaTime;
            else
            {
                _rb.velocity = Vector3.zero;
            }
            if (LastDirection == Vector3.zero || !IsMoving) return;
            var newRotation = Quaternion.LookRotation(LastDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * 12);
        }
    }
}