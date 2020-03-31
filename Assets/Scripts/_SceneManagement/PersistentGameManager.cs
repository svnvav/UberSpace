using System;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class PersistentGameManager : MonoBehaviour
    {
        public static PersistentGameManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ContinueGame()
        {
            
        }

        public void StartLevel(int index)
        {
            
        }
    }
}