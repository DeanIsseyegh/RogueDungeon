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

[UnityEngine.AddComponentMenu("Wwise/Spatial Audio/AkRoom")]
[UnityEngine.RequireComponent(typeof(UnityEngine.Collider))]
[UnityEngine.DisallowMultipleComponent]
/// @brief An AkRoom is an enclosed environment that can only communicate to the outside/other rooms with AkRoomPortals
/// @details The AkRoom component uses its required Collider component to determine when AkRoomAwareObjects enter and exit the room using the OnTriggerEnter and OnTriggerExit callbacks.
public class AkRoom : AkTriggerHandler
{
	public static ulong INVALID_ROOM_ID = unchecked((ulong)(-1));

	public static ulong GetAkRoomID(AkRoom room)
	{
		return room == null ? INVALID_ROOM_ID : room.GetID();
	}

	public static int RoomCount { get; private set; }

	#region Fields

	[UnityEngine.Tooltip("Higher number has a higher priority")]
	/// In cases where a game object is in an area with two rooms, the higher priority room will be chosen for AK::SpatialAudio::SetGameObjectInRoom()
	/// The higher the priority number, the higher the priority of a room.
	public int priority = 0;

	/// The reverb auxiliary bus.
	public AK.Wwise.AuxBus reverbAuxBus = new AK.Wwise.AuxBus();

	[UnityEngine.Range(0, 1)]
	/// The reverb control value for the send to the reverb aux bus.
	public float reverbLevel = 1;

	[UnityEngine.Range(0, 1)]
	/// Loss value modeling transmission through walls.
	public float transmissionLoss = 1;

	/// Wwise Event to be posted on the room game object.
	public AK.Wwise.Event roomToneEvent = new AK.Wwise.Event();

	[UnityEngine.Range(0, 1)]
	[UnityEngine.Tooltip("Send level for sounds that are posted on the room game object; adds reverb to ambience and room tones. Valid range: (0.f-1.f). A value of 0 disables the aux send.")]
	/// Send level for sounds that are posted on the room game object; adds reverb to ambience and room tones. Valid range: (0.f-1.f). A value of 0 disables the aux send.
	public float roomToneAuxSend = 0;

	/// This is the list of AkRoomAwareObjects that have entered this AkRoom
	private System.Collections.Generic.List<AkRoomAwareObject> roomAwareObjectsEntered = new System.Collections.Generic.List<AkRoomAwareObject>();

	/// This is the list of AkRoomAwareObjects that have entered this AkRoom while it was inactive or disabled.
	private System.Collections.Generic.List<AkRoomAwareObject> roomAwareObjectsDetectedWhileDisabled = new System.Collections.Generic.List<AkRoomAwareObject>();

	private UnityEngine.Collider roomCollider = null;
	private System.Type previousColliderType;

	private UnityEngine.Vector3 previousPosition;
	private UnityEngine.Vector3 previousScale;
	private UnityEngine.Quaternion previousRotation;
	private bool bSentToWwise = false;
	private ulong geometryID = AkSurfaceReflector.INVALID_GEOMETRY_ID;

	#endregion

	public bool TryEnter(AkRoomAwareObject roomAwareObject)
	{
		if (roomAwareObject)
		{
			if (isActiveAndEnabled)
			{
				if(!roomAwareObjectsEntered.Contains(roomAwareObject))
					roomAwareObjectsEntered.Add(roomAwareObject);
				return true;
			}
			else
			{
				if (!roomAwareObjectsDetectedWhileDisabled.Contains(roomAwareObject))
					roomAwareObjectsDetectedWhileDisabled.Add(roomAwareObject);
				return false;
			}
		}
		return false;
	}

	public void Exit(AkRoomAwareObject roomAwareObject)
	{
		if (roomAwareObject)
		{
			roomAwareObjectsEntered.Remove(roomAwareObject);
			roomAwareObjectsDetectedWhileDisabled.Remove(roomAwareObject);
		}
	}

	/// Access the room's ID
	public ulong GetID()
	{
		return AkSoundEngine.GetAkGameObjectID(gameObject);
	}

	public ulong GetGeometryID()
	{
		return geometryID;
	}

	public void SetGeometryID(ulong id)
	{
		geometryID = id;
	}

	public bool IsAssociatedGeometryFromCollider()
	{
		return geometryID == GetID();
	}

	private void SetGeometryFromCollider()
	{
		_SetGeometryFromCollider(true);
	}

	private void UpdateGeometryFromCollider()
	{
		_SetGeometryFromCollider(false);
	}

	private void _SetGeometryFromCollider(bool needSetGeometry)
	{
		if (roomCollider == null)
			roomCollider = GetComponent<UnityEngine.Collider>();

		if (roomCollider.GetType() == typeof(UnityEngine.MeshCollider))
		{
			geometryID = GetID();
			UnityEngine.MeshCollider meshCollider = GetComponent<UnityEngine.MeshCollider>();

			if (needSetGeometry)
				AkSurfaceReflector.SetGeometryFromMesh(meshCollider.sharedMesh, geometryID, false, false, false);
			AkSurfaceReflector.SetGeometryInstance(geometryID, geometryID, INVALID_ROOM_ID, transform);

			previousColliderType = typeof(UnityEngine.MeshCollider);
		}
		else if (roomCollider.GetType() == typeof(UnityEngine.BoxCollider))
		{
			geometryID = GetID();
			UnityEngine.BoxCollider boxCollider = GetComponent<UnityEngine.BoxCollider>();
			UnityEngine.GameObject tempGameObject = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
			UnityEngine.Mesh mesh = tempGameObject.GetComponent<UnityEngine.MeshFilter>().sharedMesh;

			tempGameObject.transform.position = boxCollider.bounds.center;
			tempGameObject.transform.rotation = transform.rotation;
			UnityEngine.Vector3 roomScale = new UnityEngine.Vector3();
			roomScale.x = transform.localScale.x * boxCollider.size.x;
			roomScale.y = transform.localScale.y * boxCollider.size.y;
			roomScale.z = transform.localScale.z * boxCollider.size.z;
			tempGameObject.transform.localScale = roomScale;

			if (needSetGeometry)
				AkSurfaceReflector.SetGeometryFromMesh(mesh, geometryID, false, false, false);
			AkSurfaceReflector.SetGeometryInstance(geometryID, geometryID, INVALID_ROOM_ID, tempGameObject.transform);

			previousColliderType = typeof(UnityEngine.BoxCollider);
			UnityEngine.GameObject.Destroy(tempGameObject);
		}
		else if (roomCollider.GetType() == typeof(UnityEngine.CapsuleCollider))
		{
			geometryID = GetID();
			UnityEngine.CapsuleCollider capsuleCollider = GetComponent<UnityEngine.CapsuleCollider>();
			UnityEngine.GameObject tempGameObject = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
			UnityEngine.Mesh mesh = tempGameObject.GetComponent<UnityEngine.MeshFilter>().sharedMesh;

			tempGameObject.transform.position = capsuleCollider.bounds.center;
			tempGameObject.transform.rotation = transform.rotation;
			tempGameObject.transform.localScale = GetCapsuleScale(transform.localScale, capsuleCollider.radius, capsuleCollider.height, capsuleCollider.direction);

			if (needSetGeometry)
				AkSurfaceReflector.SetGeometryFromMesh(mesh, geometryID, false, false, false);
			AkSurfaceReflector.SetGeometryInstance(geometryID, geometryID, INVALID_ROOM_ID, tempGameObject.transform);

			previousColliderType = typeof(UnityEngine.CapsuleCollider);
			UnityEngine.GameObject.Destroy(tempGameObject);
		}
		else if (roomCollider.GetType() == typeof(UnityEngine.SphereCollider))
		{
			geometryID = GetID();
			UnityEngine.GameObject tempGameObject = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere);
			UnityEngine.Mesh mesh = tempGameObject.GetComponent<UnityEngine.MeshFilter>().sharedMesh;

			tempGameObject.transform.position = roomCollider.bounds.center;
			tempGameObject.transform.localScale = roomCollider.bounds.size;

			if (needSetGeometry)
				AkSurfaceReflector.SetGeometryFromMesh(mesh, geometryID, false, false, false);
			AkSurfaceReflector.SetGeometryInstance(geometryID, geometryID, INVALID_ROOM_ID, tempGameObject.transform);

			previousColliderType = typeof(UnityEngine.SphereCollider);
			UnityEngine.GameObject.Destroy(tempGameObject);
		}
		else
		{
			UnityEngine.Debug.LogWarning(name + " has an invalid collider for wet transmission. Wet Transmission will be disabled.");
			// in case a geometry was added with the room's ID, remove it
			if (previousColliderType == typeof(UnityEngine.MeshCollider) ||
				previousColliderType == typeof(UnityEngine.BoxCollider) ||
				previousColliderType == typeof(UnityEngine.SphereCollider) ||
				previousColliderType == typeof(UnityEngine.CapsuleCollider))
				AkSoundEngine.RemoveGeometry(GetID());

			previousColliderType = roomCollider.GetType();
			geometryID = AkSurfaceReflector.INVALID_GEOMETRY_ID;
		}
	}

	public void SetRoom()
	{
		if (!AkSoundEngine.IsInitialized())
			return;

		var roomParams = new AkRoomParams
		{
			Up = transform.up,
			Front = transform.forward,

			ReverbAuxBus = reverbAuxBus.Id,
			ReverbLevel = reverbLevel,
			TransmissionLoss = transmissionLoss,

			RoomGameObj_AuxSendLevelToSelf = roomToneAuxSend,
			RoomGameObj_KeepRegistered = roomToneEvent.IsValid(),
		};

		if (bSentToWwise == false)
			RoomCount++;

		if (geometryID == AkSurfaceReflector.INVALID_GEOMETRY_ID)
		{
			SetGeometryFromCollider();
		}

		AkSoundEngine.SetRoom(GetID(), roomParams, geometryID, name);
		bSentToWwise = true;

		AkRoomManager.RegisterRoomUpdate(this);
	}

	private void Update()
	{
		if (previousPosition != transform.position ||
			previousScale != transform.lossyScale ||
			previousRotation != transform.rotation)
		{
			if (IsAssociatedGeometryFromCollider())
			{
				UpdateGeometryFromCollider();
			}

			if (previousRotation != transform.rotation)
			{
				SetRoom();
			}
			else
			{
				AkRoomManager.RegisterRoomUpdate(this);
			}

			previousPosition = transform.position;
			previousScale = transform.lossyScale;
			previousRotation = transform.rotation;
		}
	}

	private UnityEngine.Vector3 GetCapsuleScale(UnityEngine.Vector3 localScale, float radius, float height, int direction)
	{
		UnityEngine.Vector3 scale = new UnityEngine.Vector3();

		switch (direction)
		{
			case 0:
				scale.y = UnityEngine.Mathf.Max(localScale.y, localScale.z) * (radius * 2);
				scale.z = scale.y;
				scale.x = UnityEngine.Mathf.Max(scale.y, localScale.x * height);
				break;
			case 2:
				scale.x = UnityEngine.Mathf.Max(localScale.x, localScale.y) * (radius * 2);
				scale.y = scale.x;
				scale.z = UnityEngine.Mathf.Max(scale.x, localScale.z * height);
				break;
			case 1:
			default:
				scale.x = UnityEngine.Mathf.Max(localScale.x, localScale.z) * (radius * 2);
				scale.y = UnityEngine.Mathf.Max(scale.x, localScale.y * height);
				scale.z = scale.x;
				break;
		}

		return scale;
	}

	public override void OnEnable()
	{
		roomCollider = GetComponent<UnityEngine.Collider>();

		AkSurfaceReflector surfaceReflectorComponent = gameObject.GetComponent<AkSurfaceReflector>();
		if (surfaceReflectorComponent != null && surfaceReflectorComponent.enabled)
			SetGeometryID(surfaceReflectorComponent.GetID());
		else
			SetGeometryFromCollider();

		SetRoom();

		// if objects entered the room while disabled, enter them now
		for (var i = 0; i < roomAwareObjectsDetectedWhileDisabled.Count; ++i)
			AkRoomAwareManager.ObjectEnteredRoom(roomAwareObjectsDetectedWhileDisabled[i], this);

		roomAwareObjectsDetectedWhileDisabled.Clear();
		base.OnEnable();

		// init update condition
		previousPosition = transform.position;
		previousScale = transform.lossyScale;
		previousRotation = transform.rotation;
	}

	private void OnDisable()
	{
		for (var i = 0; i < roomAwareObjectsEntered.Count; ++i)
		{
			roomAwareObjectsEntered[i].ExitedRoom(this);
			AkRoomAwareManager.RegisterRoomAwareObjectForUpdate(roomAwareObjectsEntered[i]);
			roomAwareObjectsDetectedWhileDisabled.Add(roomAwareObjectsEntered[i]);
		}
		roomAwareObjectsEntered.Clear();

		AkRoomManager.RegisterRoomUpdate(this);

		// in case a geometry was added with the room's ID, remove it
		if (previousColliderType == typeof(UnityEngine.MeshCollider) ||
			previousColliderType == typeof(UnityEngine.BoxCollider) ||
			previousColliderType == typeof(UnityEngine.SphereCollider) ||
			previousColliderType == typeof(UnityEngine.CapsuleCollider))
			AkSoundEngine.RemoveGeometry(GetID());
		previousColliderType = null;

		// stop sounds applied to the room game object
		if (roomToneEvent.IsValid())
			AkSoundEngine.StopAll(GetID());

		RoomCount--;
		AkSoundEngine.RemoveRoom(GetID());
		bSentToWwise = false;
	}

	private void OnTriggerEnter(UnityEngine.Collider in_other)
	{
		AkRoomAwareManager.ObjectEnteredRoom(in_other, this);
	}

	private void OnTriggerExit(UnityEngine.Collider in_other)
	{
		AkRoomAwareManager.ObjectExitedRoom(in_other, this);
	}

	public void PostRoomTone()
	{
		if (roomToneEvent.IsValid() && isActiveAndEnabled)
			roomToneEvent.Post(GetID());
	}

	public override void HandleEvent(UnityEngine.GameObject in_gameObject)
	{
		PostRoomTone();
	}

	public class PriorityList
	{
		private static readonly CompareByPriority s_compareByPriority = new CompareByPriority();

		/// Contains all active rooms sorted by priority.
		private System.Collections.Generic.List<AkRoom> rooms = new System.Collections.Generic.List<AkRoom>();

		public ulong GetHighestPriorityActiveAndEnabledRoomID()
		{
			var room = GetHighestPriorityActiveAndEnabledRoom();
			return GetAkRoomID(room);
		}
		public AkRoom GetHighestPriorityActiveAndEnabledRoom()
		{
			for (int i = 0; i < rooms.Count; i++)
			{
				if (rooms[i].isActiveAndEnabled)
					return rooms[i];
			}

			return null;
		}

		public int Count { get { return rooms.Count; } }

		public void Clear()
		{
			rooms.Clear();
		}

		public void Add(AkRoom room)
		{
			var index = BinarySearch(room);
			if (index < 0)
				rooms.Insert(~index, room);
		}

		public void Remove(AkRoom room)
		{
			rooms.Remove(room);
		}

		public bool Contains(AkRoom room)
		{
			return room && rooms.Contains(room);
		}

		public int BinarySearch(AkRoom room)
		{
			return room ? rooms.BinarySearch(room, s_compareByPriority) : -1;
		}

		public AkRoom this[int index]
		{
			get { return rooms[index]; }
		}

		private class CompareByPriority : System.Collections.Generic.IComparer<AkRoom>
		{
			public virtual int Compare(AkRoom a, AkRoom b)
			{
				var result = a.priority.CompareTo(b.priority);
				if (result == 0 && a != b)
					return 1;

				return -result; // inverted to have highest priority first
			}
		}
	}

#region Obsolete
	[System.Obsolete(AkSoundEngine.Deprecation_2021_1_0)]
	public float wallOcclusion
	{
		get
		{
			return transmissionLoss;
		}
		set
		{
			transmissionLoss = value;
		}
	}
#endregion
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.