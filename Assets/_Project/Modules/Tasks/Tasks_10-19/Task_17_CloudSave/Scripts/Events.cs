using System;
using UnityEngine;

namespace Skillhub.Tasks.CloudSave
{
    [CreateAssetMenu(fileName = "Events", menuName = "Scriptable Objects/Events")]
    public class Events : ScriptableObject
    {
        public event Action OnGamePaused;
        public event Action OnGameResumed;

        public void InvokeGamePaused()
        {
            OnGamePaused?.Invoke();
        }

        public void InvokeGameResumed()
        {
            OnGameResumed?.Invoke();
        }
    }
}
