using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class EasyVRmenu_prototype : MonoBehaviour
{

    // Use this for initialization

    //these are the values needed for curvature
    [Range(0.1f,10)]
    public float a = 3.57f;
    [Range(-10, 10)]
    public float b = 5.2f;
    public float width = 800;
    public float height = 480;
    public float Xdiv;

    // these are the values used for diferential squares
    public int numberOfRectangles = 20;

    //this is the pathcontainer in which objects will appear
    public Transform pathcontainer;

    public Material matRectangles;


    //these are the individual meshes
    Mesh[] meshes;

    // this is where the mesh is created
    public MeshFilter mshFiltOutput;

    void Start()
    {
        GameObject objToSpawn;
       
        //spawn object
        objToSpawn = new GameObject("CurvedCanvas");

        /*pathcontainer = objToSpawn.transform;
        mshFiltOutput=objToSpawn.AddComponent<MeshFilter>();
        objToSpawn.AddComponent<MeshCollider>();
        */

        drawGeometry();

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void destroyGeometry()
    {
        if (pathcontainer != null)
        {
            for (int ii = 0; ii < pathcontainer.childCount; ii++)
            {

                GameObject.DestroyImmediate(pathcontainer.GetChild(ii).gameObject);

            }
        }


    }


    public void drawGeometry()
    {
        // scale the canvas to the size
        width = gameObject.GetComponent<RectTransform>().sizeDelta[0] * transform.localScale.x;
        height = gameObject.GetComponent<RectTransform>().sizeDelta[1] * transform.localScale.y;



        GameObject child;

        MeshFilter tempMeshF;
        MeshRenderer meshRender;

        Xdiv = (width / numberOfRectangles);

        Vector3[] points = new Vector3[numberOfRectangles];

        meshes = new Mesh[numberOfRectangles];
        CombineInstance[] TBC = new CombineInstance[meshes.Length];

        // top part of the geometry
        for (int ii = 0; ii < numberOfRectangles; ii++)
        {
            //creting the different rectangles
            child = new GameObject("DiffRect" + ii);
            child.transform.parent = pathcontainer;
            tempMeshF = child.gameObject.AddComponent<MeshFilter>();
            meshRender = child.gameObject.AddComponent<MeshRenderer>();
            meshRender.material = matRectangles;
            meshRender.enabled = true;

            // four points of the mesh            
            float z1 = b / a * Mathf.Sqrt(Mathf.Pow(a, 2) - Mathf.Pow(ii * Xdiv - width / 2, 2)) - b;
            float z2 = b / a * Mathf.Sqrt(Mathf.Pow(a, 2) - Mathf.Pow((ii + 1) * Xdiv - width / 2, 2)) - b;

            points[0] = transform.position + new Vector3(-width / 2, -height / 2, 0) + new Vector3(ii * Xdiv, 0, z1);
            points[1] = transform.position + new Vector3(-width / 2, -height / 2, 0) + new Vector3((ii + 1) * Xdiv, 0, z2);
            points[3] = transform.position + new Vector3(-width / 2, height / 2, 0) + new Vector3((ii + 1) * Xdiv, 0, z2);
            points[2] = transform.position + new Vector3(-width / 2, height / 2, 0) + new Vector3(ii * Xdiv, 0, z1);



            // create quads
            meshes[ii] = createGeometry(points[0], points[1], points[2], points[3], tempMeshF, ii * Xdiv / width, (ii + 1) * Xdiv / width);

            TBC[ii].mesh = meshes[ii];
            TBC[ii].transform = child.transform.localToWorldMatrix;

        }

        // combine quads
        Mesh meshOut = new Mesh();
        meshOut.CombineMeshes(TBC, true);

        mshFiltOutput.mesh = meshOut;
        //mshFiltOutput.transform.GetComponent<MeshCollider>().sharedMesh = meshOut;



       
    }


    Mesh createGeometry(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, MeshFilter mf, float UX_0, float UX_1)
    {

        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        //vertices
        Vector3[] vertices = new Vector3[4];

        /*
		             2     3

		             0     1

		*/
        vertices[0] = p1;
        vertices[1] = p2;
        vertices[2] = p3;
        vertices[3] = p4;



        // normal vector
        Vector3[] normals = new Vector3[4];
        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;



        // triangles indices
        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;
        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        // texture uv
        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(UX_0, 0);
        uv[1] = new Vector2(UX_1, 0);
        uv[2] = new Vector2(UX_0, 1);
        uv[3] = new Vector2(UX_1, 1);

        /*
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);
        */


        mesh.vertices = vertices;
        mesh.triangles = tri;
        mesh.normals = normals;
        mesh.uv = uv;

        return mesh;
    }
}
