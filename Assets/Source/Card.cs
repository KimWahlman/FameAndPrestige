using UnityEngine;
using System;
using System.Collections;

public abstract class Card : MonoBehaviour
{
    public int id;
    public string cardName;
    public int cost;
    public bool hasBeenPlayed = false;
    public bool isOnTheBoard = false;
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
	public TextMesh point;
	public GameObject cardArt;

	public float xPixel;
	public float yPixel;

    private Vector3 localScale;
	private string Theme;
	public bool toShowPoint = false;
	public bool alreadyShowed = false;

    GameObject CardZoomed;
    LastPlayZone lastPlayZone;

    public bool isBeingDragged = false;


	public string toolTipText ; // set this in the Inspector


	private string currentToolTipText = "";
	private GUIStyle guiStyleFore;
	private GUIStyle guiStyleBack;

    public UIManager UImanager;
    public NetworkManager networkManager;

    void Start()
    {
        lastPlayZone = GameObject.Find("LastPlayZone").GetComponent<LastPlayZone>();
        UImanager = GameObject.Find("UIManager").GetComponent<UIManager>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Awake()
    {
        localScale = this.transform.localScale;
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
        deckPosition = this.transform.position;


        


        guiStyleFore = new GUIStyle();

		guiStyleFore.fontSize = 22;
		guiStyleFore.normal.textColor = Color.black;
		guiStyleFore.alignment = TextAnchor.UpperCenter ;
		guiStyleFore.wordWrap = true;
		guiStyleFore.active.background = Texture2D.blackTexture;

    }

	public void LoadResource(string cardTitle, string cardDescription, string point_text, string type, string theme, string imgPath){


		if (type.Equals("pure")) {
			Sprite card_board = Resources.LoadAll<Sprite>("card_board")[0];
			currentSprite.sprite = card_board;
			faceUpSprite = card_board;
		} else {
			Sprite card_board = Resources.LoadAll<Sprite>("mixed_card_board")[0];
			currentSprite.sprite = card_board;
			faceUpSprite = card_board;
		}
		Theme = theme;

		//Debug.Log ("--------LOADING RESOURCES---------");
		title.text = cardTitle;
		point.text = point_text;

		var originaltext = cardDescription;
		description.text = TextWrap(originaltext,30);

		Sprite[] testTexture = Resources.LoadAll<Sprite>("picture/"+imgPath);

		cardArt.GetComponent<SpriteRenderer>().sprite = testTexture[0];
		xPixel = 7;
		yPixel = 8.2f;


		var xScale = xPixel / cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		var yScale = yPixel / cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

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

		/*
		if (toShowPoint && isMine) {
			currentToolTipText = toolTipText;
			StartCoroutine (stopToolTip(1));
		}
		*/
	}


    void OnMouseExit()
    {
        ZoomCard(false);
		currentToolTipText = "";
    }

    void Update()
    {
        if (!isMine)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!toShowPoint)
            {
                UImanager.ShowBubble("Are you sure?\n This will cost 1 point");
                StartCoroutine(setVariableTrue());
                return;
            }
            else
            {
                if (alreadyShowed)
                {
                    showTheme();
                }
                else
                {
                    networkManager.AskTheme(this, cardName, id, ownerID);
                    alreadyShowed = true;
                }
            }
        }
    }

	public void showTheme(){
        UImanager.ShowBubble ("The theme of this card is : "+ Theme +" !");
	}

	public void showErrorTheme(){
        UImanager.ShowBubble ("Not enough points to show the theme.");
	}

	IEnumerator setVariableTrue(){

		yield return new WaitForSeconds(0.5f);
		toShowPoint = true;
	}

    abstract public void useCard();

    public void playCard()
    {
        hasBeenPlayed = true;
        isOnTheBoard = true;
        revealCard();
        useCard();
    }
		
    public void revealCard()
    {
        currentSprite.sprite = faceUpSprite;
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.gameObject.transform.localScale += new Vector3(0.155f, 0.155f, 0.0f);
    }

    public void hideCard()
    {
        Debug.Log("hide card");
		currentSprite.sprite = null;
        isOnTheBoard = false;
        this.gameObject.SetActive(false);
        if(CardZoomed)
            Destroy(CardZoomed);

    }

    public void destroyCard()
    {
        if (CardZoomed)
            Destroy(CardZoomed);

        Destroy(this.gameObject);
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
            currentSprite.transform.localScale = new Vector3(0.145f, 0.145f, 0.0f);
			cardArt.SetActive (false);
			title.gameObject.SetActive (false);
			description.gameObject.SetActive (false);
			point.gameObject.SetActive (false);
			toShowPoint = false;
			alreadyShowed = false;
		}


        //StartCoroutine("PositionOnTheHand", posTransform);

        PositionOnTheHand(posTransform, true);

        //assign the ownerID of the card
        ownerID = playerID;

        isMine = cardIsMine;

        hasBeenPlayed = false;
        isOnTheBoard = false;
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
			TextMesh zoomedPoint = (TextMesh)Instantiate (point, point.transform.localPosition, point.transform.rotation); 


			zoomedCardArt.transform.SetParent (CardZoomed.transform);
			zoomedTitle.transform.SetParent (CardZoomed.transform);
			zoomedDescription.transform.SetParent (CardZoomed.transform);
			zoomedPoint.transform.SetParent (CardZoomed.transform);
		
            //create a new component on the zoomed card
            SpriteRenderer newSprite = CardZoomed.AddComponent<SpriteRenderer>();
            //add the current sprite (card faced up)
            newSprite.sprite = currentSprite.sprite;

            Transform goTransform = this.gameObject.transform;

            //if card has been played then zoom it but without the offset on Y axis
            if(!isOnTheBoard)
                CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y + 2, goTransform.position.z - 1);
            else
                CardZoomed.transform.position = new Vector3(goTransform.position.x, goTransform.position.y, goTransform.position.z - 1);
            CardZoomed.gameObject.layer = 1;

			this.localScale *= 1.5f;

            
            
            if (lastPlayZone.CheckContainCard(this.id)) 
            {
                CardZoomed.transform.localScale = goTransform.localScale * 2.5f;
                description.gameObject.SetActive(true);
            } else
            {
                CardZoomed.transform.localScale = goTransform.localScale * 1.8f;
            }

            //hide the real card sprite
            currentSprite.sprite = null;
			cardArt.SetActive (false);
			title.gameObject.SetActive (false);
			description.gameObject.SetActive (false);
			point.gameObject.SetActive (false);
		
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
				point.gameObject.SetActive (true);
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
        isOnTheBoard = false;
        this.transform.position = handPosition;
    }

	IEnumerator stopToolTip(float time)
	{
		yield return new WaitForSeconds(time);

		currentToolTipText = "";
	}
}