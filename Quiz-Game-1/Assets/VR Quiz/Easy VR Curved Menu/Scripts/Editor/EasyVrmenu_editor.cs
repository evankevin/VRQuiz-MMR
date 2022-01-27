using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(EasyVRmenu_basic))]
public class EasyVrmenu_editor : Editor
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

        EasyVRmenu_basic myScript = (EasyVRmenu_basic)target;

        if (GUILayout.Button("Generate"))
        {
            myScript.drawGeometry();
        }

        if (GUILayout.Button("Destroy"))
        {
            myScript.destroyGeometry();
        }

        /*GUILayout.Label("A value of the elipse");
        a = GUILayout.HorizontalSlider( a, -5.0f, 5.0f);

        GUILayout.Label("B value of the elipse");
        b = GUILayout.HorizontalSlider(b, 0f, 5.0f);*/


        a = myScript.a;
        b = myScript.b;

        if(a!=a_prev||b!=b_prev)
        {
            myScript.destroyGeometry();
            myScript.drawGeometry();
        }

        a_prev = a;
        b_prev = b;
                        

    }

}