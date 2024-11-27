using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;

public class RailScript : MonoBehaviour
{
    public bool normalDir;
    public SplineContainer railSpline;
    public float totalSplineLength;

    [Header("Rail Visuals")]
    public Material railMaterial;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        railSpline = GetComponent<SplineContainer>();
        totalSplineLength = railSpline.CalculateLength();
        SetupMeshComponents();
        GenerateRailMesh();
    }

    private void SetupMeshComponents()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.material = railMaterial;
    }

    private void GenerateRailMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        Vector3[] splinePoints = new Vector3[50];
        Vector3[] splineTangents = new Vector3[50];

        for (int i = 0; i < 50; i++)
        {
            Vector3 center = splinePoints[i];

            for (int j = 0; j < 8; j++)
            {
                vertices.Add(center);
            }
        }

        for (int i = 0; i < 49; i++)
        {
            int ringStartIdx = i * 8;
            int nextRingStartIdx = (i + 1) * 8;

            for (int j = 0; j < 8; j++)
            {
                int currentIdx = ringStartIdx + j;
                int nextIdx = ringStartIdx + (j + 1) % 8;
                int nextRingIdx = nextRingStartIdx + j;
                int nextRingNextIdx = nextRingStartIdx + (j + 1) % 8;

                triangles.Add(currentIdx);
                triangles.Add(nextRingIdx);
                triangles.Add(nextIdx);
                triangles.Add(nextIdx);
                triangles.Add(nextRingIdx);
                triangles.Add(nextRingNextIdx);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        meshFilter.mesh = mesh;
    }

    public Vector3 LocalToWorldConversion(float3 localPoint)
    {
        Vector3 worldPos = transform.TransformPoint(localPoint);
        return worldPos;
    }

    public float3 WorldToLocalConversion(Vector3 worldPoint)
    {
        float3 localPos = transform.InverseTransformPoint(worldPoint);
        return localPos;
    }

    public float CalculateTargetRailPoint(Vector3 playerPos, out Vector3 worldPosOnSpline)
    {
        float3 nearestPoint;
        float time;
        SplineUtility.GetNearestPoint(railSpline.Spline, WorldToLocalConversion(playerPos), out nearestPoint, out time);
        worldPosOnSpline = LocalToWorldConversion(nearestPoint);
        return time;
    }

public void CalculateDirection(float3 railForward, Vector3 playerForward)
{
    Vector3 railForwardFlat = new Vector3(railForward.x, 0, railForward.z).normalized;
    Vector3 playerForwardFlat = new Vector3(playerForward.x, 0, playerForward.z).normalized;

    float angle = Vector3.Angle(railForwardFlat, playerForwardFlat);
    
    normalDir = angle <= 90f;
}
}