using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace TaskAsync
{
    public class ButtonRickroll : MonoBehaviour
    {
        private const string RickrollUrl =
        "https://www.youtube.com/watch?v=dQw4w9WgXcQ";


        public async void Roll()
        {
            Debug.Log("Start asynchronous downloading");

            string result = await LoadPageAsync(RickrollUrl);

            if (result != null)
            {
                Debug.Log("Downloaded.");

                Application.OpenURL(RickrollUrl);
            }
        }

        private async Task<string> LoadPageAsync(string url)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);

            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                Debug.Log($"Downloading: {operation.progress * 100:F0}%");

                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
                return null;
            }

            return request.downloadHandler.text;
        }

    }
}
