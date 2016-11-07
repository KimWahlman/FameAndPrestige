using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragAndDrop : MonoBehaviour
{
    private bool dragging = false;
    private float distance;
    private Card draggedCard;

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        
        draggedCard = gameObject.GetComponent<Card>();

        if (draggedCard.ownerID == Player.idPlayer)
        {
            dragging = true;
            draggedCard.isBeingDragged = true;
            draggedCard.putInFront(true);
        }
        else
        {
            dragging = false;
        }
    }

    void OnMouseUp()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.name == "PlayableZone")
            {
                PlayableZone pz = hit.transform.gameObject.GetComponent<PlayableZone>();
                draggedCard.transform.position = pz.getSlot();
                pz.addCard();

                draggedCard.useCard();
            }
            else
            {
                draggedCard.returnBackToHand();
            }
        }

        dragging = false;
        draggedCard.isBeingDragged = false;
        draggedCard.putInFront(false);
    }


    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector3(rayPoint.x, rayPoint.y, this.transform.position.z);
        }
    }
}
