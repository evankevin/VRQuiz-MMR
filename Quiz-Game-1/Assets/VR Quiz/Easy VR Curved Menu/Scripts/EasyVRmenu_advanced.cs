using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EasyVRmenu_advanced : MonoBehaviour
{

    // Use this for initialization

    //these are the values needed for curvature
    [Range(0.1f, 5)]
    public float a = 2;
    [Range(-5, 5)]
    public float b = 2;
    public float width = 800;
    public float height = 480;

    public float width2, height2;


    public float Xdiv;

    // these are the values used for diferential squares
    public int numberOfRectangles = 20;


    //this is the pathcontainer in which objects will appear
    public Transform pathcontainer, outputContainer;
    
    
    //these are the individual meshes
    Mesh[] meshes;


    //these are the materials
    Material[] matRectangles_over;
    Material[] matRectangles;
    Material[] matRectangles_click;


    GameObject[] objectsTOTransform;
    GameObject[] output_GO;


    void Start()
    {
        destroyGeometry();
        drawGeometry();

    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount-1 == outputContainer.childCount)
        {
            checkCurvedAndOriginal();
        }
    }

    public void destroyGeometry()
    {

        //destroy in the pathcointainer
        GameObject[] tempArray = new GameObject[pathcontainer.transform.childCount];

        
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = pathcontainer.transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
        }


        //destroy in the outputcontainer
        tempArray = new GameObject[outputContainer.transform.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = outputContainer.transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
        }

        // set the canvas objects to visible
        GameObject[] objectsTOTransform = new GameObject[gameObject.transform.childCount];

        for (int kk = 0; kk < objectsTOTransform.Length-1; kk++)
        {
            gameObject.GetComponent<Canvas>().enabled=true;
        }


        }


    public void drawGeometry()
    {

        // REPEAT THE PROCESS FOR ALL THE OBJECTS INSIDE THE CANVAS EXCEPT THE LAST TWO

        //destroy in the pathcointainer
        objectsTOTransform = new GameObject[gameObject.transform.childCount];

        //this will be the gameobjects generated in output
        output_GO = new GameObject[objectsTOTransform.Length-1];

        //resize the materials arrays
        matRectangles_over = new Material[objectsTOTransform.Length-1];
        matRectangles = new Material[objectsTOTransform.Length-1];
        matRectangles_click = new Material[objectsTOTransform.Length-1]; ;

        for (int kk = 0; kk < output_GO.Length; kk++)
        {
            
            //for delegate
            int kk2 = kk;

            objectsTOTransform[kk2] = gameObject.transform.GetChild(kk2).gameObject;

            

            Image tryImage = objectsTOTransform[kk2].GetComponent<Image>();
            Texture textureOfImage=new Texture2D(300, 150);


            width = gameObject.GetComponent<RectTransform>().sizeDelta[0];
            height = gameObject.GetComponent<RectTransform>().sizeDelta[1];



            if (tryImage!=null)
            {
                // scale the canvas to the size. these is the main parameter 
                width2 = tryImage.GetComponent<RectTransform>().sizeDelta[0];
                height2 = tryImage.GetComponent<RectTransform>().sizeDelta[1];

                //get the main texture of the object
                textureOfImage = tryImage.mainTexture;

            }

            
            GameObject child;

            MeshFilter tempMeshF;
            MeshRenderer meshRender;

            Xdiv = (width2 / numberOfRectangles);

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
                //meshRender.material = matRectangles;
                meshRender.enabled = false;

                // four points of the mesh  
                float x1 = objectsTOTransform[kk2].transform.position[0]+ ii * Xdiv - width2 / 2;
                float x2 = objectsTOTransform[kk2].transform.position[0]+(ii + 1) * Xdiv - width2 / 2;
                          
                float z1 = b / a * Mathf.Sqrt(Mathf.Pow(a, 2) - Mathf.Pow(x1, 2)) - b - 1e-2f* kk2;
                float z2 = b / a * Mathf.Sqrt(Mathf.Pow(a, 2) - Mathf.Pow(x2, 2)) - b - 1e-2f * kk2;

         
                points[0] = objectsTOTransform[kk2].transform.position + new Vector3(-width2 / 2, -height2 / 2, 0) + new Vector3(ii * Xdiv, 0, z1);
                points[1] = objectsTOTransform[kk2].transform.position + new Vector3(-width2 / 2, -height2 / 2, 0) + new Vector3((ii + 1) * Xdiv, 0, z2);
                points[3] = objectsTOTransform[kk2].transform.position + new Vector3(-width2 / 2, height2 / 2, 0) + new Vector3((ii + 1) * Xdiv, 0, z2);
                points[2] = objectsTOTransform[kk2].transform.position + new Vector3(-width2 / 2, height2 / 2, 0) + new Vector3(ii * Xdiv, 0, z1);



                // create quads
                meshes[ii] = createGeometry(points[0], points[1], points[2], points[3], tempMeshF, ii * Xdiv / width2, (ii + 1) * Xdiv / width2);

                TBC[ii].mesh = meshes[ii];
                TBC[ii].transform = child.transform.localToWorldMatrix;

            }

            // combine quads into an output gameobject 
            output_GO[kk2] = new GameObject(objectsTOTransform[kk2].name);
            MeshFilter mshFiltOutput = output_GO[kk2].AddComponent<MeshFilter>();
            MeshCollider mshColl = output_GO[kk2].AddComponent<MeshCollider>();
            MeshRenderer mshRendOut = output_GO[kk2].AddComponent<MeshRenderer>();
           
            output_GO[kk2].transform.parent = outputContainer.transform;

            //combine meshes
            Mesh meshOut = new Mesh();
            meshOut.CombineMeshes(TBC, true);

            mshFiltOutput.mesh = meshOut;


            if (objectsTOTransform[kk2].GetComponent<EventTrigger>() != null)
            {
                mshColl.sharedMesh = meshOut;
            }

            //destroy intermediate objects
            GameObject[] tempArray = new GameObject[pathcontainer.transform.childCount];

            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = pathcontainer.transform.GetChild(i).gameObject;
            }

            foreach (var childGo in tempArray)
            {
                DestroyImmediate(childGo);
            }


            //copy event triggers of the gameobject




            // CUSTOM MATERIALS
            // read the materials
            matRectangles_over[kk2] = new Material(Shader.Find("Unlit/Texture"));
            matRectangles_over[kk2].mainTexture = textureOfImage;
            matRectangles_over[kk2].color =  new Color(1, 1, 1, 1);

            matRectangles[kk2] = new Material(Shader.Find("Mobile/Particles/Multiply"));
            matRectangles[kk2].mainTexture = textureOfImage;
            matRectangles[kk2].color = new Color(1,1,1,0.5f);

            matRectangles_click[kk2] = new Material(Shader.Find("Mobile/Diffuse"));
            matRectangles_click[kk2].mainTexture = textureOfImage;
            matRectangles_click[kk2].color = new Color(1,0.5f, 0.5f, 1);

            Debug.Log("Generating object ind=" + kk2 + " name=" + output_GO[kk2].name);
            mshRendOut.material = matRectangles[kk2];


            // CUSTOM EVENT TRIGGERS

            if (objectsTOTransform[kk2].GetComponent<EventTrigger>()!=null)
            {
                output_GO[kk2].AddComponent<EventTrigger>();

                //store event triggers from object to curve
                EventTrigger tempTrig = output_GO[kk2].GetComponent<EventTrigger>();
                tempTrig.triggers= objectsTOTransform[kk2].GetComponent<EventTrigger>().triggers;


                //click event
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { MyFunction_CLICK(output_GO[kk2], matRectangles_click[kk2]); });
                mshFiltOutput.GetComponent<EventTrigger>().triggers.Add(entry);

                //enter event
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { MyFunction_ENTER(output_GO[kk2], matRectangles_over[kk2]); });
                mshFiltOutput.GetComponent<EventTrigger>().triggers.Add(entry);

                //quit event
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((eventData) => { MyFunction_EXIT(output_GO[kk2], matRectangles[kk2]); });
                mshFiltOutput.GetComponent<EventTrigger>().triggers.Add(entry);             

                
            }
                      
        }

        //set the canvas object to disable
        gameObject.GetComponent<Canvas>().enabled = false;
    }


    //THESE ARE THE FUNCTIONS CALLED 

    public void checkCurvedAndOriginal()
    {
        for (int kk = 0; kk < objectsTOTransform.Length - 1; kk++)
        {
            output_GO[kk].SetActive(objectsTOTransform[kk].activeSelf);
        }
    }


    public void MyFunction_ENTER(GameObject go, Material mat)
    {
        Debug.Log("enter");
        Debug.Log("ENTER " + go.name);
        go.GetComponent<MeshRenderer>().material = mat;
    }

    public void MyFunction_EXIT(GameObject go, Material mat)
    {
        Debug.Log("EXIT: " + go.name);
        go.GetComponent<MeshRenderer>().material = mat;
    }

    public void MyFunction_CLICK(GameObject go, Material mat)
    {
        Debug.Log("CLICK: " + go.name);
        go.GetComponent<MeshRenderer>().material = mat;
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

