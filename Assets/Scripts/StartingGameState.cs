using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class StartingGameState : MonoBehaviour
    {
        public GameStateManager.eGameState startingGameState;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}