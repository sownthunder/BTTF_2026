using UnityEngine;

namespace QuanticBrains.Core.Scripts.Agent.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField]
        private SettingsContainer _settingsContainer;
        private void Awake()
        {
            _settingsContainer.Initialize(this);
        }
    }
}
