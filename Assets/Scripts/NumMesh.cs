using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class NumMesh : MonoBehaviour
{
    private int _number;
    public int number
    {
        get => _number;
        set {
            _number = value;
            UpdateUv();
        }
    }

    public int AtlasCount;
    Vector2[] UvPoints =
        {
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(0,0),
            new Vector2(1,0)

        };
    Mesh mesh;
    void OnEnable()
    {
        mesh = new Mesh();
        mesh.name = "TestMesh";
        mesh.vertices = new Vector3[] {
            new Vector3(-.5f,.5f,0),
            new Vector3(.5f,.5f,0f),
            new Vector3(-.5f,-.5f,0),
            new Vector3(.5f,-.5f,0)
        };

        mesh.normals = new Vector3[]
        {
            new Vector3(-1,0,0),
            new Vector3(-1,0,0),
            new Vector3(-1,0,0),
            new Vector3(-1,0,0),
        };
        
        mesh.triangles = new int[] {
            0,1,2,3,2,1
        };

        mesh.uv = UvPoints;

        MeshFilter filter;
        if(TryGetComponent<MeshFilter>(out filter)) filter.mesh = mesh;

        UpdateUv();
    }

    void UpdateUv()
    {
        Vector2[] TransformedUVs = {
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0)
        };

        for(int i = 0; i < 4; i++)
        {
            TransformedUVs[i].x = (UvPoints[i].x + (float)_number)  / AtlasCount;
            TransformedUVs[i].y = UvPoints[i].y;
        }
        if(mesh) mesh.uv = TransformedUVs;
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireMesh(mesh, transform.position,transform.rotation, transform.localScale); 
    }


}
