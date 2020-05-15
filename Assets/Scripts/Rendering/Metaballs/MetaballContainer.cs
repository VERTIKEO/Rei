using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetaballContainer : MonoBehaviour {
    [HideInInspector] public float safeZone = 0.2f;
    [HideInInspector] public float resolution = 0.04f;
    [HideInInspector] public float threshold = 1f;
    public float smoothingAngle = 90f;

    [Header("Acceleration")]
    [HideInInspector] public bool useGpu = true;
    public ComputeShader computeShader;

    private CubeGrid grid;

    [HideInInspector] public Ball[] metaballs;

    public void Start() {
        for(int i = 0; i < metaballs.Length; i++) {
            //metaballs[i].pos = metaballs[i].pos - this.transform.localPosition;
            metaballs[i].pos = new Vector3(
                metaballs[i].pos.x / this.transform.localScale.x,
                metaballs[i].pos.y / this.transform.localScale.y,
                metaballs[i].pos.z / this.transform.localScale.z
            );

        }

        this.grid = new CubeGrid(this.gameObject, this.resolution, this.threshold, this.computeShader, metaballs, this.useGpu);
    }

    public void Update() {
        this.grid.evaluateAll();

        Mesh mesh = GetMesh();

        mesh.Clear();
        mesh.vertices = this.grid.vertices.ToArray();
        mesh.triangles = this.grid.getTriangles();

        if(smoothingAngle > 0f) {
            NormalSolver.RecalculateNormals(mesh, smoothingAngle);
            //mesh.Optimize();
        }
    }

    // public void Reconfigure() {
    //     Debug.Log("reconfigured CubeGrid");
    //     grid.Reconfigure(resolution, threshold);
    // }

    private Mesh GetMesh() {
        MeshFilter filter = this.GetComponent<MeshFilter>();

        if(Application.isEditor) {
            if(filter.sharedMesh == null)
                filter.sharedMesh = new Mesh();

            return filter.sharedMesh;
        } else {
            if(filter.mesh == null)
                filter.mesh = new Mesh();

            return filter.mesh;
        }
    }

    public void OnDestroy() {
        GetMesh().Clear(false);
        this.grid.OnDestroy();
    }
}