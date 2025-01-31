using System;
using System.Collections;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEditor.ShaderGraph;
using UnityEngine.Rendering.VirtualTexturing;
using Object = System.Object;

namespace PSoft.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _mainCamera;
        public CinemachineCamera followCamera;
        
        [Header("Input Actions")] 
        private PlayerInputActions _playerControls;
        private InputAction _tilt;
        private InputAction _jump;
        private InputAction _cameraMove;
        private InputAction _cameraSwap;
        private InputAction _ghostMove;
        private InputAction _ghostLook;
        private InputAction _possess;
        
        [Header("Physics")]
        private Rigidbody _rb;
        private float _tiltValueX;
        private float _tiltValueY;
        private Vector3 _cameraVector;
        private Quaternion _cameraRotation;
        
        [Header("Handle Controls")]
        private GameObject _handle;
        public float tiltSensitivity;
        public float jumpSensitivity;
        public float airFloatValue;
        private Transform _stickingPoint;
        private bool _grounded;
        public float groundCheckLength;
        private bool _freeLook;

        [Header("Ghost Controls")] 
        public float ghostSpeed;
        private float ghostCameraSpeed;
        private bool _ghostMode;
        private Vector3 _ghostVector;
        public Transform orientation;
        public Transform possessPoint;
        public GameObject _handleTarget;

        
        
        //Basic Controller setup
        private void Awake()
        {
            followCamera.enabled = true;
            
            _playerControls = new PlayerInputActions();
            _mainCamera = Camera.main;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _grounded = false;
            _freeLook = false;
            
            _handleTarget = null;
        }
        //Here we bind the InputActions
        private void OnEnable()
        {
            _playerControls.Enable();
            
            _tilt = _playerControls.WeaponControl.Tilt;
            _tilt.performed += x => _tiltValueX += x.ReadValue<Vector2>().x;
            _tilt.performed += x => _tiltValueY += x.ReadValue<Vector2>().y;

            _cameraSwap = _playerControls.WeaponControl.CameraSwap;
            _cameraSwap.started += CameraSwap;
            _cameraSwap.performed += CameraSwap;
            
            _jump = _playerControls.WeaponControl.Jump;
            _jump.performed += OnJump;

            _possess = _playerControls.GhostControl.Possess;
            _possess.performed += OnPossess;
            
            _ghostMove = _playerControls.GhostControl.Move;
            _ghostMove.performed += GhostMove;
            _ghostMove.canceled += GhostMove;
        }

        private void OnDisable()
        {
            _tilt.Disable();
            _jump.Disable(); 
            _cameraSwap.Disable();
            _ghostMove.Disable();
        }

        public void Start()
        {
            _ghostMode = true;
            _playerControls.GhostControl.Enable();
            _playerControls.WeaponControl.Disable();
        }
        
        private void FixedUpdate()
        {
            if (!_ghostMode)
            {
                //Move Hammer if not in freelook mode
                if (!_freeLook)
                {
                    Quaternion tiltRotation = Quaternion.Euler(_tiltValueY * tiltSensitivity,0, _tiltValueX * tiltSensitivity * -1);
                    _cameraRotation = Quaternion.LookRotation(_mainCamera.transform.forward, Vector3.up);
                    _handle.transform.rotation = (_cameraRotation.normalized * tiltRotation);
                }
                CheckGround();
                //Manuel Gravity
                _rb.AddForce(new Vector3(0, -1.0f, 0) * (_rb.mass * airFloatValue));
            }
            else
            {
                CheckHandle();
                //Orientation of camera compared to ghost
                Vector3 viewDir = transform.position - new Vector3(followCamera.transform.position.x, followCamera.transform.position.y, followCamera.transform.position.z);
                orientation.forward = viewDir.normalized;
                Vector3 inputDir = orientation.right * _ghostVector.x + orientation.up * _ghostVector.y + orientation.forward * _ghostVector.z;
                //Move ghost
                transform.position += inputDir * ghostSpeed;
            }
        }
        
        
        private void OnPossess(InputAction.CallbackContext context)
        {
            if (_ghostMode)
            {
                if (_handleTarget != null)
                {
                    _stickingPoint = _handleTarget.transform.Find("StickingPoint");
                    _handle = _handleTarget;
                    _rb = _handleTarget.GetComponent<Rigidbody>();
                    _rb.useGravity = false;
                    followCamera.Follow = _handleTarget.transform;
                    _playerControls.GhostControl.Move.Disable();
                    _ghostMode = !_ghostMode;
                    _playerControls.WeaponControl.Enable();
                
                    _freeLook = false;
                    followCamera.GetComponent<CinemachineInputAxisController>().enabled = false;
                    _grounded = true;
                }
            }
            else
            {
                _stickingPoint = null;
                _handle = null;
                _rb.useGravity = true;
                _rb = null;
                followCamera.Follow = this.transform;
                _playerControls.GhostControl.Move.Enable();
                _ghostMode = true;
                _playerControls.WeaponControl.Disable();
                followCamera.GetComponent<CinemachineInputAxisController>().enabled = true;
            }
            
        }
        
       

        private void CameraSwap(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _freeLook = !_freeLook;
                followCamera.GetComponent<CinemachineInputAxisController>().enabled = true;
            }
            else if (context.performed)
            {
                _freeLook = !_freeLook;
                followCamera.GetComponent<CinemachineInputAxisController>().enabled = false;
            }
        }

       

        private void GhostMove(InputAction.CallbackContext values)
        {
            _ghostVector = values.ReadValue<Vector3>();
        }
        
    

        private void OnJump(InputAction.CallbackContext context)
        {
            if (_grounded)
            {
                StartCoroutine(StickTimer());
                _rb.AddForce( jumpSensitivity * _handle.transform.up, ForceMode.Impulse);
            }
        }

        private void CheckHandle()
        {
            RaycastHit hit;
            if(Physics.Raycast(possessPoint.position, possessPoint.forward,out hit,2f))
            {
                if (hit.transform.CompareTag("Handle"))
                {
                    //UI to signifie possess here
                    _handleTarget = hit.collider.gameObject;
                }
                else
                {
                    _handleTarget = null;
                }
            }
            else
            {
                _handleTarget = null;
            }
        }
        private void CheckGround()
        {
            if(_grounded) return;
            RaycastHit hit;
            if(Physics.Raycast(_stickingPoint.position, _stickingPoint.up,out hit,groundCheckLength))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    Debug.DrawRay(transform.TransformDirection(_stickingPoint.position), transform.TransformDirection(_stickingPoint.transform.up) * hit.distance, Color.red);
                    _grounded = true;
                    _rb.constraints = RigidbodyConstraints.FreezePosition;
                }
            }
        }
        //Added so that the player can take off the ground , otherwise the ray will keep the player grounded
        IEnumerator StickTimer()
        {
            _rb.constraints = RigidbodyConstraints.None;
            yield return new WaitForSeconds(0.05f);
            _grounded = false;
        }
    }
}
