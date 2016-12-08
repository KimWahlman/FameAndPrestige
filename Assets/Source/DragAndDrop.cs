using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragAndDrop : MonoBehaviour
{
    private bool dragging = false;
    private float distance;
    private Card draggedCard;

    private GameManager gameManager;

    private PlayableZone pz;
	LayerMask l;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		l = this.gameObject.layer;
    }

    void OnMouseDrag()
    {
		draggedCard = gameObject.GetComponent<Card>();
		draggedCard.toShowPoint = false;
        grabCard();
    }
    void OnMouseUp()
    {
        releaseCard();
		draggedCard = gameObject.GetComponent<Card>();
		draggedCard.toShowPoint = true;
    }

    void Update()
    {
        //dragging system
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector3(rayPoint.x, rayPoint.y, -1.0f);
        }
    }

    void grabCard()
    {

        distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        draggedCard = gameObject.GetComponent<Card>();
        
        //if it's mine allow the player to drag it
        if (!draggedCard.hasBeenPlayed && draggedCard.isMine && gameManager.checkPlayerTurn())
        {
            dragging = true;
            draggedCard.isBeingDragged = true;
          	this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    //when the card is dropped
    void releaseCard()
    {

        if (!draggedCard.hasBeenPlayed && draggedCard.isMine && gameManager.checkPlayerTurn())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {       		
                //if the card is dropped on the zone
                if (hit.transform.gameObject.name == "PlayableZone" && !gameManager.checkStoredCard(draggedCard.id))
                {
                    //get the script of the zone (contain the slots position)
                    pz = hit.transform.gameObject.GetComponent<PlayableZone>();

                    //assign the free slot position to the card
                    draggedCard.PositionOnTheBoard(pz.getSlot());
                    draggedCard.isOnTheBoard = true;
                    
                    gameManager.storeCard(draggedCard.id);
					gameManager.checkEndTurnButton ();
                    gameManager.ReOrderPlayerHandAfterDrop(draggedCard.id);
                    
                    setDraggingFlag(false);
                    return;

                }
                else
                {
                   
                    if (!gameManager.myPlayer.cardsHeld.ContainsKey(draggedCard.id))
                    {
                        //return from PlayZone
                        gameManager.removeStoredCard(draggedCard.id);
                        gameManager.ReOrderPlayerHandAfterReturn(draggedCard.id, draggedCard);                        
                    }
                    else {
                        //return from voidZone
                        draggedCard.returnBackToHand();
                    }

                    
                    gameManager.checkEndTurnButton ();
                    setDraggingFlag(false);
                }                         
            }else
            {                
                CheckFrom();
                return;
            }
        }      
    }

    void CheckFrom() {
        if (!gameManager.myPlayer.cardsHeld.ContainsKey(draggedCard.id))
        {
            gameManager.removeStoredCard(draggedCard.id);
            gameManager.ReOrderPlayerHandAfterReturn(draggedCard.id, draggedCard);                 
            setDraggingFlag(false);
        }
        else
        {
            draggedCard.returnBackToHand();            
            setDraggingFlag(false);
        }
    }

    void setDraggingFlag(bool flag) {
        draggedCard.gameObject.layer = l;
        dragging = flag;
        draggedCard.isBeingDragged = flag;
        draggedCard.putInFront(flag);
    }


}
