using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Used to determine the player position in the scoreboard")]
    public GameObject[] playerLines;

    public SortedDictionary<float, int> scBoard;
    public Transform[] tfsRef;

    float elapsed;
    

    [Header("Where scores are set")]
    public Text[] scoresTxt;

    void Start()
    {
        //initialize
        scBoard = new SortedDictionary<float, int>();

        elapsed = 0;

        //set scores and re-order array
        udpateScores();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i = 0;

        foreach (var kvp in scBoard)
        {
            playerLines[kvp.Value].transform.position = Vector3.Lerp(playerLines[kvp.Value].transform.position, tfsRef[i].position, 0.2f);
            i++;
        }

    }

    public void udpateScores()
    {
        scBoard.Clear();
        
        // print all element of array 
        for (int i = 0; i < 4; i++)
        {
            try
            {
                scBoard.Add(float.Parse(scoresTxt[i].text), i);
            }
            catch
            {
                scBoard.Add(float.Parse(scoresTxt[i].text)+ 0.01f * i, i);
            }
        }

        //show scores if wanted
        /*
        foreach (var kvp in scBoard)
        {
            Debug.Log("Key = " +kvp.Key + " Value = "+ kvp.Value);
        }*/

    }



   
}


