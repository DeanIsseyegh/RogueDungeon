using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference inGameScene;
        private AsyncOperationHandle<SceneInstance> _handle;
        private bool unloaded;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Addressables.LoadSceneAsync(inGameScene, )
        }
    }
}