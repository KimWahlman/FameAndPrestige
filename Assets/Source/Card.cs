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
    
    void Start()
    {
        
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
        this.gameObject.SetActive(false);

        
    }
    

    abstract public void useCard();

    public void popCard(bool isMine, Transform posTransform)
    {
        if (isMine)
            currentSprite.sprite = faceUpSprite;
        else
            currentSprite.sprite = faceDownSprite;

        this.gameObject.transform.position = posTransform.position;
        this.gameObject.transform.rotation = posTransform.rotation;

        this.gameObject.SetActive(true);
    }




    
    
}