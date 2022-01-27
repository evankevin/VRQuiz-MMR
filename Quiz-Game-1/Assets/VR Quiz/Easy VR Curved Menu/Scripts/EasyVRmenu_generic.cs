using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EasyVRmenu_generic : MonoBehaviour
{

    // Use this for initialization

    //these are the values needed for curvature
    [Header("A value and B value are the sizes of the elipse")]
    [Range(0.1f, 10)]
    public float a = 4.04f;
    [Range(-10, 10)]
    public float b = 6.8f;
    public float width = 800;
    public float height = 480;
    public float Xdiv;
    public float angle;

    // these are the values used for diferential squares
    public int numberOfRectangles = 20;

    //this is the pathcontainer in which objects will appear

    [Header("These variables are used by the editor do not change them")]
    public Transform pathcontainer;

    public Material matRectangles;

    //this is the camera render
    public GameObject camRender;

    //instance of camRender
    public GameObject InstanceCamRender;

    // to prevent multiple creations of camrenders
    public bool flagCreated =false;

    //these are the individual meshes
    Mesh[] meshes;

    // this is where the mesh is created
    public MeshFilter mshFiltOutput;

    LayerMask standardMask;
    LayerMask curvedMask;

    //the main camera of the game
    public GameObject mainCam;

    void Start()
    {
        standardMask = ~0;
        curvedMask= ~(1 << 9);

        //CHECK FLAGS
        if (flagCreated)
        {
            return;
        }

        //instantiate camera render in main camera position if exists, else in (0,0,0);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        //prepare masks for the camera and the layers for the canvas
        mainCam.GetComponent<Camera>().cullingMask = curvedMask;

        SetLayerRecursively(gameObject, 9);
        

        //the camera render used for the curved menu
        InstanceCamRender = Instantiate(camRender, null);
        if (mainCam != null)
        {
            InstanceCamRender.transform.position = mainCam.transform.position;
            InstanceCamRender.transform.rotation = mainCam.transform.rotation;
        }
        else
        {
            InstanceCamRender.transform.position = new Vector3(0, 0, 0);
            InstanceCamRender.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        flagCreated = true;

        Debug.Log("ADDED CURVED MENU");


        //SET THE MESHFILTER
        pathcontainer = InstanceCamRender.transform.GetChild(0).transform;

        mshFiltOutput = pathcontainer.GetComponent<MeshFilter>();


        
        

        //draw curved menu
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
            GameObject[] gos =new GameObject[pathcontainer.childCount];

            //get all the children
            for (int ii = 0; ii < pathcontainer.childCount; ii++)
            {
                gos[ii]=pathcontainer.GetChild(ii).gameObject;
            }

            //destroy all the children
            for (int ii = 0; ii < gos.Length; ii++)
            {
                GameObject.DestroyImmediate(gos[ii]);
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

    public void OnDestroy()
    {

        //prepare masks for the camera and the layers for the canvas
        if (mainCam != null)
        {
            mainCam.GetComponent<Camera>().cullingMask = standardMask;
        }
        else
        {
            return;
        }

        SetLayerRecursively(gameObject, 0);


        destroyGeometry();

        if (InstanceCamRender != null)
        {
            GameObject.DestroyImmediate(InstanceCamRender);
        }

        Debug.Log("DESTROYED CURVED MENU");
    }


    //this is to set the layers recursively
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
