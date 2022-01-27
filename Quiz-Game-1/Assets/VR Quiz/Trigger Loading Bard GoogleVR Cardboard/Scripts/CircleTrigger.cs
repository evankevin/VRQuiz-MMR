using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CircleTrigger : MonoBehaviour
{

    //used for colors
    public Color start;
    public Color end;
    Color current;

    // used to refer to the circle trigger pointer
	public Image CircleImage;

    // this allows to know which gameobject is selected by the user
	public GameObject selectedGameObject;

    //pointer event
    public PointerEventData pointer;
    public PointerEventData lookData;
	private bool _guiRaycastHit;

    // the duration of focus trigger
    public float DurationTime;
    float value;

    // store the corrutine process to stop it in the future
    Coroutine corr;

    void Start()
    {

        //initial image conditions
        CircleImage.enabled = false;
        CircleImage.type = Image.Type.Filled;
        CircleImage.fillMethod = Image.FillMethod.Radial360;
        CircleImage.fillOrigin = 0;
        CircleImage.fillAmount = 0;
        
		
    }

    //main script that is called by the "pointer enter"
    IEnumerator loadingBar()
    {
        //create pointer event
        pointer = new PointerEventData(EventSystem.current);

        float instTime = Time.fixedTime;

        while (Time.fixedTime < DurationTime + instTime)
        {
            // set the position of the canvas to the gaze:
            value = (Time.fixedTime - instTime) / DurationTime;

            //Debug.Log(value);

            current = Color.Lerp(start, end, value + 0.001f);
            CircleImage.color = current;

            CircleImage.fillAmount = value;
            yield return null;
        }

        ExecuteEvents.Execute(selectedGameObject, pointer, ExecuteEvents.pointerClickHandler);
        undoClick();

    }



    
    public void prepareToClick()
    {

		CircleImage.enabled=true;

        Vector3 start = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(start, direction, out hit))
        {
           selectedGameObject = hit.collider.gameObject;

        }

        corr = StartCoroutine(loadingBar());


    }

	
	
    
	public void undoClick()
	{
        CircleImage.enabled = false;

        if (corr!=null)
        {

            StopCoroutine(corr);
           
        }
        

		
		
	}

    
  

     
}
