using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    [RequireComponent(typeof(Collider2D))]
    public class BlackHole : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Planet"))
            {
                var planet = other.GetComponent<Planet>();
                if (planet != null)
                {
                    planet.Die();
                }
            }
        }

        public void OnTap()
        {
            GameController.Instance.Pause();
        }
    }
}