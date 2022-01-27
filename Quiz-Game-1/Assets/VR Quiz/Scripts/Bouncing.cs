using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bouncing : MonoBehaviour
{
    // Start is called before the first frame update

    //variables for the coin movement
    public float rotSpeed;
    public float w;
    public float A;

    //initial position when created
    Vector3 initialPos;

    //this is to know if was triggered
    bool triggered = false;

    //this is the text for the coins
    Text coinTxt;


    //initial rotation
    Quaternion rotO;

    void Start()
    {
        initialPos =transform.position;
        rotO = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (triggered == false)
        {
            transform.rotation = Quaternion.Euler(rotO.eulerAngles.x, rotO.eulerAngles.y + rotSpeed * Time.fixedTime, rotO.eulerAngles.z);

            transform.position = initialPos + new Vector3(0, A * Mathf.Sin(w * Time.fixedTime), 0);
        }
    }


   

    //moving up effect for the coin
    public IEnumerator moveUp()
    {
        float elapsed = 0;

        while (elapsed < 0.5f)
        {
            transform.position +=  new Vector3(0,0.3f, 0);

            transform.localScale *=1 / 1.02f;

            elapsed += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
        coinTxt.text = ""+( int.Parse(coinTxt.text) + 10);
    }
}
