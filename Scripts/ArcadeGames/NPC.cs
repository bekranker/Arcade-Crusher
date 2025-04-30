using UnityEngine;

namespace ArcadeGames.CrossRoad
{
    public abstract class NPC : MonoBehaviour, IDamage
    {
        // Health of the enemy
        public float Health;

        // Speed of the enemy
        public float Speed;

        // Method to handle enemy movement
        public abstract void Move();

        // Method to handle enemy attack
        public abstract void Attack();

        // Method to handle enemy taking damage
        public virtual void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        // Method to handle enemy death
        public virtual void Die()
        {
            // Default behavior for enemy death
            Destroy(gameObject);
        }
    }
}