using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonResize : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 oScale, objectiveScale;
    public Color overColor, outColor;
    Image imageButton;
    RectTransform recT;

    Vector2 oSize, objectiveSize;

    void Start()
    {
        recT =GetComponent<RectTransform>();

        //oScale = transform.localScale;
        //objectiveScale = oScale;

        oSize = recT.sizeDelta;
        objectiveSize = oSize;

        imageButton = GetComponent<Image>();
        imageButton.color = outColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.localScale = Vector3.Lerp(transform.localScale, objectiveScale,0.1f);
        recT.sizeDelta = Vector2.Lerp(recT.sizeDelta, objectiveSize, 0.1f);
    }

    public void overMouse()
    {
        //objectiveScale = new Vector3(0.9F,0.9F,0.9F);

        objectiveSize = oSize*0.9f;

        imageButton.color = overColor;
    }

    public void outMouse()
    {
        //objectiveScale = oScale;

        objectiveSize = oSize;

        imageButton.color = outColor;
    }
}
