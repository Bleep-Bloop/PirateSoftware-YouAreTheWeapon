using System;
using System.Collections;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEditor.ShaderGraph;

namespace PSoft.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _mainCamera;
        public CinemachineCamera followCamera;
        
        [Header("Input Actions")] 
        public PlayerInputActions PlayerControls;
        private InputAction _tilt;
        private InputAction _jump;
        private InputAction _cameraMove;
        private InputAction _cameraSwap;
        
        [Header("Physics")]
        public Rigidbody rb;
        private float _tiltValueX;
        private float _tiltValueY;
        private Vector3 _cameraVector;
        private Quaternion _cameraRotation;
        
        [Header("Handle Controls")]
        public float tiltSensitivity;
        public float jumpSensitivity;
        public float airFloatValue;
        public Transform stickingPoint;
        private bool _grounded;
        public float groundCheckLength;
        private bool _freeLook;
           
        
        //Basic Controller setup
        private void Awake()
        {
            followCamera.enabled = true;
            
            PlayerControls = new PlayerInputActions();
            _mainCamera = Camera.main;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _grounded = false;
            _freeLook = false;
        }
        //Here we bind the InputActions
        private void OnEnable()
        {
            _tilt = PlayerControls.WeaponControl.Tilt;
            _tilt.Enable();
            _tilt.performed += x => _tiltValueX += x.ReadValue<Vector2>().x;
            _tilt.performed += x => _tiltValueY += x.ReadValue<Vector2>().y;

            _cameraSwap = PlayerControls.WeaponControl.CameraSwap;
            _cameraSwap.Enable();
            _cameraSwap.started += CameraSwap;
            _cameraSwap.performed += CameraSwap;
            
            _jump = PlayerControls.WeaponControl.Jump;
            _jump.Enable();
            _jump.performed += OnJump;
            
        }

        private void OnDisable()
        {
            _tilt.Disable();
            _jump.Disable(); 
            _cameraSwap.Disable();
        }

        private void CameraSwap(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                
                Debug.Log("Pressed");
                _freeLook = !_freeLook;
                followCamera.GetComponent<CinemachineInputAxisController>().enabled = true;
            }
            else if (context.performed)
            {
                Debug.Log("Released");
                _freeLook = !_freeLook;
                followCamera.enabled = true;
                followCamera.GetComponent<CinemachineInputAxisController>().enabled = false;
            }
        }

        private void FixedUpdate()
        {
            //Move Hammer if not in freelook mode
            if (!_freeLook)
            {
                Quaternion tiltRotation = Quaternion.Euler(_tiltValueY * tiltSensitivity,0, _tiltValueX * tiltSensitivity * -1);
                _cameraRotation = Quaternion.LookRotation(_mainCamera.transform.forward, Vector3.up);
                transform.rotation = (_cameraRotation.normalized * tiltRotation);
            }
            CheckGround();
            //Manuel Gravity
            rb.AddForce(new Vector3(0, -1.0f, 0) * (rb.mass * airFloatValue));
        }

        


        private void OnJump(InputAction.CallbackContext context)
        {
            if (_grounded)
            {
                Debug.Log("Jump");
                StartCoroutine(StickTimer());
                rb.AddForce( jumpSensitivity * this.transform.up, ForceMode.Impulse);
            }
        }

        private void CheckGround()
        {
            if(_grounded) return;
            RaycastHit hit;
            if(Physics.Raycast(stickingPoint.position, stickingPoint.up,out hit,groundCheckLength))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    Debug.DrawRay(transform.TransformDirection(stickingPoint.position), transform.TransformDirection(stickingPoint.transform.up) * hit.distance, Color.red);
                    _grounded = true;
                    rb.constraints = RigidbodyConstraints.FreezePosition;
                    Debug.Log("Stuck the landing");
                }
            }
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
