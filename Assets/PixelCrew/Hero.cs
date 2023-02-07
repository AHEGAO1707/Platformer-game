using PixelCrew.Components;
using System;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        private Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private bool _isGrounded;
        private bool _allowDoubleJump;

        private static readonly int s_isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int s_isRunning = Animator.StringToHash("is-running");
        private static readonly int s_verticalVelocity = Animator.StringToHash("vertical-velocity");

        private static readonly int Hit = Animator.StringToHash("hit");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>(); 
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void Update()
        {
            _isGrounded = IsGrounded(); 
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();
            if (isJumping)
            {
                if (isGrounded && _rigidbody.velocity.y <= 0)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
            } 
            // Короткий прыжок
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }

            _animator.SetBool(s_isGroundKey, _isGrounded);
            _animator.SetBool(s_isRunning, _direction.x != 0);
            _animator.SetFloat(s_verticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;
            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
            } else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
            }
            return yVelocity;   
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
            }
        }

        private bool IsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }

        public void SaySomething() 
        {
            Debug.Log("Something!");
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

    }
}