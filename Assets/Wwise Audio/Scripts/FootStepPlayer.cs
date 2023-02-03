using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepPlayer : MonoBehaviour
{

    public MaterialChecker materialChecker; //material checker script

    [SerializeField] private AK.Wwise.Event footstepSound;
    [SerializeField] private AK.Wwise.Switch materialSwitch; //switching wwise

    public void PlayFootStepSound()
    {
        materialChecker.GetMaterial().SetValue(gameObject);

        footstepSound.Post(gameObject);

        Debug.Log(materialSwitch);
    }
}
