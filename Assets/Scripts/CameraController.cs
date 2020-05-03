using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 4;
    public float zoomOutMax = 8;

    public float verticalClamp = 5f;
    public float horizontalClamp = 5f;
    
    private bool pan = true;
    public LayerMask mask;
    private void Start()
    {
        if(EventManager.activeEvent == null)
        {
            EventManager.activeEvent = new ActiveEvent();
        }
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
            Debug.Log("hover" + hit.collider.name);
            pan = false;
            ZoomSetup();
        }
        else
        {
            Debug.Log("No Hover!");
            ZoomSetup();
            PanSetup();
            
        }
        
        
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) && pan)
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 newPosition = Camera.main.transform.position += direction;
            Vector3 newPositionClamped = new Vector3(Mathf.Clamp(newPosition.x, -horizontalClamp, horizontalClamp), Mathf.Clamp(newPosition.y, -verticalClamp, verticalClamp/4), newPosition.z);
            Camera.main.transform.position = newPositionClamped;
        }
    }

    private void ZoomSetup()
    {
        if (Input.touchCount == 2)
        {
            pan = false;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            Zoom(difference * 0.01f);

        }
    }

    private void PanSetup()
    {
        if (Input.GetMouseButtonUp(0))
        {
            pan = true;
            EventManager.activeEvent.Invoke(this.name);
        }
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EventManager.activeEvent.Invoke(this.name);
        }
    }
    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

}
