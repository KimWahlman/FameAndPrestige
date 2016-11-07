using UnityEngine;
using System.Collections;

public abstract class Card : MonoBehaviour
{
    public int id;
    public string cardName;
    public int cost;
    public bool hasBeenPlayed;
    public int ownerID;
    
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
        if(ownerID == Player.idPlayer && !Input.GetKey(KeyCode.Mouse0))
        {
            ZoomCard(true);
        }
    }

    void OnMouseExit()
    {
        if (ownerID == Player.idPlayer)
        {
            ZoomCard(false);
        } 
    }

    void OnMouseDown()
    {
        ZoomCard(false);
    }


    abstract public void useCard();

    public void popCard(bool isMine, Transform posTransform, int playerID)
    {
        this.gameObject.SetActive(true);

        if (isMine)
            currentSprite.sprite = faceUpSprite;
        else
            currentSprite.sprite = faceDownSprite;

        this.gameObject.transform.position = posTransform.position;
        this.gameObject.transform.rotation = posTransform.rotation;

        ownerID = playerID;

        this.gameObject.SetActive(true);
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
            CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y + 2, goTransform.position.z);
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




    
    
}