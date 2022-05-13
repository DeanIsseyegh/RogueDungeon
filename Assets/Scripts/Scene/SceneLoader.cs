using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Resolvers;
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
        [SerializeField] private GameObject loadingText;
        [SerializeField] private GameObject startButton;
        private AsyncOperationHandle<SceneInstance> _handle;
        private bool loaded;
        private bool unloaded;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMainGameScene()
        {
            // StartCoroutine(LoadMainGameSceneAsync());
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("ThirdPersonScene");
            startButton.SetActive(false);
            loadingText.SetActive(true);
            // loadSceneAsync.allowSceneActivation = true;
        }

        private IEnumerator LoadMainGameSceneAsync()
        {
            AsyncOperationHandle<SceneInstance> asyncOperationHandle = Addressables.LoadSceneAsync(inGameScene);
            asyncOperationHandle.Completed += SceneLoadCompleted;
            yield return StartCoroutine(ShowLoadProgress(asyncOperationHandle));
        }

        public IEnumerator ShowLoadProgress(AsyncOperationHandle<SceneInstance> handle)
        {
            startButton.SetActive(false);
            loadingText.SetActive(true);
            while (!handle.IsDone)
            {
                yield return new WaitForEndOfFrame();
            }
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
                loaded = true;
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