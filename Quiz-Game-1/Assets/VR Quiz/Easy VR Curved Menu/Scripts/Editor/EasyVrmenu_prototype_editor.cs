using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(EasyVRmenu_prototype))]
public class EasyVrmenu_prototype_editor : Editor
{

    float a = 1, b = 1;
    float a_prev, b_prev;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        
    
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Label("A value and B value are the sizes of the elipse");

        DrawDefaultInspector();

        EasyVRmenu_prototype myScript = (EasyVRmenu_prototype)target;

        if (GUILayout.Button("Generate"))
        {
            if (myScript.mshFiltOutput!=null && myScript.pathcontainer!=null)
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

        if((a!=a_prev||b!=b_prev) && (myScript.mshFiltOutput != null && myScript.pathcontainer != null))
        {
            myScript.destroyGeometry();
            myScript.drawGeometry();
        }

        a_prev = a;
        b_prev = b;
                        

    }

}