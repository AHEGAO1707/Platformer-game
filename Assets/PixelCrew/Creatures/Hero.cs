using PixelCrew.Components;
using PixelCrew.Components.Utils;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _damageVVelocity;
        [SerializeField] private LayerCheck _wallCheck;
    
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;


        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private SpawnComponent _swordHitParticles;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        [SerializeField] private CheckCircleOverlap _interactionCheck;
        
        private bool _allowDoubleJump;
        private bool _isOnWall;

        private GameSession _session;
        private float _defaultGravityScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        protected override void Update()
        {
            base.Update();

            if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        protected override float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
           if (!IsGrounded && _allowDoubleJump)
            {
                Particles.Spawn("Jump");
                _allowDoubleJump = false;
                return JumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);   
        }
         
        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} монет добавлено. Всего монет: {_session.Data.Coins}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0 )
            {
                SpawnCoins();
            }
        }
         
        private void SpawnCoins()
        {
            var numCoinsToDispose = Math.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void BecomeInvisible(float time)
        {
            StartCoroutine(WaitInvasibilityCoroutine(time));
        }

        IEnumerator WaitInvasibilityCoroutine(float time)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSecondsRealtime(time);
            for (int i = 0; i < 5; i++)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSecondsRealtime(0.4f);
                GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSecondsRealtime(0.4f);
            }
            GetComponent<SpriteRenderer>().enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    Particles.Spawn("SlamDown");                  
                }
                if (contact.relativeVelocity.y >= _damageVVelocity)
                {
                    GetComponent<HealthComponent>().ModifyHealth(-1);
                }
            }
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            
            base.Attack();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            _session.Data.SwordsAmount += 1;
            UpdateHeroWeapon();
            Animator.runtimeAnimatorController = _armed;
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }

        public void OnDoThrow()
        {
            Particles.Spawn("Throw");
        }

        public void Throw()
        {
            if (!_session.Data.IsArmed)
            {
                Debug.Log("У меня нет меча чтобы им кидаться!");
            }
            else
            {
                if (_session.Data.SwordsAmount > 1)
                {
                    if (_throwCooldown.IsReady)
                    {
                        _session.Data.SwordsAmount -= 1;
                        Animator.SetTrigger(ThrowKey);
                        _throwCooldown.Reset();
                    }
                }
                else
                {
                    Debug.Log("У меня остался последний меч! Не надо его выкидывать!");
                }
            }
        }
    }
}