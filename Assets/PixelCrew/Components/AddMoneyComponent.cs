using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class AddMoneyComponent : MonoBehaviour
    {
        public static int smoney;
        [SerializeField] private GameObject _coinToAdd;
        public void AddMoney()
        {
            if (_coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_0")
            {
                smoney++;
            }
            else if (_coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_4")
            {
                smoney += 10;
            }
            Debug.Log("Ваши деньги: " + smoney);
        }
    }
}
