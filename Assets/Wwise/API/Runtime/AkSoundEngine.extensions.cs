#if !(UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
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

using System;

public partial class AkSoundEngine
{
	#region User Hooks - Extended for Auto-Registration

	private const string AkGameObjTypeString = "AkGameObj, AK.Wwise.Unity.MonoBehaviour";

	private class AutoObject
	{
		private readonly UnityEngine.GameObject gameObject;

		public AutoObject(UnityEngine.GameObject go)
		{
			gameObject = go;
			RegisterGameObj(gameObject, gameObject != null ? "AkAutoObject for " + gameObject.name : "AkAutoObject");
		}

		~AutoObject()
		{
			UnregisterGameObj(gameObject);
		}
	}

	private static void AutoRegister(UnityEngine.GameObject gameObject, ulong id)
	{
		Type AkGameObjectType = Type.GetType(AkGameObjTypeString);

		if (gameObject == null || !gameObject.activeInHierarchy)
		{
			new AutoObject(gameObject);
		}
		else if (gameObject.GetComponent(AkGameObjectType) == null)
		{
			gameObject.AddComponent(AkGameObjectType);
		}
	}

	static partial void PreGameObjectAPICallUserHook(UnityEngine.GameObject gameObject, ulong id)
	{
#if UNITY_EDITOR
		if (!UnityEngine.Application.isPlaying)
		{
			AutoRegister(gameObject, id);
			return;
		}
#endif

		if (!IsInRegisteredList(id) && IsInitialized())
			AutoRegister(gameObject, id);
	}

	private static readonly System.Collections.Generic.HashSet<ulong> RegisteredGameObjects =
		new System.Collections.Generic.HashSet<ulong>();

	static partial void PostRegisterGameObjUserHook(AKRESULT result, UnityEngine.GameObject gameObject, ulong id)
	{
#if UNITY_EDITOR
		if (!UnityEngine.Application.isPlaying)
			return;
#endif

		if (result == AKRESULT.AK_Success)
			RegisteredGameObjects.Add(id);
	}

	static partial void PostUnregisterGameObjUserHook(AKRESULT result, UnityEngine.GameObject gameObject, ulong id)
	{
		if (result == AKRESULT.AK_Success)
			RegisteredGameObjects.Remove(id);
	}

	// Helper method that a user might want to implement
	private static bool IsInRegisteredList(ulong id)
	{
		return RegisteredGameObjects.Contains(id);
	}

	// Helper method that a user might want to implement
	public static bool IsGameObjectRegistered(UnityEngine.GameObject in_gameObject)
	{
#if UNITY_EDITOR
		if (!UnityEngine.Application.isPlaying)
		{
			return in_gameObject.GetComponent(AkGameObjTypeString) != null;
		}
#endif

		return IsInRegisteredList(GetAkGameObjectID(in_gameObject));
	}

	#endregion
}
#endif // #if !(UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.