using System;
using System.Collections;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

namespace PSoft.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _mainCamera;
        
        [Header("Input Actions")] 
        public PlayerInputActions PlayerControls;
        private InputAction _tilt;
        private InputAction _jump;
        private InputAction _cameraMove;
        
        [Header("Physics")]
        public Rigidbody rb;
        private float _tiltValueX;
        private float _tiltValueY;
        private Vector3 _cameraVector;
        private Quaternion _cameraRotation;
        
        [Header("Controls")]
        public float tiltSensitivity;
        public float jumpSensitivity;
        public float airFloatValue;
        public Transform stickingPoint;
        private bool _grounded;
        public float groundCheckLength;

        public LayerMask groundLayer; // For optimization but rn just gonna set it to everything lol.

        public bool bIsJumping = false;
        
        
        private void Awake()
        {
            PlayerControls = new PlayerInputActions();
            _mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _grounded = false;
        }
        //Here we bind the InputActions
        private void OnEnable()
        {
            _tilt = PlayerControls.WeaponControl.Tilt;
            _tilt.Enable();
            _tilt.performed += x => _tiltValueX += x.ReadValue<Vector2>().x;
            _tilt.performed += x => _tiltValueY += x.ReadValue<Vector2>().y;
            
            _jump = PlayerControls.WeaponControl.Jump;
            _jump.Enable();
            _jump.performed += OnJump;
            
        }

        private void OnDisable()
        {
            _tilt.Disable();
            _jump.Disable(); 
        }

        private void FixedUpdate()
        {
            // ToDo: Does it maybe have something to do with this!
            
            Quaternion tiltRotation = Quaternion.Euler(_tiltValueY * tiltSensitivity,0, _tiltValueX * tiltSensitivity * -1);
            transform.rotation = tiltRotation;
            _cameraRotation = Quaternion.LookRotation(_mainCamera.transform.forward, Vector3.up);
            transform.rotation = (_cameraRotation.normalized * tiltRotation);

            if (CheckGround() == false) 
            {
                // not on ground so apply gravity
                rb.AddForce(new Vector3(0, -1.0f, 0) * (rb.mass * airFloatValue)); //Manuel Gravity
            }

        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (_grounded)
            {
                Debug.Log("Jump");
                StartCoroutine(StickTimer());
                bIsJumping = true;
                rb.AddForce( jumpSensitivity * this.transform.up, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("OnJump _grounded FALSE");
            }
        }
        
        // Return true 
        public bool CheckGround()
        {
            
            // Raycast start/end
            Vector3 origin = transform.position;
            Vector3 direction = -transform.up;
            
            // DrawLine needs a world location and Raycast needs a direction. We need to create the end point for the draw.
            Vector3 debugDrawEndPoint = origin + direction * groundCheckLength;
            Debug.DrawLine(origin, debugDrawEndPoint, Color.green, 0.1f);
            
            if (Physics.Raycast(origin, direction, out RaycastHit hit, groundCheckLength, groundLayer)) // ToDo: Ground layer later.
            {
                // ToDo: Should this just be removed if we use the LayerMask?
                if (hit.collider.CompareTag("Ground"))
                {
                    _grounded = true;
                    rb.constraints = RigidbodyConstraints.FreezePosition;
                    Debug.Log("Stuck landing although this already worked so wtf am I doing omg.");
                    return true;
                }
            }
        
            // No ground
            return false;
        }
        
        //Added so that the player can take off the ground , otherwise the ray will keep the player grounded
        IEnumerator StickTimer()
        {
            rb.constraints = RigidbodyConstraints.None;
            yield return new WaitForSeconds(0.05f);
            _grounded = false;
        }
    }
}
