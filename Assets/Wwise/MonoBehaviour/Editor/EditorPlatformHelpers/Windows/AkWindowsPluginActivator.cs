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

﻿#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class AkWindowsPluginActivator
{
	static AkWindowsPluginActivator()
	{
		AkPluginActivator.RegisterBuildTarget(UnityEditor.BuildTarget.StandaloneWindows, new AkPluginActivator.PlatformConfiguration
		{
			WwisePlatformName = "Windows",
			PluginDirectoryName = "Windows"
		});
		AkPluginActivator.RegisterBuildTarget(UnityEditor.BuildTarget.StandaloneWindows64, new AkPluginActivator.PlatformConfiguration
		{
			WwisePlatformName = "Windows",
			PluginDirectoryName = "Windows"
		});

		var buildConfig = new AkBuildPreprocessor.PlatformConfiguration
		{
			WwisePlatformName = "Windows"
		};
		AkBuildPreprocessor.RegisterBuildTarget(UnityEditor.BuildTarget.StandaloneWindows, buildConfig);
		AkBuildPreprocessor.RegisterBuildTarget(UnityEditor.BuildTarget.StandaloneWindows64, buildConfig);
	}
}
#endif