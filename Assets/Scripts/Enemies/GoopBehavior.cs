using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GoopKey {
    [SerializeField] public Vector3 position;
    [SerializeField] public float density;
    [SerializeField] public float time;

    // TODO: add padding to reach 32 bytes

    // public GoopKey()
    // {
    //     position = new Vector3();
    //     density = 1f;
    //     time = 0f;
    // }
}

[Serializable]
public struct Metaball {
    [SerializeField] public Vector3 pos;
    [SerializeField] public float radius;
    [SerializeField] public float wiggle;

    // TODO: add padding to reach 32 bytes

    public Metaball(GoopKey key) {
        this.pos = key.position;
        this.radius = key.density / 2f;
        this.wiggle = 0f;
    }

    Vector3 Animate(float delta) {
        // TODO: animate movement;
        return pos;
    }
}

public class GoopBehavior : MonoBehaviour
{
    public List<GoopKey> path;
    public Material material;
    public PhysicMaterial physicMaterial;
    public bool isCollisionTrigger = false;

    [Header("Quality parameters")]
    public float isoLevel = 0.5f;
    //public float 

    //[Header("Animation parameters")]

    // Start is called before the first frame update
    void Start()
    {
        Populate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     *     Goop handling
     */
    [HideInInspector]
    public bool isCollisionBaked;
    private List<Metaball> _metaballs;

    void SimulateMetaballs() {

    }

    /// Given a data structure of GoopKey instances, 
    void Populate() {
        for (int i = 0; i < path.Count - 1; i++) {
            GoopKey a = path[i];
            GoopKey b = path[i+1];

            Metaball first = new Metaball(a.density > b.density ? a : b);
            Metaball last = new Metaball(a.density > b.density ? b : a);

            _metaballs.AddRange(PopulateSegment(first, last));
        }
    }

    void GenerateColliders() {

    }

    // TODO: split in more functions
    IEnumerable<Metaball> PopulateSegment(Metaball first, Metaball last) {
        List<Metaball> balls = new List<Metaball>();
        Vector3 segment = last.pos - first.pos;
        Vector3 segmentNormal = segment.normalized;
        float segmentLength = segment.magnitude;

        // Step 1) generate intra-keys
        Metaball[] intrakeys;
        {

            // calculate how many spheres fit in a sliced cone between two GoopKeys
            _Cone cone = new _Cone();
            cone.height = segmentLength;
            // approximate correct cone size

            // calculate intraspheres
            float[] intraspheres = _FitSpheresInCone(cone, last.radius);

            // put intraspheres along segment
            intrakeys = new Metaball[intraspheres.Length];
            Metaball prevKey = first;
            for (int i = 0; i <intraspheres.Length; i++) {
                // position: previous position + direction normal * (previous radius + current diameter)
                intrakeys[i].pos = prevKey.pos + segmentNormal * (prevKey.radius + intraspheres[i]*2f); // TODO: make position extension interpolable
                intrakeys[i].radius = intraspheres[i];
                intrakeys[i].wiggle = 0;

                prevKey = intrakeys[i];
            }
        }

        // Step 2) instantiate colliders for intrakeys
        foreach (Metaball ball in intrakeys) {
            GameObject obj = new GameObject("Subcollider");
            obj.transform.parent = this.transform;

            // add components
            obj.AddComponent<SphereCollider>();

            // set attributes
            SphereCollider collider = obj.GetComponent<SphereCollider>();
            collider.radius = ball.radius;
            collider.material = this.physicMaterial;
            collider.isTrigger = this.isCollisionTrigger;

            obj.transform.localPosition = first.pos + ball.pos;
        }

        // Step 3) generate particle metaballs
        Metaball[] particles = {};
        {

        }


        balls.AddRange(intrakeys);
        balls.AddRange(particles);
        return balls;
    }

    /*
     *  Math utils
     */
    private struct _Cone {
        public float height;
        public float baseRadius;
    }

    private static float[] _FitSpheresInCone(_Cone cone, float minRadius) {
        List<float> spheres = new List<float>();

        while(true /* until the cone is short enough */) {
            float radius = _FitSphereInCone(cone);
            if(radius < minRadius) break;
            spheres.Add(radius);

            // cut cone
            cone.height -= radius * 2f;
            cone.baseRadius = 0f;
        }

        return spheres.ToArray();
    }

    /// Find radius of sphere at the base of the cone
    /// (source: http://mathcentral.uregina.ca/QQ/database/QQ.09.07/s/juan1.html)
    private static float _FitSphereInCone(_Cone cone) {
        float radius = 0f;
        float hypothenuse = Mathf.Sqrt(cone.baseRadius*cone.baseRadius + cone.height*cone.height);

        radius = (cone.height * cone.baseRadius) / hypothenuse;
        radius /= 1f + (cone.baseRadius / hypothenuse);

        return radius;
    }
}
