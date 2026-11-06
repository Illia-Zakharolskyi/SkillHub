using UnityEngine;
using UnityEngine.SceneManagement;

namespace Skillhub.Tasks.CloudSave
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private string _menuSceneName;


        public void OnPause()
        {
            Time.timeScale = 0f;
            _gamePanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _pausePanel.SetActive(true);
        }

        public void OnResume()
        {
            Time.timeScale = 1f;
            _pausePanel.SetActive(false);
            _gamePanel.SetActive(true);
        }

        public void OnSettings()
        {
            _settingsPanel.SetActive(true);
            _pausePanel.SetActive(false);
        }

        public void OnExit()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_menuSceneName);
        }
    }
}