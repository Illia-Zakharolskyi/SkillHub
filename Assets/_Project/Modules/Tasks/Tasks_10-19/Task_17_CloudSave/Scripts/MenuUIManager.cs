using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Skillhub.Tasks.CloudSave
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mainMixer;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private string _sceneName;

        private void Start()
        {
            float savedMasterVolume = PlayerPrefs.GetFloat("MasterVol", 0.8f);
            float masterDb = Mathf.Log10(Mathf.Max(0.0001f, savedMasterVolume)) * 20;

            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVol", 0.4f);
            float musicDb = Mathf.Log10(Mathf.Max(0.0001f, savedMusicVolume)) * 20;

            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVol", 0.6f);
            float sfxDb = Mathf.Log10(Mathf.Max(0.0001f, savedSFXVolume)) * 20;

            _ = _mainMixer.SetFloat("MasterVol", masterDb);
            _ = _mainMixer.SetFloat("MusicVol", musicDb);
            _ = _mainMixer.SetFloat("SFXVol", sfxDb);
        }


        public void OpenMain()
        {
            _mainPanel.SetActive(true);
            _settingsPanel.SetActive(false);
        }

        public void OpenSettings()
        {
            _mainPanel.SetActive(false);
            _settingsPanel.SetActive(true);
        }

        public void Play()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
