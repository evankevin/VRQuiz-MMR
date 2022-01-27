using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTransition : MonoBehaviour
{

    public GameObject sphere;

    void Start()
    {
        StartCoroutine(sphereGrouth());
    }



    IEnumerator sphereGrouth()
    {
        for (float f = 0.5f; f <= 20; f += 0.05f)
        {
            sphere.transform.localScale = new Vector3(f, f, f);
            yield return new WaitForSeconds(0.01f);
        }
        sphere.SetActive(false);

    }
}
