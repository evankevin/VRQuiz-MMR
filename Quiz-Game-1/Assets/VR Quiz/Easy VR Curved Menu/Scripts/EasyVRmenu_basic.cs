using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EasyVRmenu_basic : MonoBehaviour {

    // Use this for initialization

    //these are the values needed for curvature
    [Range(0.1f, 5)]
    public float a = 2;
    [Range(-5, 5)]
    public float b = 2;
    public float width = 800;
    public float height = 480;
    public float Xdiv;

    // these are the values used for diferential squares
    public int numberOfRectangles=20;


    //this is the pathcontainer in which objects will appear
    public Transform pathcontainer;


    //these are the material and textures used
    public Material matRectangles, matRectangles_over, material_click;


    //these are the individual meshes
    Mesh[] meshes;

    // this is where the mesh is created
    public MeshFilter mshFiltOutput;

    void Start ()
    {
        drawGeometry();    

    }
	
	
    // Update is called once per frame
	void Update ()
    {
        
    }

    public void destroyGeometry()
    {
        for (int ii=0;ii<pathcontainer.childCount;ii++)
        { GameObject.DestroyImmediate(pathcontainer.GetChild(ii).gameObject);

            GameObject.FindGameObjectWithTag("meshTest").GetComponent<MeshFilter>().mesh = null;
            GameObject.FindGameObjectWithTag("meshTest").GetComponent<MeshCollider>().sharedMesh = null;
        }
        
        
    }


    public void drawGeometry()
    {
        // scale the canvas to the size
        width = gameObject.GetComponent<RectTransform>().sizeDelta[0];
        height = gameObject.GetComponent<RectTransform>().sizeDelta[1];


       
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
            child = new GameObject("DiffRect"+ii);
            child.transform.parent = pathcontainer;
            tempMeshF = child.gameObject.AddComponent<MeshFilter>();
            meshRender = child.gameObject.AddComponent<MeshRenderer>();
            meshRender.material = matRectangles;
            meshRender.enabled = false;

            // four points of the mesh            
                float z1 = b / a * Mathf.Sqrt(Mathf.Pow(a, 2) - Mathf.Pow(ii * Xdiv - width / 2, 2)) - b;
                float z2 = b / a * Mathf.Sqrt(Mathf.Pow(a, 2) - Mathf.Pow((ii + 1) * Xdiv - width / 2, 2)) - b;
            
            points[0] = transform.position+new Vector3(-width/2,-height/2,0)+new Vector3(ii * Xdiv, 0,z1);
            points[1] = transform.position + new Vector3(-width / 2, -height / 2, 0) + new Vector3((ii+1) * Xdiv, 0, z2);
            points[3] = transform.position + new Vector3(-width / 2, height / 2, 0) + new Vector3((ii+1) * Xdiv, 0, z2);
            points[2] = transform.position + new Vector3(-width / 2, height / 2, 0) + new Vector3(ii * Xdiv, 0, z1);

           

            // create quads
            meshes[ii]= createGeometry(points[0], points[1], points[2], points[3], tempMeshF, ii*Xdiv/2, (ii+1)*Xdiv/2);

            TBC[ii].mesh=meshes[ii];
            TBC[ii].transform = child.transform.localToWorldMatrix;

        }

        // combine quads
        Mesh meshOut = new Mesh();
        meshOut.CombineMeshes(TBC,true);

        mshFiltOutput.mesh = meshOut;
        mshFiltOutput.transform.GetComponent<MeshCollider>().sharedMesh = meshOut;



        // the new object has the same events that the ones set on the initial object

        //click event
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { MyFunction_CLICK (mshFiltOutput.gameObject); });
        mshFiltOutput.GetComponent<EventTrigger>().triggers.Add(entry);

        //enter event
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { MyFunction_ENTER(mshFiltOutput.gameObject); });
        mshFiltOutput.GetComponent<EventTrigger>().triggers.Add(entry);

        //quit event
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { MyFunction_EXIT(mshFiltOutput.gameObject); });
        mshFiltOutput.GetComponent<EventTrigger>().triggers.Add(entry);

    }


    //THESE ARE THE FUNCTIONS CALLED 
    public void MyFunction_ENTER(GameObject go)
    {
        Debug.Log("ENTER " + go.name);
        go.GetComponent<MeshRenderer>().material = matRectangles_over;
    }

    public void MyFunction_EXIT(GameObject go)
    {
        Debug.Log("EXIT: " + go.name);
        go.GetComponent<MeshRenderer>().material = matRectangles;
    }

    public void MyFunction_CLICK(GameObject go)
    {
        Debug.Log("CLICK: " + go.name);
        go.GetComponent<MeshRenderer>().material = material_click;
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
