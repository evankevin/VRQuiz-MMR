using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(EasyVRmenu_generic))]
public class EasyVrmenu_generic_editor : Editor
{

    float a = 4.04f, b = 6.8f;
    float a_prev, b_prev;

    public GameObject InstanceCamRender;
    EasyVRmenu_generic myScript;

    void Awake()
    {
        // Setup the SerializedProperties.
        myScript = (EasyVRmenu_generic)target;

               
    }

    

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();


        if (GUILayout.Button("Generate"))
        {
            if (myScript.mshFiltOutput != null && myScript.pathcontainer != null)
            {
                myScript.drawGeometry();
            }
        }

        if (GUILayout.Button("Destroy"))
        {
            if (myScript.mshFiltOutput != null && myScript.pathcontainer != null)
            {
                myScript.destroyGeometry();
            }
        }



        a = myScript.a;
        b = myScript.b;

        if ((a != a_prev || b != b_prev) && (myScript.mshFiltOutput != null && myScript.pathcontainer != null))
        {
            myScript.destroyGeometry();
            myScript.drawGeometry();
        }

        a_prev = a;
        b_prev = b;


    }


    
}