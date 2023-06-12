using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class AddMoneyComponent : MonoBehaviour
    {
        [SerializeField] private int _numCoins;
        private Creatures.Hero _hero;

        private void Start ()
        {
            _hero = FindObjectOfType<Creatures.Hero>();
        }

        public void Add()
        {
            _hero.AddCoins(_numCoins);
        }
    }
}
