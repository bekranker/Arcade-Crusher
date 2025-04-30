using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeGames.CrossRoad
{
    public class PlayerStepMovement : MonoBehaviour
    {
        [Header("-----Props")]
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private Vector2 _moveClamp;
        [SerializeField] private float _jumpAmount;

        private Player_Actions _inputActions;
        private Vector2 previousInput = Vector2.zero;

        void Awake()
        {
            _inputActions = new();
        }

        void OnEnable()
        {
            _inputActions.Enable();
        }

        void Update()
        {
            Vector2 currentInput = _inputActions.Player.Move.ReadValue<Vector2>();

            // One-shot detection: went from no input to some input
            if (previousInput == Vector2.zero && currentInput != Vector2.zero)
            {
                Move(currentInput);
            }
            previousInput = currentInput;
        }

        void Move(Vector2 direction)
        {
            if (transform.position.y > _moveClamp.y || transform.position.y < _moveClamp.x)
                transform.DOMoveY(transform.position.y + direction.y * _jumpAmount, _jumpSpeed);
        }
    }
}