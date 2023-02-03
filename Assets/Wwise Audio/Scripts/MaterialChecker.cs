using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChecker : MonoBehaviour
{
    public LayerMask layerMask;

    #region private variables
    private RaycastHit hit;
    private Vector3 direction = Vector3.down;
    private Transform trn;
    private Vector3 checkOffset = Vector3.up * 0.1f;
    #endregion


    public AK.Wwise.Switch GetMaterial()
    {
        if (Physics.Raycast(trn.position + checkOffset, direction, out hit, layerMask))
        {
            SoundMaterial sm = hit.collider.gameObject.GetComponent<SoundMaterial>();

            if (sm != null)
            {
                return sm.material;
            }

        }
        return null;
    }

}
