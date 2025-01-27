using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace PSoft
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Customer : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private WeaponType _currentWeaponRequest; 
        private GameManager _gameManagerInstance; // The GameManager handling the current game instance.
        private GameObject _playerCamera;
        private Animator _animator; // ToDo: Removing the serialize and going through awake. I think that is the issue aha. BUT, its in a child so will that break something? I guess we will see lol.
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        [SerializeField] private float smoothRotationSpeed = 1.2f; // The rotation speed used by our SmoothlyRotateToTarget().

        [SerializeField] private GameObject weaponRequestBubblePrefab; // The prefab for the weapon request speech bubble.
        [SerializeField] private Transform weaponRequestBubbleLocation; // The parent/spawn location of our request bubble.
        private WorldSpaceTextBubble _activeWeaponRequestBubble;
        [SerializeField] private float delayBeforeRequestDisplay = 2.0f; // Time in seconds before a customer displays their request after arriving at the counter.
        [SerializeField] private float delayBeforeHidingWeaponRequest = 5.0f; // Time in seconds before the request bubble is hidden after displaying.
        
        /* World locations used in moving this customer. */
        private Vector3 _startWaypoint;     // Customer spawn/starting location.
        private Vector3 _counterWaypoint;   // Location at counter where weapon request is made.
        private Vector3 _exitWaypoint;      // Location where customer exits after receiving weapon.

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            // Keep our animator informed of our velocity.
            _animator.SetBool(IsMoving, _navMeshAgent.velocity.magnitude > 0.01f);
        }

        private void OnDestroy()
        {
            // Unbind/unsubscribe to game manager delegates/events when destroying.
            if (_gameManagerInstance != null)
            {
                _gameManagerInstance.OnRoundStartEvent -= OnRoundStart;
                _gameManagerInstance.OnRoundEndEvent -= OnRoundEnd;
            }
        }

        private void Start()
        {
            // Find and save objects in scene.
            _playerCamera = GameObject.FindWithTag("MainCamera");
            _gameManagerInstance = GameObject.FindFirstObjectByType<GameManager>(); // ToDo: Need if statement because I didn't find these and got error aha. Iunno prob could just leave whatever.
            
            // Create this customer's weapon request bubble.
            _activeWeaponRequestBubble = Instantiate(weaponRequestBubblePrefab, weaponRequestBubbleLocation.position, transform.rotation, weaponRequestBubbleLocation).GetComponent<WorldSpaceTextBubble>();
            _activeWeaponRequestBubble.gameObject.SetActive(false); // Hide it until ready to display.

            // And bind our round functions to the game manager's events.
            _gameManagerInstance.OnRoundStartEvent += OnRoundStart;
            _gameManagerInstance.OnRoundEndEvent += OnRoundEnd;
        }

        // An Event function for GameManager's OnRoundStartEvent. Prepares this customer for the round.
        private void OnRoundStart()
        {
            Debug.Log("Customer::OnRoundStart()");
            
            // Create a new random weapon request.
            SetCurrentWeaponRequest(GetRandomWeaponType());

            // Send the customer to the counter and invoke OnReachedCounter() on arrival.
            MoveCustomerTo(_counterWaypoint, OnReachedCounter);
        }
        
        // An Event function for GameManager's OnRoundEndEvent. Prepares this customer for the round.
        private void OnRoundEnd()
        {
            Debug.Log("Customer::OnRoundEnd()");

            // Send the customer to the exit
            MoveCustomerTo(_exitWaypoint);
        }

        private void SetCurrentWeaponRequest(WeaponType inWeaponType) 
        {
            // ToDo: Handle duplicate requests or weighted odds for types here.
            _currentWeaponRequest = inWeaponType;
        }

        private WeaponType GetRandomWeaponType()
        {
            var v = Enum.GetValues(typeof(WeaponType));
            return (WeaponType)v.GetValue(Random.Range(0, v.Length));
        }

        // Displays this customer's weapon request through a world space canvas before hiding it after a delay.
        private void DisplayCurrentWeaponRequestInWorld()
        {
            // Debug.Log("Customer::DisplayCurrentWeaponRequest(): I would like a " + _currentWeaponRequest);
            
            // Pass our weapon request to the request bubble.
            _activeWeaponRequestBubble.SetWeaponRequestImage(_currentWeaponRequest);

            // Rotate the bubble to face the player. ToDo/Question: Do I want to include a .y = 0 (to stop the speech bubble from rotating upwards when camera is above).
            _activeWeaponRequestBubble.transform.rotation = 
                Quaternion.LookRotation(_activeWeaponRequestBubble.transform.position - _playerCamera.transform.position);
 
            // Show the weapon request bubble.
            _activeWeaponRequestBubble.gameObject.SetActive(true);
            
            // Hide the request bubble after a delay.
            Invoke(nameof(HideRequestBubble), delayBeforeHidingWeaponRequest);
        }

        private void HideRequestBubble()
        {
            _activeWeaponRequestBubble.gameObject.SetActive(false);
        }

        // ToDo/Note: I added a callback to the parameters that specifies what to do on arrive. This keeps logic modular and reusable.
        private void MoveCustomerTo(Vector3 inLocation, System.Action onArrival = null)
        {
            if (!_navMeshAgent)
                return;
            
            // Move to given location.
            _navMeshAgent.destination = inLocation;
            StartCoroutine(WaitUntilArrival(onArrival)); // ToDo/Question: What if we don't pass. Should I check for null? Or will just writing this way auto check aha.
        }
        
        // Coroutine for Arrival handling
        private IEnumerator WaitUntilArrival(System.Action onArrival)
        {
            while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
            {
                yield return null; // Wait for the next frame.
            }

            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f) // ToDo/Question: I feel like the magnitude supposed to be 0.01 or whatever (maybe 0.1) because isn't it always a little above zero? From what other guy said aha.
            {
                onArrival.Invoke();
            }
        }

        // Starts a coroutine to smoothly rotate this customer to the target rotation.
        private void RotateCustomerToFaceTarget(Quaternion targetRotation)
        {
            StartCoroutine(SmoothlyRotateToTarget(targetRotation));
        }

        private IEnumerator SmoothlyRotateToTarget(Quaternion targetRotation)
        {
            float rotationSpeed = smoothRotationSpeed;
            float timeElapsed = 0f;
            
            // Save the initial rotation before we start rotating.
            Quaternion startRotation = transform.rotation;
            
            // Smoothly rotate over time.
            while (timeElapsed < 1f)
            {
                timeElapsed += Time.deltaTime * rotationSpeed; // Increment time.
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
                yield return null; // Wait until the next frame.
            }
            
            // Ensure the final rotation is the target rotation
            transform.rotation = targetRotation;
        }

        public void SetWaypointLocations(Vector3 inStartLocation, Vector3 inCounterLocation, Vector3 inExitLocation)
        {
            _startWaypoint = inStartLocation;
            _counterWaypoint = inCounterLocation;
            _exitWaypoint = inExitLocation;
        }

        private void OnReachedCounter()
        {
            Debug.Log("Customer::OnReachedCounter(): Counter Reached");

            // After arriving we rotate the customer to face the player's camera.
            var directionToPlayer = _playerCamera.transform.position - transform.position; 
            var targetRotation = Quaternion.LookRotation(directionToPlayer);
            // We only want to rotate along Y (Up/Down). Note: We use .Euler instead of setting x/y due to how Unity handles them internally. Safer this way.
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            
            RotateCustomerToFaceTarget(targetRotation);

            // Display the weapon request after a short delay.
            Invoke(nameof(DisplayCurrentWeaponRequestInWorld), delayBeforeRequestDisplay);
        }

    }
}
