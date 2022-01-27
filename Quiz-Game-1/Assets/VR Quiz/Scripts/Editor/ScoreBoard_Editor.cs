using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(ScoreBoard))]
public class ScoreBoard_Editor : Editor
{

    void OnEnable()
    {
        // Setup the SerializedProperties.


    }


    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        ScoreBoard myScript = (ScoreBoard)target;

        if (GUILayout.Button("Reorder Array"))
        {
            myScript.udpateScores();
        }

       


    }

}