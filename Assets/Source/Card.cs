using UnityEngine;
using System;
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
	SpriteRenderer currentArt;

	public TextMesh title;
	public TextMesh description;
	public GameObject cardArt;

	public float xPixel;
	public float yPixel;

    private Vector3 localScale;
	public bool toShowPoint = true;


    GameObject CardZoomed;

    public bool isBeingDragged = false;


	public string toolTipText ; // set this in the Inspector


	private string currentToolTipText = "";
	private GUIStyle guiStyleFore;
	private GUIStyle guiStyleBack;



    void Awake()
    {
        localScale = this.transform.localScale;
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
        deckPosition = this.transform.position;       

		guiStyleFore = new GUIStyle();
		guiStyleFore.fontSize = 16;
		guiStyleFore.normal.textColor = Color.white;
		guiStyleFore.alignment = TextAnchor.UpperCenter ;
		guiStyleFore.wordWrap = true;

    }

	public void LoadResource(string cardTitle, string cardDescription, string imgPath){


		//Debug.Log ("--------LOADING RESOURCES---------");
		title.text = cardTitle;

		var originaltext = cardDescription;
		description.text = TextWrap(originaltext,30);
		//Debug.Log (imgPath);

		//Debug.Log ("picture/" + imgPath);

		Sprite[] testTexture = Resources.LoadAll<Sprite>("picture/"+imgPath);
		//Debug.Log (testTexture [0]);
		cardArt.GetComponent<SpriteRenderer>().sprite = testTexture[0];
		xPixel = 7;
		yPixel = 8;


		var xScale = xPixel / cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		var yScale = yPixel / cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		//Debug.Log(cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size);

		cardArt.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(xScale,yScale,0);
	}

	public static string TextWrap(string originaltext, int LineLimit)
	{
		string output = "";
		string[] words = originaltext.Split(' ');
		int line = 0;
		foreach (string word in words)
		{
			if ((line + word.Length + 1) <= LineLimit)
			{
				output += " " + word;
				line += word.Length + 1;
			}
			else
			{
				output += "\n" + word;
				line = word.Length;
			}
		}
		return output;
	}


    void OnMouseEnter()
    {        
		if(currentSprite.sprite == faceUpSprite && !Input.GetKey(KeyCode.Mouse0))
        {
            ZoomCard(true);
        }
    }

	void OnMouseUp(){

		if (toShowPoint) {
			Debug.Log ("SHOW POINT");
			currentToolTipText = toolTipText;
			StartCoroutine (stopToolTip(1));
		}
	}


    void OnMouseExit()
    {
		Debug.Log ("ON MOUSE EXIT");
        ZoomCard(false);
		currentToolTipText = "";
    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
       	//ZoomCard(false);
    }


    abstract public void useCard();

    public void playCard()
    {
        print("card played");
        hasBeenPlayed = true;
        revealCard();
        useCard();
    }

    public void discardCard()
    {

    }

    public void revealCard()
    {
        currentSprite.sprite = faceUpSprite;
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void hideCard()
    {
        //currentSprite.sprite = faceDownSprite;
		currentSprite.sprite = null;
        /*
		cardArt.SetActive (false);
		title.gameObject.SetActive (false);
		description.gameObject.SetActive (false);
        */
        this.gameObject.SetActive(false);
        if(CardZoomed)
            Destroy(CardZoomed);

    }

    public void popCard(bool cardIsMine, Transform posTransform, int playerID)
    {
        //activate the card
        this.gameObject.SetActive(true);

        //show it faceup if it's mine
		if (cardIsMine) {
			currentSprite.sprite = faceUpSprite;
		} else {
			currentSprite.sprite = faceDownSprite;
			cardArt.SetActive (false);
			title.gameObject.SetActive (false);
			description.gameObject.SetActive (false);
		}


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

    
    void ZoomCard(bool zoomed)
    {
       if(zoomed && !isBeingDragged)
        {
            //create new object to show the zoomed card
			CardZoomed = new GameObject();


			GameObject zoomedCardArt = (GameObject)Instantiate (cardArt, cardArt.transform.localPosition, cardArt.transform.rotation); 
			TextMesh zoomedTitle = (TextMesh)Instantiate (title, title.transform.localPosition, title.transform.rotation);
			TextMesh zoomedDescription = (TextMesh)Instantiate (description, description.transform.localPosition, description.transform.rotation); 
	

			zoomedCardArt.transform.SetParent (CardZoomed.transform);
			zoomedTitle.transform.SetParent (CardZoomed.transform);
			zoomedDescription.transform.SetParent (CardZoomed.transform);
		
            //create a new component on the zoomed card
            SpriteRenderer newSprite = CardZoomed.AddComponent<SpriteRenderer>();
            //add the current sprite (card faced up)
            newSprite.sprite = currentSprite.sprite;


            //change the order of layer in order to show it over other sprites
            //newSprite.sortingOrder = 1;


//			Debug.Log (zoomedTitle.transform.position);

            //set the new position and scal of the zoomed card
            Transform goTransform = this.gameObject.transform;

            //if card has been played then zoom it but without the offset on Y axis
            if(!hasBeenPlayed)
                CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y + 2, goTransform.position.z - 1);
            else
                CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y, goTransform.position.z - 1);

            CardZoomed.gameObject.layer = 1;
//			Debug.Log (zoomedTitle.transform.position);

			this.localScale *= 1.5f;
            CardZoomed.transform.localScale = goTransform.localScale * 1.8f;


            //hide the real card sprite
            currentSprite.sprite = null;
			cardArt.SetActive (false);
			title.gameObject.SetActive (false);
			description.gameObject.SetActive (false);
		
            //this.gameObject.SetActive(false);           
        }
        else
        {
			
			if (CardZoomed)
            {
                //destroy the zoomed card
				this.localScale /= 1.5f;
                Destroy(CardZoomed);
                //shwo the real card sprite
                currentSprite.sprite = faceUpSprite;
				cardArt.SetActive (true);
				title.gameObject.SetActive (true);
				description.gameObject.SetActive (true);
			}
        }
        //Debug.Log("this.position:"+ this.transform.localPosition);
    }


	void OnGUI()
	{
		if (currentToolTipText != "")
		{
			var x = Event.current.mousePosition.x;
			var y = Event.current.mousePosition.y;
			GUI.Label (new Rect (x-150,y+20,300,60), currentToolTipText, guiStyleFore);
		}
	}

	/*
    void ZoomCard(bool zoomed) {
        if (zoomed && !isBeingDragged)
        {
            this.transform.localScale = localScale * 2;
        }
        else {
            this.transform.localScale = localScale;
        }
    }
	*/

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
		Debug.Log ("handPosition:"+handPosition);
        this.transform.position = handPosition;
    }

	IEnumerator stopToolTip(float time)
	{
		yield return new WaitForSeconds(time);

		currentToolTipText = "";
	}
}