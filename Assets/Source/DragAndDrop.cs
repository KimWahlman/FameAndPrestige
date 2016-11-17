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

    void Awake()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        }
        else
        {
            dragging = false;
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
                if (hit.transform.gameObject.name == "PlayableZone")
                {
                    //get the script of the zone (contain the slots position)
                    pz = hit.transform.gameObject.GetComponent<PlayableZone>();

                    //assign the free slot position to the card
                    draggedCard.PositionOnTheBoard(pz.getSlot());

                    //networkManager.SendPlayCard(draggedCard.ownerID, draggedCard.id);
                    gameManager.storeCard(draggedCard.id);

                    //(should request to the server if i can play)
                    //if yes, use the card
                    //else, put the card back

                    //use the dropped card 
                    //draggedCard.playCard();
                    //gameManager.ReOrderPlayerHand(draggedCard.ownerID, draggedCard.id);
                }
                else
                {
                    //return to hand if it's dropped in the void
                    draggedCard.returnBackToHand();
                    gameManager.removeStoredCard(draggedCard.id);
                }
            }
        }

        dragging = false;
        draggedCard.isBeingDragged = false;
        draggedCard.putInFront(false);
    }


}
