#if !(UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) <COPYRIGHTYEAR> Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public class AkVector64
{
	public void Zero() { 
		X = 0.0;
		Y = 0.0;
		Z = 0.0;
	}

	public double X = 0.0;
	public double Y = 0.0;
	public double Z = 0.0;

	public static implicit operator AkVector64(UnityEngine.Vector3 vector) {
		AkVector64 ret = new AkVector64();
		ret.X = vector.x;
		ret.Y = vector.y;
		ret.Z = vector.z;
		return ret; 
	}

}

#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
