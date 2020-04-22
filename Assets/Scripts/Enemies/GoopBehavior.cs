using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GoopPoint {
    [SerializeField] public Vector3 position;
    [SerializeField] public float density;
    [SerializeField] public float time;

    public GoopPoint()
    {
        position = new Vector3();
        density = 1f;
        time = 0f;
    }
}

public class GoopBehavior : MonoBehaviour
{
    public List<GoopPoint> path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
