using System.Collections.Generic;
using ArcadeCrusher.Player;
using UnityEngine;

namespace ArcadeGames.CrossRoad
{
    public class CrossRoad_Enemy : NPC
    {
        public Vector2 DirectionToGo;
        private CrossRoad_EnemyType _selectedEnemyType;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private float _intervalCounter;
        public PlayerShoot @PlayerShoot { get; set; }

        public void Init(CrossRoad_WaveType crossRoadWave, PlayerShoot playerShoot)
        {
            _selectedEnemyType = crossRoadWave.GetRandomEnemyType();
            @PlayerShoot = playerShoot;
            Speed = _selectedEnemyType.Speed;
            Health = _selectedEnemyType.Health;
            _spriteRenderer.sprite = _selectedEnemyType.EnemySprite;
            _intervalCounter = _selectedEnemyType.IntervalEvent_TimeSpan;
            _selectedEnemyType.OnStart_EnemyEvent?.Execute(gameObject);
        }
        public override void Die()
        {
            PlayerShoot.IncreaseBulletCount(999);
            GeneralScoreHandler.Instance.IncreaseScore(_selectedEnemyType.ScoreAmount);
            _selectedEnemyType.OnDead_EnemyEvent?.Execute(gameObject);
            base.Die();
        }
        void Update()
        {
            Move();
            IntervalEvent();
        }
        public override void Move()
        {
            transform.position += new Vector3(DirectionToGo.x, 0, 0) * Speed * Time.deltaTime;
        }
        public override void TakeDamage(float damage)
        {
            _selectedEnemyType.OnHit_EnemyEvent?.Execute(gameObject);
            base.TakeDamage(damage);
        }
        public override void Attack()
        {
            print("Attack Phase");
        }
        private void IntervalEvent()
        {
            if (_intervalCounter > 0)
            {
                _intervalCounter -= Time.deltaTime;
            }
            else
            {
                _selectedEnemyType.OnInterval_EnemyEvent?.Execute(gameObject);
                _intervalCounter = _selectedEnemyType.IntervalEvent_TimeSpan;
            }
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                player.TakeDamage(999);
                Destroy(gameObject);
            }
            if (collision.CompareTag("Finish"))
            {
                Destroy(gameObject);
            }
        }
    }
}