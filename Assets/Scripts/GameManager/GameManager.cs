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

        [SerializeField] private GameScore activeGameScore; // Score for the current game. ToDo: weird wording fr.

        //* Delegates/Events/Actions *//
        public event Action OnRoundStartEvent;  // Invoked when a new round is starting.
        public event Action OnRoundEndEvent;    // Invoked when a customer has received their request weapon.

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // Bind to respond to game score event changes.
            activeGameScore.OnHandleSizeSet += OnGameScoreUpdated;
            activeGameScore.OnIsCorrectWeaponTypeSet += OnGameScoreUpdated;
            activeGameScore.OnFinalBuildTimeSet += OnGameScoreUpdated;
            activeGameScore.OnWeaponDurabilitySet += OnGameScoreUpdated;

            activeGameScore.WeaponDurability = 69420;

            // Create our customer to be used. ToDo/Question: Should I create here, or just place in scene?
            //SpawnCustomer(); ToDo: I added a spawn delay to see if it was causing an issue I was having. I can't remember if this fixed it so I am leaving for now. Will come back before merge (LOL).
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

        // A callback function for events in our GameScore. Receives the GameScoreProperty and it's value.
        private void OnGameScoreUpdated<T>(T value, GameScoreProperty scoreProperty)
        {
            //* Note: Currently this function will not run when a change is made to our _activeGameScore in the inspector during runtime. *//
            
            Debug.Log($"GameManager::OnGameScoreUpdate(): {scoreProperty}'s new value is {value}");

            switch (scoreProperty)
            {
                case GameScoreProperty.None:
                    Debug.Log("GameManager::OnGameScoreUpdate() scoreProperty is None.");
                    break;
                case GameScoreProperty.HandleSize:
                    // ToDo: Response/React.
                    break;
                case GameScoreProperty.IsCorrectWeaponType:
                    // ToDo: Response/React.
                    break;
                case GameScoreProperty.FinalBuildTime:
                    // ToDo: Response/React.
                    break;
                case GameScoreProperty.WeaponDurability:
                    Debug.Log($"Weapon Durability Update : {value}");
                    // ToDo: Update health bar probably.
                    break;
                default:
                    Debug.Log("GameManager::OnGameScoreUpdate() scoreProperty response not found.");
                    break;
            }
        }
        
    }
    
    // A struct used for handling player score in the game.
    [System.Serializable]
    public struct GameScore
    {
        // Note: These are invoked in their associated setters, even if the value is not changed. *//
        public event Action<HandleSize, GameScoreProperty> OnHandleSizeSet;
        public event Action<bool, GameScoreProperty> OnIsCorrectWeaponTypeSet;
        public event Action<float, GameScoreProperty> OnFinalBuildTimeSet;
        public event Action<int, GameScoreProperty> OnWeaponDurabilitySet;
        
        [SerializeField] private HandleSize handleSize;     // (Backing Field) The handle size of the weapon. // ToDo/Question: Should the handle size give a score, or work more as a multiplier because it correlates to difficulty.
        [SerializeField] private bool isCorrectWeaponType;  // (Backing Field) Whether or not the weapon made was the same as the customer's request.
        [SerializeField] private float finalBuildTime;      // (Backing Field) The time it took to build the weapon.
        [SerializeField] private int weaponDurability;      // (Backing Field) The weapon's final durability after turning in.
        // ToDo: Score Ranking[SerializeField] private ScoreRanking _scoreRanking; // ToDo: Get/Set/Calulating and all that. I just want to test, do I need to make public things also [SeriliazeField] to show (like UPROPERTY and USTRUCT)? probably not but just double  checkk.

        public HandleSize HandleSize
        {
            get => handleSize;

            set
            {
                handleSize = value; // Assign value to the backing field.
                OnHandleSizeSet?.Invoke(handleSize, GameScoreProperty.HandleSize); // Inform observers.
            }
        }

        public bool IsCorrectWeaponType // Whether or not the weapon made was the same as the customer's request.
        {
            get => isCorrectWeaponType;

            set
            {
                isCorrectWeaponType = value; // Assign value to the backing field.
                OnIsCorrectWeaponTypeSet?.Invoke(isCorrectWeaponType, GameScoreProperty.IsCorrectWeaponType);
            }
        }

        // ToDo/Question: Should I have buildTime or final build time? I feel like Final build time best way game manager can pass time when finished, doesn't need to continously update here right.
        public float FinalBuildTime // The time it took to build the weapon.
        {
            get => finalBuildTime;

            set
            {
                finalBuildTime = value; // Assign value to the backing field.
                OnFinalBuildTimeSet?.Invoke(finalBuildTime, GameScoreProperty.FinalBuildTime); // Inform observers.
            }
        }

        public int WeaponDurability // The weapon's final durability after turning in.
        {
            get => weaponDurability;

            set
            {
                weaponDurability = value; // Assign value to the backing field.
                OnWeaponDurabilitySet?.Invoke(weaponDurability, GameScoreProperty.WeaponDurability); // Notify observers.
            }
        } 
    }

    // An enum representing the members of a GameScore.
    public enum GameScoreProperty
    {
        None,
        HandleSize,
        IsCorrectWeaponType,
        FinalBuildTime,
        WeaponDurability
    }

    // An enum representing the possible ranks for a score.
    public enum ScoreRanking
    {
        Unfinished,
        S,  // Highest (95% up) (iunno how score looks yet so 95% means very little of course).
        A,
        B,
        C,
        D
    }

}
