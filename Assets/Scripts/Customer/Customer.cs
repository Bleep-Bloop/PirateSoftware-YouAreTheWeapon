using System;
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

        /* World locations used in moving this customer. */
        private Vector3 _startWaypoint;     // Customer spawn/starting location.
        private Vector3 _counterWaypoint;   // Location at counter where weapon request is made.
        private Vector3 _exitWaypoint;      // Location where customer exits after receiving weapon.

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
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
            // Find and save the game manager in our current scene.
            _gameManagerInstance = GameObject.FindFirstObjectByType<GameManager>();
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

            // Send the customer to the counter to give their weapon request.
            MoveCustomerTo(_counterWaypoint);
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

        // Sets this customer's NavMeshAgent's destination to the given Vector3. // ToDo: Add Event/Action/Delegate parameter to respond when destination reached?
        public void MoveCustomerTo(Vector3 inLocation)
        {
            // Move to given location.
            _navMeshAgent.destination = inLocation;
        }

        public void SetWaypointLocations(Vector3 inStartLocation, Vector3 inCounterLocation, Vector3 inExitLocation)
        {
            _startWaypoint = inStartLocation;
            _counterWaypoint = inCounterLocation;
            _exitWaypoint = inExitLocation;
        }

    }
}
