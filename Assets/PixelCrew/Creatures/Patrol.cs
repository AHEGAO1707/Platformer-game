using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}