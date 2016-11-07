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
        }
        else
        {
            dragging = false;
        }
    }

    void OnMouseUp()
    {
        dragging = false;
        draggedCard.isBeingDragged = false;
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
