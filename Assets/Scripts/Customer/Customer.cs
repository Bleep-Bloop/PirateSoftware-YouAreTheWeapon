using System;
using System.Collections;
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
        [SerializeField] private float delayBeforeRequestDisplay = 2.0f; // Time in seconds before a customer displays their request after arriving at the counter.

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

        // Displays this customer's current weapon request to the player in the game world. This is purely cosmetic.
        private void DisplayCurrentWeaponRequestInWorld()
        {
            // ToDo: Display this customer's weapon request through an image or text bubble.
            // Temporary debug.log for testing.
            Debug.Log("Customer::DisplayCurrentWeaponRequest(): I would like a " + _currentWeaponRequest);
        }
        
        // OK I am passing a callback to MoveCustomerTo that speicfies what to do upon arrival. This keeps the logic modular and reusable.
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

        public void SetWaypointLocations(Vector3 inStartLocation, Vector3 inCounterLocation, Vector3 inExitLocation)
        {
            _startWaypoint = inStartLocation;
            _counterWaypoint = inCounterLocation;
            _exitWaypoint = inExitLocation;
        }

        private void OnReachedCounter()
        {
            Debug.Log("Customer::OnReachedCounter(): Counter Reached");
            
            // After arriving rotate the customer to face the player's camera.
            var lookAtRotation = Quaternion.LookRotation(_playerCamera.transform.position - transform.position);
            // Note .LookRotation constrains rotation to align with forward and up axes so we don't have to set local pitch angle to handle when the camera is above the character.
            transform.rotation = lookAtRotation; // ToDo: I could probably slerp but I am just going to snap rotation for now. Figure out improvement later.
            
            // Display the weapon request after a short delay.
            Invoke(nameof(DisplayCurrentWeaponRequestInWorld), delayBeforeRequestDisplay);
        }

    }
}
