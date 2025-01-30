using System;
using UnityEngine;

namespace PSoft
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;
        
        public event Action<int> OnMaxHealthChanged;        // Invoked when maxHealth changes, passing the new value
        public event Action<int> OnCurrentHealthChanged;    // Invoked when currentHealth changes, passing the new value.
        
        public int GetMaxHealth => maxHealth;
        public int GetCurrentHealth => currentHealth;

        public void SetMaxHealth(int newMaxHealth)
        {
            var bValueChanged = (maxHealth != newMaxHealth);
            
            maxHealth = newMaxHealth;
            
            // Inform observers after the change.
            if(bValueChanged)
                OnMaxHealthChanged?.Invoke(maxHealth);
        }
        
        // Decrement current health by the given amount (clamped 0-maxHealth). Invokes OnCurrentHealthSet on a change.
        public void TakeDamage(int damageAmount)
        {
            var previousHealth = currentHealth;
            
            currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);

            if(previousHealth != currentHealth)
                OnCurrentHealthChanged?.Invoke(currentHealth);
        }

        // Increment current health by the given amount (clamped 0-maxHealth). Invokes OnCurrentHealthSet on a change.
        public void HealDamage(int healAmount)
        {
            var previousHealth = currentHealth;
            
            currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
            
            if(previousHealth != currentHealth)
                OnCurrentHealthChanged?.Invoke(currentHealth);
        }
        
        private void Awake()
        {
            // We set in Awake() instead of Start() to set it before any other script references it (e.g. UI).
            currentHealth = maxHealth;
        }
        
    }
}
