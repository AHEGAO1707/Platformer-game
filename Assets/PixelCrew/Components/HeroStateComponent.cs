using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class HeroStateComponent : MonoBehaviour
    {
        [SerializeField] private HeroState state;
        [SerializeField] private float InvisabilityTime;
        private Creatures.Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Creatures.Hero>();
        }

        public void ActivateState()
        {
            if (state == HeroState.INVISABILITY)
            {
                _hero.BecomeInvisible(InvisabilityTime);
            }
        }
    }

    public enum HeroState
    {
        INVISABILITY
    }
}
