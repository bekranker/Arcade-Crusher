using UnityEngine;

namespace ArcadeGames.CrossRoad
{
    public abstract class Enemy : MonoBehaviour
    {
        // Health of the enemy
        public int Health { get; protected set; }

        // Speed of the enemy
        public float Speed { get; protected set; }

        // Method to handle enemy movement
        public abstract void Move();

        // Method to handle enemy attack
        public abstract void Attack();

        // Method to handle enemy taking damage
        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        // Method to handle enemy death
        protected virtual void Die()
        {
            // Default behavior for enemy death
            Destroy(gameObject);
        }
    }
}