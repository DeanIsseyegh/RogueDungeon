using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference inGameScene;
        [SerializeField] private AssetReference startScreenScene;
        private AsyncOperationHandle<SceneInstance> _handle;
        private bool unloaded;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMainGameScene()
        {
            Addressables.LoadSceneAsync(inGameScene, LoadSceneMode.Single)
                .Completed += SceneLoadCompleted;
        }
        
        public void LoadMainGameSceneStandard()
        {
            SceneManager.LoadScene("ThirdPersonScene");
        }

        private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Successfully loaded scene");
                _handle = obj;
            }
        }

        void UnloadScene()
        {
            Addressables.UnloadSceneAsync(_handle, true).Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                    Debug.Log("Successfully unloaded scene");
            };
        }
    }
}