using System;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        /*[SerializeField] private LayerCheck _groundCheck;*/

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;

        private static readonly int s_isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int s_isRunning = Animator.StringToHash("is-running");
        private static readonly int s_verticalVelocity = Animator.StringToHash("vertical-velocity");

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

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

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

            _animator.SetBool(s_isGroundKey, isGrounded);
            _animator.SetBool(s_isRunning, _direction.x != 0);
            _animator.SetFloat(s_verticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
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

        /*private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }*/

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
    }
}