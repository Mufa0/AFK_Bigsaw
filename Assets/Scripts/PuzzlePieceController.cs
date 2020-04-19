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
    private Renderer renderer;

    public GameObject spawnPoint;
    public float horizontalDistance = 1f;
    public float verticalDistance = 1f;
    private void Start()
    {

        if (EventManager.activeEvent == null)
        {
            EventManager.activeEvent = new ActiveEvent();
        }
        EventManager.activeEvent.AddListener(SetActive);
        
        renderer = this.GetComponent<Renderer>();


        Random.seed = GetInstanceID() * System.DateTime.Now.Millisecond;
        Vector2 dist = Random.insideUnitCircle;
        Vector3 endPosition = new Vector3(dist.x * horizontalDistance, dist.y * verticalDistance,  0f);
        this.transform.position = spawnPoint.transform.position;
        this.transform.localPosition = this.transform.localPosition + endPosition;
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
            ChangeMaterial(selectedOutlineMaterial, outlineMaterial.name);
            EventManager.activeEvent.Invoke(this.transform.name);
        }
        
    }

    public void OnMouseDrag()
    {
        if (movable && Input.touchCount <= 1)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCord));
            this.transform.position = Vector2.Lerp(this.transform.position, mousePosition + offset, speed);
        }


    }

    private void SetActive(string name)
    {
       
        if (!this.transform.name.Equals(name) && this.active)
        {
            this.active = false;
            ChangeMaterial(outlineMaterial, selectedOutlineMaterial.name);
            
        } 
    }
    //TODO: Change this to avoid errors
    private void ChangeMaterial(Material mat, string name)
    {
        if (renderer.materials[0].name.Equals(name))
        {
            renderer.materials[0] = mat;
        }
        else
        {
            renderer.materials[1] = mat;
        }
    }
    
}
