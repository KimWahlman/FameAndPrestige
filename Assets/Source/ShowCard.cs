using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class ShowCard : MonoBehaviour {

    public TextMesh title;
    public TextMesh description;
    public GameObject cardArt;
    public float xPixel;
    public float yPixel;
    
	// Use this for initialization




	public void LoadResource(string cardTitle, string cardDescription, string imgPath){


		Debug.Log ("--------LOADING RESOURCES---------");
		title.text = cardTitle;

		var originaltext = cardDescription;
		description.text = TextWrap(originaltext,30);
		//Debug.Log (imgPath);


		Sprite[] testTexture = Resources.LoadAll<Sprite>(imgPath);
		//Debug.Log (testTexture [0]);
		cardArt.GetComponent<SpriteRenderer>().sprite = testTexture[0];
		xPixel = 7;
		yPixel = 8;


		var xScale = xPixel / cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		var yScale = yPixel / cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		//Debug.Log(cardArt.GetComponent<SpriteRenderer>().sprite.bounds.size);

		cardArt.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(xScale,yScale,0);
	}


	void Start () {


		Debug.Log ("--------STARTING OF GAMEOBJECT---------");
        
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
