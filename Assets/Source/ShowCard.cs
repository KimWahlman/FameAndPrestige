using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowCard : MonoBehaviour {

    public TextMesh title;
    public TextMesh description;
    public GameObject cardArt;
    public float xPixel;
    public float yPixel;
     
    public GameObject card; 
	// Use this for initialization
	void Start () {
        xPixel = 7;
        yPixel = 8;
  
        title.text = card.GetComponent<Card>().cardName;

        var originaltext = "Word cards are the type of cards that the players play out during the game. Each player should always have three cards on their hand";
        description.text = TextWrap(originaltext,30);

        Sprite[] testTexture = Resources.LoadAll<Sprite>("picture");
        cardArt.GetComponent<SpriteRenderer>().sprite = testTexture[0];
                 
        //Debug.Log("anchorMax" + (cardArt.GetComponent<RectTransform>().anchorMax));
        /*var anchorMax = cardArt.GetComponent<RectTransform>().anchorMax;
        var anchorMin = cardArt.GetComponent<RectTransform>().anchorMin;
        var x = anchorMax.x - anchorMin.x;
        var y = anchorMax.y - anchorMin.y;*/

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


    // Update is called once per frame
    void Update () {
	
	}
}
