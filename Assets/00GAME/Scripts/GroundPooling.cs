using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPooling : ObjectPooling<Ground>
{
    public override void ReturnAllPooling()
    {
        foreach (Transform g in this.transform)
        {
            if (g.gameObject.activeSelf)
            {
                g.GetComponent<Ground>().SetPerfectTime(false);
                ReturnToPool(g.GetComponent<Ground>());
            }
        }
    }
}
