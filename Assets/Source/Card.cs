using UnityEngine;
using System.Collections;

public abstract class Card : MonoBehaviour
{
    public int id;
    public string cardName;
    public int cost;
    public bool hasBeenPlayed;
    public int ownerID;
    public bool isMine;

    public Vector3 startPosition;

    public Sprite faceUpSprite;
    public Sprite faceDownSprite;
    SpriteRenderer currentSprite;

    GameObject CardZoomed;

    public bool isBeingDragged = false;


    void Awake()
    {
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        if(currentSprite.sprite == faceUpSprite && !Input.GetKey(KeyCode.Mouse0))
        {
            ZoomCard(true);
        }
    }

    void OnMouseExit()
    {
        ZoomCard(false);
    }

    void OnMouseDown()
    {
        ZoomCard(false);
    }


    abstract public void useCard();

    public void playCard()
    {
        print("card played");
        hasBeenPlayed = true;
        revealCard();
        useCard();
    }

    public void revealCard()
    {
        currentSprite.sprite = faceUpSprite;
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void popCard(bool cardIsMine, Transform posTransform, int playerID)
    {
        //activate the card
        this.gameObject.SetActive(true);

        //show it faceup if it's mine
        if (cardIsMine)
            currentSprite.sprite = faceUpSprite;
        else
            currentSprite.sprite = faceDownSprite;


        AssignNewPosition(posTransform, false);

        //assign the ownerID of the card
        ownerID = playerID;

        isMine = cardIsMine;
    }

    public void AssignNewPosition(Transform posTransform, bool onPlayableZone)
    {
        
        //assign it to the hands position
        this.gameObject.transform.position = posTransform.position;
        this.gameObject.transform.rotation = posTransform.rotation;

        //save the starting position of the card (if it has to return to owner hand)
        if(!onPlayableZone)
            startPosition = posTransform.position;
    }

    void ZoomCard(bool zoomed)
    {
       if(zoomed && !isBeingDragged)
        {
            //create new object to show the zoomed card
            CardZoomed = new GameObject();

            //create a new component on the zoomed card
            SpriteRenderer newSprite = CardZoomed.AddComponent<SpriteRenderer>();
            //add the current sprite (card faced up)
            newSprite.sprite = currentSprite.sprite;
            //change the order of layer in order to show it over other sprites
            newSprite.sortingOrder = 1;

            //set the new position and scal of the zoomed card
            Transform goTransform = this.gameObject.transform;

            //if card has been played then zoom it but without the offset on Y axis
            if(!hasBeenPlayed)
                CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y + 2, goTransform.position.z);
            else
                CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y, goTransform.position.z);

            CardZoomed.transform.localScale = goTransform.localScale * 2;

            //hide the real card sprite
            currentSprite.sprite = null;

        }
        else
        {
            if (CardZoomed)
            {
                //destroy the zoomed card
                Destroy(CardZoomed);
                //shwo the real card sprite
                currentSprite.sprite = faceUpSprite;
            }
        }
    }

    //change the layer order of the card
    public void putInFront(bool inFront)
    {
        if (inFront)
            currentSprite.sortingOrder = 1;
        else
            currentSprite.sortingOrder = 0;
    }

    public void returnBackToHand()
    {
        this.transform.position = startPosition;
    }







    
    
}