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

    // Struct used for handling player score in the game.
    [System.Serializable] // ToDo/Note: If Serializable I can [SerializeField] an instance of this (say in GameManager) and monitor it. Not really necessary but I want to see values so leaving for now.
    public struct GameScore
    {
        // ToDo/Question/Note: I am wasting time making this. I don't even think I will ever use it when the individuals exist. At least learned something I guess.
        /* Informs observers of a CHANGE in game score. This is different from the other property events as this one
         * ONLY invokes when a change is made on the value. */
        public event Action<GameScore> OnGameScoreChanged; // Informs observers of a change in game score. 
        
        private void SetProperty<T>(ref T backingField, T newValue) where T : IEquatable<T>
        {
            // Check if the value being set to BackingField is different from its current value.
            bool newValueIsDifferent = !backingField.Equals(newValue);

            // Update the backing field.
            backingField = newValue;

            // If the value was different, inform observers. Note: This is more expensive so we call only on change.
            if (newValueIsDifferent)
                OnGameScoreChanged?.Invoke(this);
        }
        
        
        private HandleSize _handleSize; // (Backing Field) The handle size of the weapon. // ToDo/Question: Should the handle size give a score, or work more as a multiplier because it correlates to difficulty.
        public event Action<HandleSize> OnHandleSizeSet; // Informs observers of _handleSize's value after setting.
        public HandleSize HandleSize
        {
            get => _handleSize;

            set
            {
                _handleSize = value; // Assign value to the backing field.
                OnHandleSizeSet?.Invoke(_handleSize); // Inform observers.
            }
        }
        
        private bool _isCorrectWeaponType;  // (Backing Field) Whether or not the weapon made was the same as the customer's request.
        public event Action<bool> OnIsCorrectWeaponTypeSet; // Informs observers of _isCorrectWeaponType's value after setting.
        public bool IsCorrectWeaponType // Whether or not the weapon made was the same as the customer's request.
        {
            get => _isCorrectWeaponType;

            set
            {
                _isCorrectWeaponType = value; // Assign value to the backing field.
                OnIsCorrectWeaponTypeSet?.Invoke(_isCorrectWeaponType);
            }
        }

        // ToDo/Question: Should I have buildTime or final build time? I feel like Final build time best way game manager can pass time when finished, doesn't need to continously update here right.
        private float _finalBuildTime; // (Backing Field) The time it took to build the weapon.
        public event Action<float> OnFinalBuildTimeSet; // Informs observers of _finalBuildTime's value after setting.

        public float FinalBuildTime // The time it took to build the weapon.
        {
            get => _finalBuildTime;

            set
            {
                _finalBuildTime = value; // Assign value to the backing field.
                OnFinalBuildTimeSet?.Invoke(_finalBuildTime); // Inform observers.
            }
        }

        private int _weaponDurability; // (Backing Field) The weapon's final durability after turning in.
        public event Action<int> OnWeaponDurabilitySet; // Informs observers of _weaponDurability's value after setting.
        public int WeaponDurability // The weapon' s final durability after turning in.
        {
            get => _weaponDurability;

            set
            {
                _weaponDurability = value; // Assign value to the backing field.
                OnWeaponDurabilitySet?.Invoke(_weaponDurability); // Notify observers.
            }
        }

    }
    
    // An enum representing the ranks for player's score.
    public enum ScoreRankings
    {
        S,  // Highest (95% up) (iunno how score looks yet so 95% means very little of course).
        A,
        B,
        C,
        D
    }
    
    
    
    
}
