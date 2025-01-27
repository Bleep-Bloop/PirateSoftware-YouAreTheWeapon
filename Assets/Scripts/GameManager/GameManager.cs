using System;
using UnityEngine;

namespace PSoft
{
    public class GameManager : MonoBehaviour
    {
        // ToDo: Make singleton.
        
        [SerializeField] private GameObject customerPrefab; // The prefab of the customer to be instantiated in our game loop.
        private Customer _activeCustomer; // The customer currently in use.
        [SerializeField] private float firstRoundDelayTime = 5.0f; // A time in seconds used to delay the first round start.

        /* Locations used in customer navigation */
        [SerializeField] private Transform customerStartLocation;   // The customer spawn location.
        [SerializeField] private Transform customerCounterLocation; // The location where a customer requests weapon from player.
        [SerializeField] private Transform customerExitLocation;    // The location where a customer leaves.

        //* Delegates/Events/Actions *//
        public event Action OnRoundStartEvent;  // Invoked when a new round is starting.
        public event Action OnRoundEndEvent;    // Invoked when a customer has received their request weapon.

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            
            // ToDo: Invoking SpawnCustomer after delay to see if it has to do with the transforms not being set aha.
            
            // Create our customer to be used. ToDo/Question: Should I create here, or just place in scene?
            //SpawnCustomer();
            Invoke(nameof(SpawnCustomer), 2.0f);
            
            // TEMPORARY: Start the first round after a short delay.
            Invoke(nameof(StartNewRound), firstRoundDelayTime);
            
            // TEMPORARY: Ending the round after another delay to test loop?
            // Invoke(nameof(EndRound), firstRoundDelayTime * 2);
        }

        private void StartNewRound()
        {
            Debug.Log("GameManager::StartNewRound(): Round Starting...");
            
            // ToDo: Whatever else the game manager needs at round start. To be figured out.
                
            // Inform observers of the round start.
            OnRoundStartEvent?.Invoke();
        }

        private void EndRound()
        {
            Debug.Log("GameManager::EndRound(): Round Ending...");
            
            // ToDo: Whatever needs to happen on round end. To be figured out.
            
            // Inform observers of the round end.
            OnRoundEndEvent?.Invoke();
        }

        // Creates a customer to be used in the game loop. Passing relevant data and saving it to our _activeCustomer property.
        private void SpawnCustomer()
        {
            Vector3 counterLocation = customerCounterLocation.position;
            Vector3 startLocation = customerStartLocation.position;
            // Get the rotation to spawn the customer facing the counter location.
            var spawnRotation =
                Quaternion.LookRotation(counterLocation - startLocation);
            
            // Spawn the customer and save a reference.
            _activeCustomer = Instantiate(customerPrefab, startLocation, spawnRotation).GetComponent<Customer>();
            
            // Ensure this customer has the proper scene location data.
            _activeCustomer.SetWaypointLocations(startLocation, counterLocation, customerExitLocation.position);
        }

    }
}
