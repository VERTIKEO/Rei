using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReflectionChild : MonoBehaviour
{
    // TODO: on PostProcessScene
    void Awake()
    {
        BoxCollider c;
        if(transform.parent.gameObject.TryGetComponent<BoxCollider>(out c)) {
            ReflectionProbe r = GetComponent<ReflectionProbe>(); 
            r.size = c.size;
            r.center = c.center - transform.localPosition;
        }

        this.enabled = false;
    }
}
