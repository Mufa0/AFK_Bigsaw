using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PuzzlePieceController : MonoBehaviour
{

    private bool movable=true;
    private bool active;
    private float zCord;
    private Vector3 offset;

    public float snappingDistance=0.05f;
    public float speed = 0.2f;

    public Material outlineMaterial;
    public Material selectedOutlineMaterial;

    public GameObject spawnPoint;
    public float horizontalDistance = 4f;
    public float verticalDistance = 3f;

    public float scalingFactor = 0.5f;

    private Vector3 originalScale;



    private void Start()
    {

        originalScale = transform.localScale;

        if (EventManager.activeEvent == null)
        {
            EventManager.activeEvent = new ActiveEvent();
        }
        EventManager.activeEvent.AddListener(SetActive);
        


        //Setting up initial position and rotations
        Random.seed = GetInstanceID() * System.DateTime.Now.Millisecond;
        Vector2 dist = Random.insideUnitCircle;
        Vector3 endPosition = new Vector3(dist.x * horizontalDistance, dist.y * verticalDistance,  0f);
        this.transform.position = spawnPoint.transform.position;
        this.transform.localPosition = this.transform.localPosition + endPosition;
        if (GameController.rotation)
        {
            int rotationDegree = (int)Mathf.Floor(Random.Range(-4, 4));
            transform.Rotate(new Vector3(0,0,1)* rotationDegree*90);
        }
    }

    public void OnMouseUp()
    {
        if (movable && active)
        {
            
            //If rotation is matching rotation of placeholder
            if (this.transform.rotation.z == 0f)
            {
                Vector2 position = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
                
                if (Vector2.Distance(position, Vector2.zero) <= snappingDistance)
                {
                    this.transform.localPosition = new Vector3(0f, 0f, 0f);
                    movable = false;
                    active = false;
                }
                else
                {

                    applyScaleAndRotation();
                }
            }
            else
            {

                applyScaleAndRotation();
            }

        }

    }

    private void applyScaleAndRotation()
    {
        if (!transform.localScale.Equals(originalScale * (1 + scalingFactor)))
        {
            transform.localScale = originalScale * (1 + scalingFactor);
        }
        else
        {
            if (GameController.rotation)
            {
                transform.Rotate(new Vector3(0, 0, 1) * -90);
            }
            
        }

    }

    public void OnMouseDown()
    {
        
        if(Input.touchCount <= 1)
        {
            zCord = Camera.main.WorldToScreenPoint(this.transform.position).z;
            offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCord));
            active = true;
            EventManager.activeEvent.Invoke(this.transform.name);
        }
        
    }

    public void OnMouseDrag()
    {
        if (movable && Input.touchCount <= 1)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCord));
            Vector3 diff = (mousePosition + offset) - this.transform.position;
            if (diff.magnitude > 0)
            {
                //this.transform.position = Vector2.Lerp(this.transform.position, mousePosition + offset, speed);
                this.transform.position = mousePosition + offset;
                transform.localScale = originalScale;
            }

        }


    }

    private void SetActive(string name)
    {
       
        if (!this.transform.name.Equals(name) && this.active)
        {
            this.active = false;
            transform.localScale = originalScale;
            
        } 
    }
    
}
