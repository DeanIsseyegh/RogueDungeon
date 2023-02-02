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

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

[UnityEditor.InitializeOnLoad]
public class AkPluginActivator
{
	public const string ALL_PLATFORMS = "All";
	public const string CONFIG_DEBUG = "Debug";
	public const string CONFIG_PROFILE = "Profile";
	public const string CONFIG_RELEASE = "Release";

	private const string EditorConfiguration = CONFIG_PROFILE;

	private const string MENU_PATH = "Assets/Wwise/Activate Plugins/";
	private const UnityEditor.BuildTarget INVALID_BUILD_TARGET = (UnityEditor.BuildTarget)(-1);

	private const string WwisePluginFolder = "Runtime/Plugins";

	// The following check is required until "BuildTarget.Switch" is available on all versions of Unity that we support.
	private static readonly UnityEditor.BuildTarget SwitchBuildTarget = GetPlatformBuildTarget("Switch");

	private static readonly System.Collections.Generic.Dictionary<string, System.DateTime> s_LastParsed =
		new System.Collections.Generic.Dictionary<string, System.DateTime>();

	private static readonly System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<AkPluginInfo>>
		s_PerPlatformPlugins = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<AkPluginInfo>>();

	private static readonly System.Collections.Generic.HashSet<PluginID> builtInPluginIDs =
		new System.Collections.Generic.HashSet<PluginID>
		{
			PluginID.AkCompressor,
			PluginID.AkDelay,
			PluginID.AkExpander,
			PluginID.AkGain,
			PluginID.AkMatrixReverb,
			PluginID.AkMeter,
			PluginID.AkParametricEQ,
			PluginID.AkPeakLimiter,
			PluginID.AkRoomVerb,
#if !UNITY_2018_3_OR_NEWER
			PluginID.VitaReverb,
			PluginID.VitaCompressor,
			PluginID.VitaDelay,
			PluginID.VitaDistortion,
			PluginID.VitaEQ,
#endif
		};

	private static readonly System.Collections.Generic.HashSet<PluginID> alwaysSkipPluginsIDs =
		new System.Collections.Generic.HashSet<PluginID>
		{
			PluginID.SineGenerator,
			PluginID.SinkAuxiliary,
			PluginID.SinkCommunication,
			PluginID.SinkControllerHeadphones,
			PluginID.SinkControllerSpeaker,
			PluginID.SinkDVRByPass,
			PluginID.SinkNoOutput,
			PluginID.SinkSystem,
			PluginID.ToneGenerator,
			PluginID.WwiseSilence,
			PluginID.AkAudioInput,
		};

	private static readonly System.Collections.Generic.Dictionary<PluginID, string> PluginIDToStaticLibName =
		new System.Collections.Generic.Dictionary<PluginID, string>
		{
			{ PluginID.Ak3DAudioBedMixer, "Ak3DAudioBedMixerFX" },
			{ PluginID.AkAudioInput, "AkAudioInputSource" },
			{ PluginID.AkCompressor, "AkCompressorFX" },
			{ PluginID.AkRouterMixer, "AkRouterMixerFX" },
			{ PluginID.AkConvolutionReverb, "AkConvolutionReverbFX" },
			{ PluginID.AkDelay, "AkDelayFX" },
			{ PluginID.AkExpander, "AkExpanderFX" },
			{ PluginID.AkFlanger, "AkFlangerFX" },
			{ PluginID.AkGain, "AkGainFX" },
			{ PluginID.AkGuitarDistortion, "AkGuitarDistortionFX" },
			{ PluginID.AkHarmonizer, "AkHarmonizerFX" },
			{ PluginID.AkMatrixReverb, "AkMatrixReverbFX" },
			{ PluginID.AkMeter, "AkMeterFX" },
			{ PluginID.AkMotionSink, "AkMotionSink" },
			{ PluginID.AkMotionSource, "AkMotionSourceSource" },
			{ PluginID.AkMotionGeneratorSource, "AkMotionGeneratorSource" },
			{ PluginID.AkParametricEQ, "AkParametricEQFX" },
			{ PluginID.AkPeakLimiter, "AkPeakLimiterFX" },
			{ PluginID.AkPitchShifter, "AkPitchShifterFX" },
			{ PluginID.AkRecorder, "AkRecorderFX" },
			{ PluginID.AkReflect, "AkReflectFX" },
			{ PluginID.AkRoomVerb, "AkRoomVerbFX" },
			{ PluginID.AkSoundSeedGrain, "AkSoundSeedGrainSource" },
			{ PluginID.AkSoundSeedWind, "AkSoundSeedWindSource" },
			{ PluginID.AkSoundSeedWoosh, "AkSoundSeedWooshSource" },
			{ PluginID.AkStereoDelay, "AkStereoDelayFX" },
			{ PluginID.AkSynthOne, "AkSynthOneSource" },
			{ PluginID.AkTimeStretch, "AkTimeStretchFX" },
			{ PluginID.AkTremolo, "AkTremoloFX" },
			{ PluginID.AuroHeadphone, "AuroHeadphoneFX" },
			{ PluginID.CrankcaseAudioREVModelPlayer, "CrankcaseAudioREVModelPlayerSource" },
			{ PluginID.iZHybridReverb, "iZHybridReverbFX" },
			{ PluginID.iZTrashBoxModeler, "iZTrashBoxModelerFX" },
			{ PluginID.iZTrashDelay, "iZTrashDelayFX" },
			{ PluginID.iZTrashDistortion, "iZTrashDistortionFX" },
			{ PluginID.iZTrashDynamics, "iZTrashDynamicsFX" },
			{ PluginID.iZTrashFilters, "iZTrashFiltersFX" },
			{ PluginID.iZTrashMultibandDistortion, "iZTrashMultibandDistortionFX" },
			{ PluginID.MasteringSuite, "MasteringSuiteFX" },
			{ PluginID.AkImpacterSource, "AkImpacterSource" },
			{ PluginID.McDSPFutzBox, "McDSPFutzBoxFX" },
			{ PluginID.McDSPLimiter, "McDSPLimiterFX" },
			{ PluginID.ResonanceAudioRenderer, "ResonanceAudioFX" },
			{ PluginID.ResonanceAudioRoomEffect, "ResonanceAudioFX" },
			{ PluginID.IgniterLive, "IgniterLiveSource" },
			{ PluginID.IgniterLiveSynth, "IgniterLiveSource" }
		};
	
	// Support libraries are DLLs that do not have an associated Wwise plug-in ID; they are meant to be loaded manually by the application
	private static readonly System.Collections.Generic.List<string> SupportLibraries = 
		new System.Collections.Generic.List<string>
		{
			"AkVorbisHwAccelerator"
		};

	public delegate void FilterOutPlatformDelegate(UnityEditor.BuildTarget target, UnityEditor.PluginImporter pluginImporter, string pluginPlatform);
	public static FilterOutPlatformDelegate FilterOutPlatformIfNeeded = FilterOutPlatformIfNeeded_Default;

	static AkPluginActivator()
	{
		ActivatePluginsForEditor();
	}

	// Use reflection because projects that were created in Unity 4 won't have the CurrentPluginConfig field
	public static string GetCurrentConfig()
	{
		var CurrentPluginConfigField = typeof(AkWwiseProjectData).GetField("CurrentPluginConfig");
		var CurrentConfig = string.Empty;
		var data = AkWwiseProjectInfo.GetData();

		if (CurrentPluginConfigField != null && data != null)
		{
			CurrentConfig = (string)CurrentPluginConfigField.GetValue(data);
		}

		if (string.IsNullOrEmpty(CurrentConfig))
		{
			CurrentConfig = CONFIG_PROFILE;
		}

		return CurrentConfig;
	}

	private static void SetCurrentConfig(string config)
	{
		var CurrentPluginConfigField = typeof(AkWwiseProjectData).GetField("CurrentPluginConfig");
		var data = AkWwiseProjectInfo.GetData();
		if (CurrentPluginConfigField != null)
		{
			CurrentPluginConfigField.SetValue(data, config);
			UnityEngine.Debug.LogFormat("WwiseUnity: Changed plugin configuration for game runtime to {0}", config);
		}

		UnityEditor.EditorUtility.SetDirty(AkWwiseProjectInfo.GetData());
	}

	private static void ActivateConfig(string config)
	{
		SetCurrentConfig(config);
		CheckMenuItems(config);
	}

	[UnityEditor.MenuItem(MENU_PATH + CONFIG_DEBUG)]
	public static void ActivateDebug()
	{
		ActivateConfig(CONFIG_DEBUG);
	}

	[UnityEditor.MenuItem(MENU_PATH + CONFIG_PROFILE)]
	public static void ActivateProfile()
	{
		ActivateConfig(CONFIG_PROFILE);
	}

	[UnityEditor.MenuItem(MENU_PATH + CONFIG_RELEASE)]
	public static void ActivateRelease()
	{
		ActivateConfig(CONFIG_RELEASE);
	}

	private static UnityEditor.BuildTarget GetPlatformBuildTarget(string platform)
	{
		var targets = System.Enum.GetNames(typeof(UnityEditor.BuildTarget));
		var values = System.Enum.GetValues(typeof(UnityEditor.BuildTarget));

		for (var ii = 0; ii < targets.Length; ++ii)
		{
			if (platform.Equals(targets[ii]))
				return (UnityEditor.BuildTarget)values.GetValue(ii);
		}

		return INVALID_BUILD_TARGET;
	}

	public class PlatformConfiguration
	{
		public string WwisePlatformName;
		public string PluginDirectoryName;
		public ISet<string> Architectures = null;
		public string DSPDirectoryPath = "";
		public string StaticPluginRegistrationName = null;
		public string StaticPluginDefine = null;
		public bool RequiresStaticPluginRegistration = false;
	}

	public static Dictionary<UnityEditor.BuildTarget, PlatformConfiguration> BuildTargetToPlatformConfiguration = new Dictionary<UnityEditor.BuildTarget, PlatformConfiguration>();

	public static void RegisterBuildTarget(UnityEditor.BuildTarget target, PlatformConfiguration config)
	{
		BuildTargetToPlatformConfiguration.Add(target, config);
	}

	private static string[] GetPluginInfoFromPath(string path)
	{
		var indexOfPluginFolder = path.IndexOf(WwisePluginFolder, System.StringComparison.OrdinalIgnoreCase);
		if (indexOfPluginFolder == -1)
		{
			return null;
		}

		return path.Substring(indexOfPluginFolder + WwisePluginFolder.Length + 1).Split('/');
	}

	private static List<UnityEditor.PluginImporter> GetWwisePluginImporters()
	{
		UnityEditor.PluginImporter[] pluginImporters = UnityEditor.PluginImporter.GetAllImporters();
		List<UnityEditor.PluginImporter> wwisePlugins = new List<UnityEditor.PluginImporter>();
		foreach (var pluginImporter in pluginImporters)
		{
			if (pluginImporter.assetPath.Contains("Wwise/API/"))
			{
				wwisePlugins.Add(pluginImporter);
			}
		}
		return wwisePlugins;
	}

	private static void SetupStaticPluginRegistration(UnityEditor.BuildTarget target, PlatformConfiguration config)
	{
		if (!config.RequiresStaticPluginRegistration)
			return;

		string deploymentTargetName = config.WwisePlatformName;

		var staticPluginRegistration = new StaticPluginRegistration(target);
		var importers = GetWwisePluginImporters();
		foreach (var pluginImporter in importers)
		{
			var splitPath = GetPluginInfoFromPath(pluginImporter.assetPath);
			if (splitPath == null)
			{
				continue;
			}

			var pluginPlatform = splitPath[0];
			if (pluginPlatform != config.PluginDirectoryName)
				continue;

			var pluginConfig = string.Empty;

			if (config.Architectures == null || config.Architectures.Count == 0)
			{
				// No architectures, so path is /<platform>/<config>/
				pluginConfig = splitPath[1];
			}
			else
			{
				// Path is /<platform>/<arch>/<config>/
				pluginConfig = splitPath[2];

				var pluginArch = splitPath[1];
				if (!config.Architectures.Contains(pluginArch))
				{
					UnityEngine.Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
					continue;
				}
			}

			if (pluginConfig != "DSP")
				continue;

			if (!IsPluginUsed(config, pluginPlatform, System.IO.Path.GetFileNameWithoutExtension(pluginImporter.assetPath)))
				continue;

			staticPluginRegistration.TryAddLibrary(config, pluginImporter.assetPath);
		}

		System.Collections.Generic.HashSet<AkPluginInfo> plugins;
		s_PerPlatformPlugins.TryGetValue(deploymentTargetName, out plugins);
		var missingPlugins = staticPluginRegistration.GetMissingPlugins(plugins);
		if (missingPlugins.Count == 0)
		{
			if (plugins == null)
				UnityEngine.Debug.LogWarningFormat("WwiseUnity: The activated Wwise plug-ins may not be correct. Could not read PluginInfo.xml for platform: {0}", deploymentTargetName);
			
			staticPluginRegistration.TryWriteToFile(config);
		}
		else
		{
			UnityEngine.Debug.LogErrorFormat(
				"WwiseUnity: These plugins used by the Wwise project are missing from the Unity project: {0}. Please check folder Assets/Wwise/API/Runtime/Plugin/{1}.",
				string.Join(", ", missingPlugins.ToArray()), deploymentTargetName);
		}
	}

	private static void SetStandalonePlatformData(UnityEditor.PluginImporter pluginImporter, string platformName, string architecture)
	{
		var isLinux = platformName == "Linux";
		var isWindows = platformName == "Windows";
		var isMac = platformName == "Mac";
		var isX86 = architecture == "x86";
		var isX64 = architecture == "x86_64";

#if !UNITY_2019_2_OR_NEWER
		pluginImporter.SetPlatformData(UnityEditor.BuildTarget.StandaloneLinux, "CPU", isLinux && isX86 ? "x86" : "None");
		pluginImporter.SetPlatformData(UnityEditor.BuildTarget.StandaloneLinuxUniversal, "CPU", !isLinux ? "None" : isX86 ? "x86" : isX64 ? "x86_64" : "None");
#endif
		pluginImporter.SetPlatformData(UnityEditor.BuildTarget.StandaloneLinux64, "CPU", isLinux && isX64 ? "x86_64" : "None");
		pluginImporter.SetPlatformData(UnityEditor.BuildTarget.StandaloneWindows, "CPU", isWindows && isX86 ? "AnyCPU" : "None");
		pluginImporter.SetPlatformData(UnityEditor.BuildTarget.StandaloneWindows64, "CPU", isWindows && isX64 ? "AnyCPU" : "None");
		pluginImporter.SetPlatformData(UnityEditor.BuildTarget.StandaloneOSX, "CPU", isMac ? "AnyCPU" : "None");
	}

	public static void ActivatePluginsForDeployment(UnityEditor.BuildTarget target, bool Activate)
	{
		if (!BuildTargetToPlatformConfiguration.TryGetValue(target, out var platformConfiguration))
		{
			UnityEngine.Debug.Log("WwiseUnity: Build Target " + target + " not supported.");
			return;
		}

		if (Activate)
		{
			SetupStaticPluginRegistration(target, platformConfiguration);
		}

		var importers = GetWwisePluginImporters();
		foreach (var pluginImporter in importers)
		{
			var splitPath = GetPluginInfoFromPath(pluginImporter.assetPath);
			if (splitPath == null)
			{
				continue;
			}

			var pluginPlatform = splitPath[0];
			if (pluginPlatform != platformConfiguration.PluginDirectoryName)
			{
				if (Activate)
				{
					FilterOutPlatformIfNeeded(target, pluginImporter, pluginPlatform);
				}

				continue;
			}

			var pluginName = splitPath[splitPath.Length - 1].Split('.')[0];
			var pluginArch = string.Empty;
			var pluginConfig = string.Empty;
			var bIsSupportLibrary = SupportLibraries.Contains(pluginName);

			switch (pluginPlatform)
			{
				case "WebGL":
				case "iOS":
				case "tvOS":
				case "PS4":
				case "PS5":
				case "XboxOne":
				case "Stadia":
				case "XboxSeriesX":
				case "XboxOneGC":
					pluginConfig = splitPath[1];
					break;

				case "Android":
					pluginArch = splitPath[1];
					pluginConfig = splitPath[2];

					if (pluginArch == "armeabi-v7a")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.Android, "CPU", "ARMv7");
					}
					else if (pluginArch == "arm64-v8a")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.Android, "CPU", "ARM64");
					}
					else if (pluginArch == "x86")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.Android, "CPU", "x86");
					}
					else
					{
						UnityEngine.Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
					}
					break;

				case "Linux":
					pluginArch = splitPath[1];
					pluginConfig = splitPath[2];

					if (pluginArch != "x86" && pluginArch != "x86_64")
					{
						UnityEngine.Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
						continue;
					}
					SetStandalonePlatformData(pluginImporter, pluginPlatform, pluginArch);
					break;

				case "Mac":
					pluginConfig = splitPath[1];
					SetStandalonePlatformData(pluginImporter, pluginPlatform, pluginArch);
					break;

				case "WSA":
					pluginArch = splitPath[1];
					pluginConfig = splitPath[2];

					pluginImporter.SetPlatformData(UnityEditor.BuildTarget.WSAPlayer, "SDK", "AnySDK");

					if (pluginArch == "WSA_UWP_Win32")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.WSAPlayer, "CPU", "X86");
					}
					else if (pluginArch == "WSA_UWP_x64")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.WSAPlayer, "CPU", "X64");
					}
					else if (pluginArch == "WSA_UWP_ARM")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.WSAPlayer, "CPU", "ARM");
					}
					else if (pluginArch == "WSA_UWP_ARM64")
					{
						pluginImporter.SetPlatformData(UnityEditor.BuildTarget.WSAPlayer, "CPU", "ARM64");
					}
					break;

				case "Windows":
					pluginArch = splitPath[1];
					pluginConfig = splitPath[2];

					if (pluginArch != "x86" && pluginArch != "x86_64")
					{
						UnityEngine.Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
						continue;
					}
					SetStandalonePlatformData(pluginImporter, pluginPlatform, pluginArch);
					break;

				case "Switch":
					pluginArch = splitPath[1];
					pluginConfig = splitPath[2];

					if (SwitchBuildTarget == INVALID_BUILD_TARGET)
					{
						continue;
					}

					if (pluginArch != "NX32" && pluginArch != "NX64")
					{
						UnityEngine.Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
						continue;
					}
					break;

				default:
					UnityEngine.Debug.Log("WwiseUnity: Unknown platform: " + pluginPlatform);
					continue;
			}

			var AssetChanged = false;
			if (pluginImporter.GetCompatibleWithAnyPlatform())
			{
				pluginImporter.SetCompatibleWithAnyPlatform(false);
				AssetChanged = true;
			}

			var bActivate = true;
			if (pluginConfig == "DSP")
			{
				if (!bIsSupportLibrary && !IsPluginUsed(platformConfiguration, pluginPlatform, System.IO.Path.GetFileNameWithoutExtension(pluginImporter.assetPath)))
				{
					bActivate = false;
				}
			}
			else if (pluginConfig != GetCurrentConfig())
			{
				bActivate = false;
			}

			bool isCompatibleWithPlatform = bActivate && Activate;
			if (!bActivate && target == UnityEditor.BuildTarget.WSAPlayer)
			{
				AssetChanged = true;
			}
			else
			{
				AssetChanged |= pluginImporter.GetCompatibleWithPlatform(target) != isCompatibleWithPlatform;
			}

			pluginImporter.SetCompatibleWithPlatform(target, isCompatibleWithPlatform);

			if (AssetChanged)
			{
				pluginImporter.SaveAndReimport();
			}
		}
	}

	private static void FilterOutPlatformIfNeeded_Default(UnityEditor.BuildTarget target, UnityEditor.PluginImporter pluginImporter, string pluginPlatform)
	{
	}

	public static void ActivatePluginsForEditor()
	{
		var importers = GetWwisePluginImporters();
		var ChangedSomeAssets = false;

		foreach (var pluginImporter in importers)
		{
			var splitPath = GetPluginInfoFromPath(pluginImporter.assetPath);
			if (splitPath == null)
			{
				continue;
			}

			var pluginPlatform = splitPath[0];
			var pluginConfig = string.Empty;
			var editorCPU = string.Empty;
			var editorOS = string.Empty;

			PlatformConfiguration platformConfiguration = null;
			switch (pluginPlatform)
			{
				case "Mac":
					pluginConfig = splitPath[1];
					editorCPU = "AnyCPU";
					editorOS = "OSX";
					if (!BuildTargetToPlatformConfiguration.TryGetValue(UnityEditor.BuildTarget.StandaloneOSX, out platformConfiguration))
						platformConfiguration = null;
					break;

				case "Windows":
					editorCPU = splitPath[1];
					pluginConfig = splitPath[2];
					editorOS = "Windows";
					if (!BuildTargetToPlatformConfiguration.TryGetValue(UnityEditor.BuildTarget.StandaloneWindows64, out platformConfiguration))
						platformConfiguration = null;
					break;
			}

			var AssetChanged = false;
			if (pluginImporter.GetCompatibleWithAnyPlatform())
			{
				pluginImporter.SetCompatibleWithAnyPlatform(false);
				AssetChanged = true;
			}

			var bActivate = false;
			if (!string.IsNullOrEmpty(editorOS))
			{
				// If we can't find the platform configuration, it simply just might not have bene registered yet.
				// Better to do nothing than to deactivate the found plugins. If the plugin was activated, it will
				// stay activated.
				if (platformConfiguration != null)
				{
					if (pluginConfig == "DSP")
					{
						if (!s_PerPlatformPlugins.ContainsKey(platformConfiguration.WwisePlatformName))
						{
							continue;
						}

						bActivate = IsPluginUsed(platformConfiguration, pluginPlatform,
							System.IO.Path.GetFileNameWithoutExtension(pluginImporter.assetPath));
					}
					else
					{
						bActivate = pluginConfig == EditorConfiguration;
					}

					if (bActivate)
					{
						pluginImporter.SetEditorData("CPU", editorCPU);
						pluginImporter.SetEditorData("OS", editorOS);
					}

					AssetChanged |= pluginImporter.GetCompatibleWithEditor() != bActivate;
					pluginImporter.SetCompatibleWithEditor(bActivate);
				}
			}

			if (AssetChanged)
			{
				ChangedSomeAssets = true;
				UnityEditor.AssetDatabase.ImportAsset(pluginImporter.assetPath);
			}
		}

		if (ChangedSomeAssets)
		{
			UnityEngine.Debug.Log("WwiseUnity: Plugins successfully activated for " + EditorConfiguration + " in Editor.");
		}
	}

	private static void CheckMenuItems(string config)
	{
		UnityEditor.Menu.SetChecked(MENU_PATH + CONFIG_DEBUG, config == CONFIG_DEBUG);
		UnityEditor.Menu.SetChecked(MENU_PATH + CONFIG_PROFILE, config == CONFIG_PROFILE);
		UnityEditor.Menu.SetChecked(MENU_PATH + CONFIG_RELEASE, config == CONFIG_RELEASE);
	}

	public static void DeactivateAllPlugins()
	{
		var importers = GetWwisePluginImporters();
		foreach (var pluginImporter in importers)
		{
			if (pluginImporter.assetPath.IndexOf(WwisePluginFolder, System.StringComparison.OrdinalIgnoreCase) == -1)
			{
				continue;
			}

			pluginImporter.SetCompatibleWithAnyPlatform(false);
			UnityEditor.AssetDatabase.ImportAsset(pluginImporter.assetPath);
		}
	}

	public static bool IsPluginUsed(PlatformConfiguration in_config, string in_UnityPlatform, string in_PluginName)
	{
		var pluginDSPPlatform = in_config.WwisePlatformName;

		if (!s_PerPlatformPlugins.ContainsKey(pluginDSPPlatform))
		{
			return false; //XML not parsed, don't touch anything.
		}

		if (in_PluginName.Contains("AkSoundEngine"))
		{
			return true;
		}

		var pluginName = in_PluginName;
		if (in_PluginName.StartsWith("lib"))
		{
			pluginName = in_PluginName.Substring(3);
		}

		int indexOfFactory = in_PluginName.IndexOf("Factory");
		if (indexOfFactory != -1)
		{
			pluginName = in_PluginName.Substring(0, indexOfFactory);
		}

		System.Collections.Generic.HashSet<AkPluginInfo> plugins;
		if (s_PerPlatformPlugins.TryGetValue(pluginDSPPlatform, out plugins))
		{
			if (!in_config.RequiresStaticPluginRegistration)
			{
				foreach (var pluginInfo in plugins)
				{
					if (pluginInfo.DllName == pluginName)
					{
						return true;
					}
				}
			}

			//Exceptions
			if (!string.IsNullOrEmpty(in_config.StaticPluginRegistrationName) && pluginName.Contains(in_config.StaticPluginRegistrationName))
			{
				return true;
			}

			//WebGL, iOS, tvOS, and Switch deal with the static libs directly, unlike all other platforms.
			//Luckily the DLL name is always a subset of the lib name.
			foreach (var pluginInfo in plugins)
			{
				if (pluginInfo.StaticLibName == pluginName)
				{
					return true;
				}
			}
		}

		return false;
	}

	public static void Update(bool forceUpdate = false)
	{
		//Gather all GeneratedSoundBanks folder from the project
		var allPaths = AkUtilities.GetAllBankPaths();
		var bNeedRefresh = false;
		var projectDir = AkBasePathGetter.GetWwiseProjectDirectory();
		var baseSoundBankPath = AkBasePathGetter.GetFullSoundBankPathEditor();

		AkWwiseInitializationSettings.UpdatePlatforms();

		//make a copy of the platform map and handle "special" custom platforms
		var platformMap = new Dictionary<string, List<string>>();
		foreach (var key in AkUtilities.PlatformMapping.Keys)
		{
			platformMap.Add(key, new List<string>(AkUtilities.PlatformMapping[key]));
			foreach (var customPF in AkUtilities.PlatformMapping[key])
			{
				if (customPF != key && (AkWwiseInitializationSettings.PlatformSettings.IsDistinctPlatform(customPF)))
				{
					platformMap.Add(customPF, new List<string> { customPF });
					platformMap[key].Remove(customPF);
				}
			}
			if (platformMap[key].Count==0)
			{
				platformMap.Remove(key);
			}
		}


		//Go through all BasePlatforms 
		foreach (var pairPF in platformMap)
		{
			//Go through all custom platforms related to that base platform and check if any of the bank files were updated.
			var bParse = forceUpdate;
			var fullPaths = new System.Collections.Generic.List<string>();
			foreach (var customPF in pairPF.Value)
			{
				string bankPath;
				if (!allPaths.TryGetValue(customPF, out bankPath))
					continue;

				var pluginFile = "";
				try
				{
					pluginFile = System.IO.Path.Combine(projectDir, System.IO.Path.Combine(bankPath, "PluginInfo.xml"));
					pluginFile = pluginFile.Replace('/', System.IO.Path.DirectorySeparatorChar);
					if (!System.IO.File.Exists(pluginFile))
					{
						//Try in StreamingAssets too.
						pluginFile = System.IO.Path.Combine(System.IO.Path.Combine(baseSoundBankPath, customPF), "PluginInfo.xml");
						if (!System.IO.File.Exists(pluginFile))
							continue;
					}

					fullPaths.Add(pluginFile);

					var t = System.IO.File.GetLastWriteTime(pluginFile);
					var lastTime = System.DateTime.MinValue;
					bool bParsedBefore = s_LastParsed.TryGetValue(customPF, out lastTime);
					if (!bParsedBefore || lastTime < t)
					{
						bParse = true;
						s_LastParsed[customPF] = t;
					}
				}
				catch (System.Exception ex)
				{
					UnityEngine.Debug.LogError("WwiseUnity: " + pluginFile + " could not be parsed. " + ex.Message);
				}
			}

			if (bParse)
			{
				var platform = pairPF.Key;

				var newDlls = ParsePluginsXML(platform, fullPaths);
				System.Collections.Generic.HashSet<AkPluginInfo> oldDlls = null;

				//Remap base Wwise platforms to Unity platform folders names
#if !UNITY_2018_3_OR_NEWER
				if (platform.Contains("Vita"))
				{
					platform = "Vita";
				}
#endif

				s_PerPlatformPlugins.TryGetValue(platform, out oldDlls);
				s_PerPlatformPlugins[platform] = newDlls;

				//Check if there was any change.
				if (!bNeedRefresh && oldDlls != null)
				{
					if (oldDlls.Count == newDlls.Count)
					{
						oldDlls.IntersectWith(newDlls);
					}

					bNeedRefresh |= oldDlls.Count != newDlls.Count;
				}
				else
				{
					bNeedRefresh |= newDlls.Count > 0;
				}
			}
		}

		if (bNeedRefresh)
		{
			ActivatePluginsForEditor();
		}

		var currentConfig = GetCurrentConfig();
		CheckMenuItems(currentConfig);
	}

	private static System.Collections.Generic.HashSet<AkPluginInfo> ParsePluginsXML(string platform,
		System.Collections.Generic.List<string> in_pluginFiles)
	{
		var newPlugins = new System.Collections.Generic.HashSet<AkPluginInfo>();

		foreach (var pluginFile in in_pluginFiles)
		{
			if (!System.IO.File.Exists(pluginFile))
			{
				continue;
			}

			try
			{
				var doc = new System.Xml.XmlDocument();
				doc.Load(pluginFile);
				var Navigator = doc.CreateNavigator();
				var pluginInfoNode = Navigator.SelectSingleNode("//PluginInfo");
				var boolMotion = pluginInfoNode.GetAttribute("Motion", "");

				var it = Navigator.Select("//PluginLib");

				if (boolMotion == "true")
				{
					AkPluginInfo motionPluginInfo = new AkPluginInfo();
					motionPluginInfo.DllName = "AkMotion";
					newPlugins.Add(motionPluginInfo);
				}

				foreach (System.Xml.XPath.XPathNavigator node in it)
				{
					var rawPluginID = uint.Parse(node.GetAttribute("LibId", ""));
					if (rawPluginID == 0)
					{
						continue;
					}

					PluginID pluginID = (PluginID)rawPluginID;

					if (alwaysSkipPluginsIDs.Contains(pluginID))
					{
						continue;
					}

					var dll = string.Empty;

					if (platform == "Switch" || platform == "Web")
					{
						if (pluginID == PluginID.AkMeter)
						{
							dll = "AkMeter";
						}
					}
					else if (builtInPluginIDs.Contains(pluginID))
					{
						continue;
					}

					if (string.IsNullOrEmpty(dll))
					{
						dll = node.GetAttribute("DLL", "");
					}

					AkPluginInfo newPluginInfo = new AkPluginInfo();
					newPluginInfo.PluginID = rawPluginID;
					newPluginInfo.DllName = dll;

					if (!PluginIDToStaticLibName.TryGetValue(pluginID, out newPluginInfo.StaticLibName))
					{
						newPluginInfo.StaticLibName = dll;
					}

					newPlugins.Add(newPluginInfo);
				}
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError("WwiseUnity: " + pluginFile + " could not be parsed. " + ex.Message);
			}
		}

		return newPlugins;
	}

	private class StaticPluginRegistration
	{
		private readonly System.Collections.Generic.HashSet<string> FactoriesHeaderFilenames =
			new System.Collections.Generic.HashSet<string>();

		private readonly UnityEditor.BuildTarget Target;
		private bool Active;

		public StaticPluginRegistration(UnityEditor.BuildTarget target)
		{
			Target = target;
		}

		public void TryAddLibrary(PlatformConfiguration config, string AssetPath)
		{
			Active = true;
			if (AssetPath.Contains(".a"))
			{
				//Extract the lib name, generate the registration code.
				var begin = AssetPath.LastIndexOf('/') + 4;
				var end = AssetPath.LastIndexOf('.') - begin;
				var LibName = AssetPath.Substring(begin, end); //Remove the lib prefix and the .a extension

				if (!LibName.Contains("AkSoundEngine"))
				{
					string headerFilename = LibName + "Factory.h";

					string fullPath = System.IO.Path.GetFullPath(AkUtilities.GetPathInPackage(WwisePluginFolder + config.DSPDirectoryPath + headerFilename));

					if (System.IO.File.Exists(fullPath))
					{
						FactoriesHeaderFilenames.Add(headerFilename);
					}
					else
					{
						UnityEngine.Debug.LogErrorFormat("WwiseUnity: Could not find '{0}', required for building plugin.", fullPath);
					}
				}
			}
			else if (AssetPath.Contains("Factory.h"))
			{
				FactoriesHeaderFilenames.Add(System.IO.Path.GetFileName(AssetPath));
			}
		}

		public void TryWriteToFile(PlatformConfiguration config)
		{
			if (!Active)
				return;

			string CppText = "#define " + config.StaticPluginDefine;
			string RelativePath = config.DSPDirectoryPath + config.StaticPluginRegistrationName + ".cpp";

			CppText += @"
namespace AK { class PluginRegistration; };
#define AK_STATIC_LINK_PLUGIN(_pluginName_) \
extern AK::PluginRegistration _pluginName_##Registration; \
void *_pluginName_##_fp = (void*)&_pluginName_##Registration;

";

			foreach (var filename in FactoriesHeaderFilenames)
			{
				CppText += "#include \"" + filename + "\"\n";
			}

			try
			{
				var FullPath = System.IO.Path.GetFullPath(AkUtilities.GetPathInPackage(WwisePluginFolder + RelativePath));
				System.IO.File.WriteAllText(FullPath, CppText);
			}
			catch (System.Exception e)
			{
				UnityEngine.Debug.LogError("WwiseUnity: Could not write <" + RelativePath + ">. Exception: " + e.Message);
				return;
			}

			UnityEditor.AssetDatabase.Refresh();
		}

		public System.Collections.Generic.List<string> GetMissingPlugins(System.Collections.Generic.HashSet<AkPluginInfo> usedPlugins)
		{
			var pluginList = new System.Collections.Generic.List<string>();
			if (usedPlugins == null)
				return pluginList;

			foreach (var plugin in usedPlugins)
			{
				if (string.IsNullOrEmpty(plugin.StaticLibName))
				{
					continue;
				}

				string includeFilename = plugin.StaticLibName + "Factory.h";
				if (!FactoriesHeaderFilenames.Contains(includeFilename))
				{
					pluginList.Add(plugin.StaticLibName);
				}
			}

			return pluginList;
		}
	}

	private enum PluginID
	{
		// Built-in plugins
		Ak3DAudioBedMixer = 0x00BE0003, // Wwise 3D Audio Bed Mixer
		AkCompressor = 0x006C0003, //Wwise Compressor
		AkRouterMixer = 0x00AC0006, //Wwise RouterMixer
		AkDelay = 0x006A0003, //Delay
		AkExpander = 0x006D0003, //Wwise Expander
		AkGain = 0x008B0003, //Gain
		AkMatrixReverb = 0x00730003, //Matrix Reverb
		AkMeter = 0x00810003, //Wwise Meter
		AkParametricEQ = 0x00690003, //Wwise Parametric EQ
		AkPeakLimiter = 0x006E0003, //Wwise Peak Limiter
		AkRoomVerb = 0x00760003, //Wwise RoomVerb
		SineGenerator = 0x00640002, //Sine
		SinkAuxiliary = 0xB40007,
		SinkCommunication = 0xB00007,
		SinkControllerHeadphones = 0xB10007,
		SinkControllerSpeaker = 0xB30007,
		SinkDVRByPass = 0xAF0007,
		SinkNoOutput = 0xB50007,
		SinkSystem = 0xAE0007,
		ToneGenerator = 0x00660002, //Tone Generator
		WwiseSilence = 0x00650002, //Wwise Silence
#if !UNITY_2018_3_OR_NEWER
		VitaReverb = 0x008C0003, //Vita Reverb
		VitaCompressor = 0x008D0003, //Vita Compressor
		VitaDelay = 0x008E0003, //Vita Delay
		VitaDistortion = 0x008F0003, //Vita Distortion
		VitaEQ = 0x00900003, //Vita EQ
#endif

		// Static or DLL plugins
		AkAudioInput = 0xC80002,
		AkConvolutionReverb = 0x7F0003,
		AkFlanger = 0x7D0003,
		AkGuitarDistortion = 0x7E0003,
		AkHarmonizer = 0x8A0003,
		AkMotionSink = 0x1FB0007,
		AkMotionSource = 0x1990002,
		AkMotionGeneratorSource = 0x1950002,
		AkPitchShifter = 0x880003,
		AkRecorder = 0x840003,
		AkReflect = 0xAB0003,
		AkSoundSeedGrain = 0xB70002,
		AkSoundSeedWind = 0x770002,
		AkSoundSeedWoosh = 0x780002,
		AkStereoDelay = 0x870003,
		AkSynthOne = 0x940002,
		AkTimeStretch = 0x820003,
		AkTremolo = 0x830003,
		AuroHeadphone = 0x44C1073,
		CrankcaseAudioREVModelPlayer = 0x1A01052,
		iZHybridReverb = 0x21033,
		iZTrashBoxModeler = 0x71033,
		iZTrashDelay = 0x41033,
		iZTrashDistortion = 0x31033,
		iZTrashDynamics = 0x51033,
		iZTrashFilters = 0x61033,
		iZTrashMultibandDistortion = 0x91033,
		MasteringSuite = 0xBA0003,
		AkImpacterSource = 0xB80002,
		McDSPFutzBox = 0x6E1003,
		McDSPLimiter = 0x671003,
		ResonanceAudioRenderer = 0x641103,
		ResonanceAudioRoomEffect = 0xC81106,
		IgniterLive = 0x5110D2,
		IgniterLiveSynth = 0x5210D2
	}

	private class AkPluginInfo
	{
		public uint PluginID;
		public string DllName;
		public string StaticLibName;

		public override int GetHashCode()
		{
			return PluginID.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return PluginID.Equals(obj);
		}
	}
}
#endif