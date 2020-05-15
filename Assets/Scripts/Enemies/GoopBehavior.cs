using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GoopKey {
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
public struct Ball {
    [SerializeField] public Vector3 pos;
    [SerializeField] public float radius;

    [SerializeField] public bool negativeBall;

    // TODO: add padding to reach 32 bytes

    // Getters
    [SerializeField] public float wiggle;
    public float radiusSq {
        get { return (this.negativeBall ? -1 : 1) * this.radius * this.radius; }
        
    }

    // Initializers
    public Ball(GoopKey key) {
        this.pos = key.position;
        this.negativeBall = false;
        this.wiggle = 0f;

        this.radius = key.density;
    }

    Vector3 Animate(float delta) {
        // TODO: animate movement;
        return pos;
    }
}


public class GoopBehavior : MonoBehaviour
{
    private static readonly string _SEGMENT_NAME = "Segment";
    private static readonly string _SUBCOLLIDER_NAME = "Subcollider";
    private static readonly float _EPSILON = 0.00001f;

    [SerializeField] public List<GoopKey> path;
    public Material material;
    public PhysicMaterial physicMaterial;
    public bool isCollisionTrigger = false;

    [Header("Visual parameters")]
    public float safeZone = 0.2f;
    

    //[Header("Animation parameters")]

    // [Header("Debug operations")]
    // public bool useGpu = true;

    // Start is called before the first frame update
    public void Start()
    {
        Populate();
        UpdateRenderer();
    }

    // Update is called once per frame
    void Update() {
        SimulateMetaballs();
        UpdateRenderer();
    }

    public void Clear() {
        ClearColliders();
        // container.OnDestroy();
    }

    void UpdateRenderer() {
        int ac = 0;
        foreach(KeyValuePair<GameObject, int> segment in _segmentsCount) {
            Vector4[] balls = new Vector4[16];
            int length = Math.Min(16, segment.Value);

            Debug.Log("Segment: " + segment.Key.transform.localPosition);

            if(length < segment.Value) {
                Debug.LogWarning("Segment has extra items over max count");
            }

            for(int i = 0; i < length; i++) {
                Ball b = _metaballs[ac + i];
                balls[i] = new Vector4(b.pos.x, b.pos.y, b.pos.z, b.radius);
                Debug.Log("Ball: " + balls[i].ToString());
            }

            MeshRenderer renderer = segment.Key.GetComponent<MeshRenderer>();
            renderer.material.SetVectorArray("_Balls", balls);
            renderer.material.SetInt("_BallsCount", length);

            ac += length;
        }
    }

    /*
     *     Goop handling
     */
    [HideInInspector]
    public bool isCollisionBaked;
    private List<Ball> _metaballs;
    private Dictionary<GameObject, int> _segmentsCount = new Dictionary<GameObject, int>();

    void SimulateMetaballs() {

    }

    /// Given a data structure of GoopKey instances, 
    void Populate() {
        Debug.Log("Generating metaballs field from " + path.Count + " keys...");
        System.Diagnostics.Stopwatch _t = new System.Diagnostics.Stopwatch();
        _t.Start();

        _metaballs = new List<Ball>();

        for (int i = 0; i < path.Count - 1; i++) {
            GoopKey a = path[i];
            GoopKey b = path[i+1];

            Ball first = new Ball(a.density > b.density ? a : b);
            Ball last = new Ball(a.density > b.density ? b : a);

            _metaballs.AddRange(PopulateSegment(first, last));
        }

        _t.Stop();
        Debug.Log("Generated " + _metaballs.Count + " metaballs in " + _t.ElapsedMilliseconds + "ms");
    }

    public void GenerateColliders() {
        ClearColliders();
        Populate();
        UpdateRenderer();
    }

    public void ClearColliders() {
        foreach(Transform child in this.transform.Cast<Transform>().ToList()) {
            if(child.name == _SEGMENT_NAME) {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    // TODO: split in more functions
    IEnumerable<Ball> PopulateSegment(Ball first, Ball last) {
        List<Ball> balls = new List<Ball>();
        Vector3 segmentVector = last.pos - first.pos;
        Vector3 segmentNormal = segmentVector.normalized;
        float segmentLength = segmentVector.magnitude;

        // Step 1) create segment GameObject
        GameObject segment = new GameObject(_SEGMENT_NAME);
        {
            segment.transform.parent = this.transform;

            // set transform
            float boxSide = Mathf.Max(first.radius, last.radius) * 2f;
            segment.transform.localScale = new Vector3(segmentLength + first.radius + last.radius + safeZone, boxSide + safeZone, boxSide + safeZone);
            segment.transform.localPosition = (first.pos + last.pos) / 2f;
            segment.transform.localRotation = Quaternion.LookRotation(segmentNormal);
            segment.transform.localEulerAngles = segment.transform.localEulerAngles - new Vector3(0, -90f, 0);

            // add collider component
            BoxCollider box = segment.AddComponent<BoxCollider>();
            box.isTrigger = this.isCollisionTrigger;
            //box.size =    // TODO: subtract the safeZone from collider size

            // add mesh components
            MeshFilter mesh = segment.AddComponent<MeshFilter>();
            mesh.mesh = AssetDatabaseHelper.LoadAssetFromUniqueAssetPath< Mesh > ( "Library/unity default resources::Cube");

            MeshRenderer renderer = segment.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = this.material;
        }

        // Step 2) generate intra-keys
        Ball[] intrakeys = _GenerateIntrakeys(first, last);

        // Step 3) instantiate subcolliders for intrakeys
        foreach (Ball ball in intrakeys) {
            GameObject obj = new GameObject(_SUBCOLLIDER_NAME);
            obj.transform.parent = segment.transform;

            // add components
            obj.AddComponent<SphereCollider>();

            // set attributes
            SphereCollider collider = obj.GetComponent<SphereCollider>();
            collider.radius = ball.radius;
            collider.material = this.physicMaterial;
            collider.isTrigger = this.isCollisionTrigger;

            // obj.transform.localPosition = ball.pos;
        }

        // Step 4) generate particle metaballs
        Ball[] particles = {};
        {

        }


        balls.AddRange(intrakeys);
        balls.AddRange(particles);

        // offset balls' position
        for(int i = 0; i < balls.Count; i++){
            Vector3 pos = balls[i].pos - segment.transform.localPosition;
            pos = segment.transform.localRotation * pos;
            
            Ball b = balls[i];
            b.pos = pos;
            balls[i] = b;
        }

        _segmentsCount.Add(segment, balls.Count);

        return balls;
    }

    private static _Cone _ApproximateCone(Ball p1, Ball p2, float h) {
        _Cone cone = new _Cone();

        cone.baseRadius = ((h - p1.radius) / h) * p1.radius;
        cone.height = h * (p1.radius / (p1.radius - p2.radius)) - p1.radius;
        Debug.Log(" -- [cone] h: " + cone.height + "; b: " + cone.baseRadius);

        return cone;
    }

    private static Ball[] _GenerateIntrakeys(Ball first, Ball last) {
        Vector3 segment = last.pos - first.pos;
        Vector3 segmentNormal = segment.normalized;
        float segmentLength = segment.magnitude;
        Debug.Log(" -- [SEGMENT] r1: " + first.radius + "; r2: " + last.radius + "; h: " + segmentLength);

        // calculate how many spheres fit in a sliced cone between two GoopKeys
        _Cone cone = _ApproximateCone(first, last, segmentLength);

        // calculate intraspheres
        float[] intraspheres;
        if(Mathf.Abs(first.radius - last.radius) > _EPSILON)
            intraspheres = _FitSpheresInCone(cone, last.radius, segmentLength);
        else
            intraspheres = _FitSpheresInCylinder(last.radius, segmentLength);

        // put intraspheres along segment
        Ball[] intrakeys = new Ball[intraspheres.Length + 2];
        Debug.Log("Intrasphere count: " + intraspheres.Length);

        // add first and last keys
        intrakeys[0].pos = segmentNormal * (first.radius); // TODO: make position extension interpolable
        intrakeys[0].radius = first.radius;
        intrakeys[0].wiggle = 0;

        intrakeys[intrakeys.Length - 1].pos = segmentNormal * (last.radius); // TODO: make position extension interpolable
        intrakeys[intrakeys.Length - 1].radius = last.radius;
        intrakeys[intrakeys.Length - 1].wiggle = 0;
        

        // add intraspheres
        Ball prevKey = first;
        for (int i = 0; i <intraspheres.Length; i++) {
            // position: previous position + direction normal * (previous radius + current diameter)
            intrakeys[i + 1].pos = prevKey.pos + segmentNormal * (prevKey.radius + intraspheres[i]); // TODO: make position extension interpolable
            intrakeys[i + 1].radius = intraspheres[i];
            intrakeys[i + 1].wiggle = 0;

            prevKey = intrakeys[i + 1];
        }

        return intrakeys;
    }

    /*
     *  Math utils
     */
    private struct _Cone {
        public float height;
        public float baseRadius;
    }

    private static float[] _FitSpheresInCone(_Cone cone, float minRadius, float heightCutoff) {
        List<float> spheres = new List<float>();
        float heightCounter = 0f;

        while(spheres.Count <= 100) {
            float radius = _FitSphereInCone(cone);
            Debug.Log(" -- [fit-sphere] r: " + radius);
            if(radius < minRadius) break;

            spheres.Add(radius);
            heightCounter += radius * 2f;

            if(heightCounter >= heightCutoff) break;      // TODO: comment thiss

            // cut cone
            cone = _CutConeByHeightFromBase(cone, radius);
        }

        return spheres.ToArray();
    }

    private static float[] _FitSpheresInCylinder(float radius, float length) {
        int count = Mathf.FloorToInt(length / (radius * 2f));
        float[] spheres = new float[count];

        for(int i = 0; i < count; i++) {
            spheres[i] = radius;
        }

        return spheres;
    } 

    private static _Cone _CutConeByHeightFromBase(_Cone oldCone, float radius) {
            _Cone newCone = new _Cone();
            newCone.height = oldCone.height - radius * 2f;
            newCone.baseRadius = (newCone.height / oldCone.height) * oldCone.baseRadius;

            Debug.Log(" -- [cut-cone] h: " + newCone.height);

            return newCone;
    }

    /// Find radius of sphere at the base of the cone
    /// (source: http://mathcentral.uregina.ca/QQ/database/QQ.09.07/s/juan1.html)
    private static float _FitSphereInCone(_Cone cone) {
        float radius = 0f;
        float hypothenuse = Mathf.Sqrt(cone.baseRadius*cone.baseRadius + cone.height*cone.height);

        //radius = (cone.height * cone.baseRadius) / hypothenuse;
        //radius /= 1f + (cone.baseRadius / hypothenuse);
        radius = cone.height / ( 1 + hypothenuse / cone.baseRadius);

        return radius;
    }
}
