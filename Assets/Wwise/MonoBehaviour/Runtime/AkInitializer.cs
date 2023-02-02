#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/*******************************************************************************
The content of this file includes portions of the proprietary AUDIOKINETIC Wwise
Technology released in source code form as part of the game integration package.
The content of this file may not be used without valid licenses to the
AUDIOKINETIC Wwise Technology.
Note that the use of the game engine is subject to the Unity(R) Terms of
Service at https://unity3d.com/legal/terms-of-service
 
License Usage
 
Licensees holding valid licenses to the AUDIOKINETIC Wwise Technology may use
this file in accordance with the end user license agreement provided with the
software or, alternatively, in accordance with the terms contained
in a written agreement between you and Audiokinetic Inc.
Copyright (c) 2022 Audiokinetic Inc.
*******************************************************************************/

#if AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES
using AK.Wwise.Unity.WwiseAddressables;
#endif

[UnityEngine.AddComponentMenu("Wwise/AkInitializer")]
[UnityEngine.DisallowMultipleComponent]
[UnityEngine.ExecuteInEditMode]
[UnityEngine.DefaultExecutionOrder(-100)]
/// @brief This script deals with initialization, and frame updates of the Wwise audio engine.  
/// It is marked as \c DontDestroyOnLoad so it stays active for the life of the game, 
/// not only one scene. Double-click the Initialization Settings entry, AkWwiseInitializationSettings, 
/// to review and edit Wwise initialization settings.
/// \sa
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=workingwithsdks__initialization.html" target="_blank">Initialize the Different Modules of the Sound Engine</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=namespace_a_k_1_1_sound_engine_a27257629833b9481dcfdf5e793d9d037.html#a27257629833b9481dcfdf5e793d9d037" target="_blank">AK::SoundEngine::Init()</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=namespace_a_k_1_1_sound_engine_a9176602bbe972da4acc1f8ebdb37f2bf.html#a9176602bbe972da4acc1f8ebdb37f2bf" target="_blank">AK::SoundEngine::Term()</a> (Note: This is described in the Wwise SDK documentation.)
/// - AkCallbackManager
public class AkInitializer : UnityEngine.MonoBehaviour
{
	private static AkInitializer ms_Instance;
#if AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES
	public AkWwiseAddressablesInitializationSettings InitializationSettings;
#else
	public AkWwiseInitializationSettings InitializationSettings;
#endif

	private void Awake()
	{
#if UNITY_EDITOR
		if (UnityEditor.BuildPipeline.isBuildingPlayer)
			return;
#endif

		if (ms_Instance)
		{
			DestroyImmediate(this);
			return;
		}

		ms_Instance = this;

#if UNITY_EDITOR
		UnityEditor.EditorApplication.quitting += OnApplicationQuit;

		if (!UnityEditor.EditorApplication.isPlaying)
			return;

		#if !(AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES)
				AkWwiseFileWatcher.Instance.XMLUpdated += AkBankManager.ReloadAllBanks;
		#endif
#endif

		DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
#if UNITY_EDITOR
		if (UnityEditor.BuildPipeline.isBuildingPlayer)
			return;
#endif

#if AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES
		InitializationSettings = AkWwiseAddressablesInitializationSettings.Instance;
#else
		InitializationSettings = AkWwiseInitializationSettings.Instance;
#endif

		if (ms_Instance == this)
		{
			AkSoundEngineController.Instance.Init(this);
		}
	}

	private void OnDisable()
	{
#if UNITY_EDITOR
		if (UnityEditor.BuildPipeline.isBuildingPlayer)
			return;
#endif

		if (ms_Instance == this)
			AkSoundEngineController.Instance.OnDisable();
	}

	private void OnDestroy()
	{
#if UNITY_EDITOR
		if (UnityEditor.BuildPipeline.isBuildingPlayer)
			return;
#endif

		if (ms_Instance == this)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.quitting -= OnApplicationQuit;
#endif
			ms_Instance = null;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (ms_Instance == this)
			AkSoundEngineController.Instance.OnApplicationPause(pauseStatus);
	}

	private void OnApplicationFocus(bool focus)
	{
		if (ms_Instance == this)
			AkSoundEngineController.Instance.OnApplicationFocus(focus);
	}

	private void OnApplicationQuit()
	{
		if (ms_Instance == this)
			AkSoundEngineController.Instance.Terminate();
	}

	//Use LateUpdate instead of Update() to ensure all gameobjects positions, listener positions, environements, RTPC, etc are set before finishing the audio frame.
	private void LateUpdate()
	{
		if (ms_Instance == this)
			AkSoundEngineController.Instance.LateUpdate();
	}

#region WwiseMigration
#if UNITY_EDITOR
#pragma warning disable 0414 // private field assigned but not used.

	// previously serialized data that will be consumed by migration
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private string basePath = string.Empty;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private string language = string.Empty;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int defaultPoolSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int lowerPoolSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int streamingPoolSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private float memoryCutoffThreshold = 0f;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int monitorPoolSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int monitorQueuePoolSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int callbackManagerBufferSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private int spatialAudioPoolSize = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private uint maxSoundPropagationDepth = 0;
	[UnityEngine.HideInInspector][UnityEngine.SerializeField] private bool engineLogging = false;

#pragma warning restore 0414 // private field assigned but not used.

	private class Migration15Data
	{
		bool hasMigrated = false;

		public void Migrate(AkInitializer akInitializer)
		{
			if (hasMigrated)
				return;

			var initializationSettings = akInitializer.InitializationSettings;
			if (!initializationSettings)
			{
#if AK_WWISE_ADDRESSABLES && UNITY_ADDRESSABLES
				initializationSettings = AkWwiseAddressablesInitializationSettings.Instance;
#else
				initializationSettings = AkWwiseInitializationSettings.Instance;
#endif
				if (!initializationSettings)
					return;
			}

			initializationSettings.UserSettings.m_BasePath = akInitializer.basePath;
			initializationSettings.UserSettings.m_StartupLanguage = akInitializer.language;

			initializationSettings.AdvancedSettings.m_MonitorQueuePoolSize = (uint)akInitializer.monitorQueuePoolSize * 1024;

			initializationSettings.UserSettings.m_SpatialAudioSettings.m_MaxSoundPropagationDepth = akInitializer.maxSoundPropagationDepth;

			initializationSettings.CallbackManagerInitializationSettings.IsLoggingEnabled = akInitializer.engineLogging;

			UnityEditor.EditorUtility.SetDirty(initializationSettings);
			UnityEditor.AssetDatabase.SaveAssets();

			UnityEngine.Debug.Log("WwiseUnity: Converted from AkInitializer to AkWwiseInitializationSettings.");
			hasMigrated = true;
		}
	}

	private static Migration15Data migration15data;

	public static void PreMigration15()
	{
		migration15data = new Migration15Data();
	}

	public void Migrate15()
	{
		UnityEngine.Debug.Log("WwiseUnity: AkInitializer.Migrate15 for " + gameObject.name);

		if (migration15data != null)
			migration15data.Migrate(this);
	}

	public static void PostMigration15()
	{
		migration15data = null;
	}
#endif
#endregion
			}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.