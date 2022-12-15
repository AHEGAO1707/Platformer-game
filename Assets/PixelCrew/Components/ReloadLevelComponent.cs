using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    { 
        public void Reload()
        {
            Debug.Log("Вы погибли и потеряли " + AddMoneyComponent.smoney + " денег");
            AddMoneyComponent.smoney = 0;
            Debug.Log("Reloading scene");
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
