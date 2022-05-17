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
        [SerializeField] private string inGameSceneName;
        [SerializeField] private AssetReference startScreenScene;
        [SerializeField] private string startScreenSceneName;
        [SerializeField] private GameObject loadingText;
        [SerializeField] private GameObject startInstructions;
        private AsyncOperationHandle<SceneInstance> _handle;
        private bool isLoading;

        public void LoadMainGameScene()
        {
            if (isLoading) return;
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(inGameSceneName);
            startInstructions.SetActive(false);
            loadingText.SetActive(true);
            isLoading = true;
        }

        public void LoadStartScreenScene()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(startScreenSceneName);
        }

    }
}