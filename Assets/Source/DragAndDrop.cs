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
        else
        {
            dragging = false;
			this.gameObject.layer = l;
        }
    }

    //when the card is dropped
    void releaseCard()
    {
        if (!draggedCard.hasBeenPlayed && draggedCard.isMine && gameManager.checkPlayerTurn())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Debug.Log (ray);
			Debug.Log (Physics.Raycast (ray, out hit));
            if (Physics.Raycast(ray, out hit))
            {
				Debug.Log (hit.collider.name);
				Debug.Log (hit.transform.gameObject.name);
				Debug.Log (ray);			

                //if the card is dropped on the zone
                if (hit.transform.gameObject.name == "PlayableZone")
                {
					Debug.Log ("DROPPED INTO PLAYZONE");
                    //get the script of the zone (contain the slots position)
                    pz = hit.transform.gameObject.GetComponent<PlayableZone>();

                    //assign the free slot position to the card
                    draggedCard.PositionOnTheBoard(pz.getSlot());

                    //networkManager.SendPlayCard(draggedCard.ownerID, draggedCard.id);
                    gameManager.storeCard(draggedCard.id);

                    gameManager.ReOrderPlayerHandAfterDrop(draggedCard.id);
					this.gameObject.layer = l;
                    //(should request to the server if i can play)
                    //if yes, use the card
                    //else, put the card back
					this.gameObject.layer = l;
					dragging = false;
					draggedCard.isBeingDragged = false;
					draggedCard.putInFront(false);
					return;

                    //use the dropped card 
                    //draggedCard.playCard();
                    //gameManager.ReOrderPlayerHand(draggedCard.ownerID, draggedCard.id);
                }
                else
                {
					Debug.Log ("add the card to the player hand if it has been dragged on playable zone first");
                    //add the card to the player hand if it has been dragged on playable zone first
                    if(!gameManager.myPlayer.cardsHeld.ContainsKey(draggedCard.id))
                    {
                        gameManager.myPlayer.cardsHeld.Add(draggedCard.id, draggedCard);
                        
                        //get the next position available in the player hand
                        draggedCard.handPosition = gameManager.playerHands[gameManager.myPlayer.idPlayer].newCard().position;
						this.gameObject.layer = l;
						gameManager.removeStoredCard(draggedCard.id);
						dragging = false;
						draggedCard.isBeingDragged = false;
						draggedCard.putInFront(false);
						return;
                    }

					draggedCard.returnBackToHand();
					this.gameObject.layer = l;
					dragging = false;
					draggedCard.isBeingDragged = false;
					draggedCard.putInFront(false);
					return;

                }
            }
        }
		Debug.Log ("DROPPED INTO VOID");
		//return to hand if it's dropped in the void
		draggedCard.returnBackToHand();
		//remove from playing zone and cars ToPlay


    }


}
