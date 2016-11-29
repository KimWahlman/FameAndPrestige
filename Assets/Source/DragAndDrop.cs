using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragAndDrop : MonoBehaviour
{
    private bool dragging = false;
    private float distance;
    private Card draggedCard;

    private NetworkManager networkManager;
    private GameManager gameManager;

    private PlayableZone pz;
	LayerMask l;

    void Awake()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		l = this.gameObject.layer;
    }

    void OnMouseDown()
    {
        //Debug.Log("OnMouseDrag");
        grabCard();
    }
    void OnMouseUp()
    {
        releaseCard();
    }

    void Update()
    {
        //dragging system
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector3(rayPoint.x, rayPoint.y, this.transform.position.z);
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
            draggedCard.putInFront(true);
			this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    //when the card is dropped
    void releaseCard()
    {
        Debug.Log("draggedCard hasBeenPlayed:"+draggedCard.hasBeenPlayed+ draggedCard.isMine+ gameManager.checkPlayerTurn());
        if (!draggedCard.hasBeenPlayed && draggedCard.isMine && gameManager.checkPlayerTurn())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Debug.Log (ray);
			Debug.Log (Physics.Raycast (ray, out hit));
            if (Physics.Raycast(ray, out hit))
            {
				Debug.Log ("Ray Cast in:"+hit.collider.name);             		

                //if the card is dropped on the zone
                if (hit.transform.gameObject.name == "PlayableZone")
                {
                    Debug.Log("DROPPED INTO PLAYZONE");
                    //get the script of the zone (contain the slots position)
                    pz = hit.transform.gameObject.GetComponent<PlayableZone>();

                    //assign the free slot position to the card
                    draggedCard.PositionOnTheBoard(pz.getSlot());

                    //networkManager.SendPlayCard(draggedCard.ownerID, draggedCard.id);
                    gameManager.storeCard(draggedCard.id);
                    gameManager.ReOrderPlayerHandAfterDrop(draggedCard.id);

                    //(should request to the server if i can play)
                    //if yes, use the card
                    //else, put the card back
                    draggedCard.hasBeenPlayed = true;
                    setDraggingFlag(false);
                    return;

                    //use the dropped card 
                    //draggedCard.playCard();
                    //gameManager.ReOrderPlayerHand(draggedCard.ownerID, draggedCard.id);
                }
                else {
                    //this is for collider with other cards;
                    Debug.Log("collider with other cards");
                   
                    if (!gameManager.myPlayer.cardsHeld.ContainsKey(draggedCard.id))
                    {
                        //return from PlayZone
                        draggedCard.hasBeenPlayed = false;
                        gameManager.removeStoredCard(draggedCard.id);
                        gameManager.ReOrderPlayerHandAfterReturn(draggedCard.id, draggedCard);                        
                    }
                    else {
                        //return from voidZone
                        draggedCard.returnBackToHand();
                    }
                    setDraggingFlag(false);
                }                         
            }else
            {
                Debug.Log("-------------NO RAY OUTPUT--------------");
                CheckFrom();
                return;
            }
        }
		//Debug.Log ("DROPPED INTO VOID");
		//return to hand if it's dropped in the void
		//draggedCard.returnBackToHand();
		//remove from playing zone and cars ToPlay        
    }

    void CheckFrom() {
        if (!gameManager.myPlayer.cardsHeld.ContainsKey(draggedCard.id))
        {
            Debug.Log("--Return back to hand--");
            gameManager.removeStoredCard(draggedCard.id);
            gameManager.ReOrderPlayerHandAfterReturn(draggedCard.id, draggedCard);                 
            setDraggingFlag(false);

            //return;
        }
        else
        {
            Debug.Log("--has in hand--");
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
