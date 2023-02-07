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
            if (_coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_0" || _coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_1" || _coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_2" || _coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_3")
            {
                smoney++;
            }
            else if (_coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_4" || _coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_5" || _coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_6" || _coinToAdd.GetComponent<SpriteRenderer>().sprite.name == "Coins_7")
            {
                smoney += 10;
            }
            Debug.Log("Ваши деньги: " + smoney);
        }
    }
}
