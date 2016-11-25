using UnityEngine;
using System.Collections;

public abstract class Card : MonoBehaviour
{
    public int id;
    public string cardName;
    public int cost;
    public bool hasBeenPlayed = false;
    public int ownerID;
    public bool isMine;

    public Vector3 handPosition;
    public Vector3 deckPosition;

    public Sprite faceUpSprite;
    public Sprite faceDownSprite;
    SpriteRenderer currentSprite;

    private Vector3 localScale;

    GameObject CardZoomed;

    public bool isBeingDragged = false;


    void Awake()
    {
        localScale = this.transform.localScale;
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
        deckPosition = this.transform.position;
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

    public void hideCard()
    {
        currentSprite.sprite = faceDownSprite;
        this.gameObject.SetActive(false);
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


        //StartCoroutine("PositionOnTheHand", posTransform);

        PositionOnTheHand(posTransform, true);

        //assign the ownerID of the card
        ownerID = playerID;

        isMine = cardIsMine;

        hasBeenPlayed = false;
    }

    public void PositionOnTheHand(Transform posTransform, bool whileDrawing = false)
    {
        this.gameObject.transform.rotation = posTransform.rotation;

        if (!whileDrawing)
            this.gameObject.transform.position = posTransform.position;
        else
            StartCoroutine("lerpCards", posTransform);


        handPosition = posTransform.position;
    }

    IEnumerator lerpCards(Transform posTransform)
    {
        float duration = 0.4f;
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            this.gameObject.transform.position = Vector3.Lerp(deckPosition, posTransform.position, t / duration);
            yield return null;
        }
        this.gameObject.transform.position = posTransform.position;
    }

    public void PositionOnTheBoard(Transform posTransform)
    {
        this.gameObject.transform.position = posTransform.position;
        this.gameObject.transform.rotation = posTransform.rotation;
    }

    /*
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
    */

    void ZoomCard(bool zoomed) {
        if (zoomed && !isBeingDragged)
        {
            this.transform.localScale = localScale * 2;
        }
        else {
            this.transform.localScale = localScale;
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
        this.transform.position = handPosition;
    }
}