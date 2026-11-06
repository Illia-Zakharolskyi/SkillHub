using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Skillhub.Tasks.CloudSave
{
    public class CloudSaveManager : MonoBehaviour
    {
        public static CloudSaveManager Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private TMP_Text _errorNotificationText;
        [SerializeField] private float _autoSaveInterval = 30f;

        [Header("Player Reference")]
        [SerializeField] private Player _player;

        private const string ValuesKey = "PlayerValues";
        private bool _isInitialized = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            if (_errorNotificationText != null)
            {
                _errorNotificationText.gameObject.SetActive(false);
            }

            await InitializeUnityServicesAsync();
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.sKey.wasPressedThisFrame)
            {
                _ = SaveDataAsync();
            }
        }

        private async Task InitializeUnityServicesAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }

                _isInitialized = true;
                Debug.Log("Cloud Save: Services succesfully initialized and player signed in.");

                await LoadDataAsync();
                _ = StartCoroutine(AutoSaveRoutine());
            }
            catch (Exception ex)
            {
                ShowError("no internet connection/signing up failed.");
                Debug.LogError($"Cloud Save Initialization Error: {ex.Message}");
            }
        }

        private IEnumerator AutoSaveRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_autoSaveInterval);
                if (_isInitialized)
                {
                    _ = SaveDataAsync();
                }
            }
        }

        public async Task SaveDataAsync()
        {
            if (!_isInitialized)
            {
                Debug.Log("Cloud Save: Not working");
                return;
            }

            try
            {
                int currentVaues = _player.GetValues();
                var data = new Dictionary<string, object>
                {
                    { ValuesKey, currentVaues }
                };

                _ = await CloudSaveService.Instance.Data.Player.SaveAsync(data);
                HideError();
                Debug.Log($"Data is succesfully saved to the cloud: Values = {currentVaues}");
            }
            catch (Exception ex)
            {
                ShowError("Save failed. Please check your internet connection.");
                Debug.LogError($"Cloud Save Error: {ex.Message}");
            }
        }

        public async Task LoadDataAsync()
{
    if (!_isInitialized)
    {
        return;
    }

    try
    {
        var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { ValuesKey });

        if (loadedData.TryGetValue(ValuesKey, out var item))
        {
            int loadedGold = 0;

            try
            {
                string json = JsonConvert.SerializeObject(loadedData);
                JObject jObj = JObject.Parse(json);

                if (jObj[ValuesKey] != null)
                {
                    var token = jObj[ValuesKey];

                    if (token is JObject obj && obj["Value"] != null)
                    {
                        loadedGold = obj["Value"].Value<int>();
                    }
                    else
                    {
                        loadedGold = token.Value<int>();
                    }
                }
            }
            catch (Exception parseEx)
            {
                Debug.LogError($"Failed to parse JSON value: {parseEx.Message}");
            }

            if (_player != null)
            {
                _player.SetValues(loadedGold);
            }

            Debug.Log($"Data is successfully loaded: Values = {loadedGold}");
        }
        else
        {
            Debug.Log("Cloud Save: Key not found in the cloud, leaving local data.");
        }
        HideError();
    }
    catch (Exception ex)
    {
        ShowError("Failed to load data from the cloud.");
        Debug.LogError($"Cloud Load Error: {ex.Message}");
    }
}

        private void ShowError(string message)
        {
            if (_errorNotificationText != null)
            {
                _errorNotificationText.text = message;
                _errorNotificationText.gameObject.SetActive(true);
            }
        }

        private void HideError()
        {
            if (_errorNotificationText != null && _errorNotificationText.gameObject.activeSelf)
            {
                _errorNotificationText.gameObject.SetActive(false);
            }
        }
    }
}