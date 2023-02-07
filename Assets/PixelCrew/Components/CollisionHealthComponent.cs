using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CollisionHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _heal;

        public void ApplyDamage(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyDamage(_damage);
            }
        }

        public void ApplyHeal(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyHeal(_heal);
            }
        }
    }
}
           