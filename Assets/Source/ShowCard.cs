using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowCard : MonoBehaviour {

    public TextMesh title;
    public TextMesh description;
    public GameObject cardArt;
     
    public GameObject card;

	// Use this for initialization
	void Start () {
  
        title.text = card.GetComponent<Card>().cardName;

        var originaltext = "Word cards are the type of cards that the players play out during the game. Each player should always have three cards on their hand";
        description.text = TextWrap(originaltext,30);

        // Debug.Log("datapath:"+Application.dataPath+ "persistentDataPath:" + Application.persistentDataPath);
        GameObject test = GameObject.Find("Image");
        RectTransform testTransform = test.GetComponent<RectTransform>();
        Vector3 a = testTransform.transform.localScale;
        Debug.Log(a);


        Debug.Log(Resources.Load("picture/tree"));
        Sprite[] testTexture = Resources.LoadAll<Sprite>("picture/tree");



        cardArt.GetComponent<SpriteRenderer>().sprite = testTexture[0];
        
        //cardArt.transform.localScale = this.transform.localScale;
        Debug.Log(testTexture[0]);


        cardArt.transform.localScale = new Vector2(testTransform.rect.width,testTransform.rect.height);

        //cardArt.transform.localScale = a;

        Debug.Log(test);
        /*
        string builder = "";

        float rowLimit = 1.9f; //find the sweet spot    
        string text = "This is some text we'll use to demonstrate word wrapping. It would be too easy if a proper wrapping was already implemented in Unity :)";
        string[] parts = text.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            Debug.Log(parts[i]);
            description.text += parts[i] + " ";
           // description.renderer
            if (description.GetComponent<Renderer>().bounds.extents.x > rowLimit)
            {
                description.text = builder.TrimEnd() + System.Environment.NewLine + parts[i] + " ";
            }
            builder = description.text;
        }*/

    }

    public static string TextWrap(string originaltext, int LowLimit)
    {
        string output = "";
        string[] words = originaltext.Split(' ');
        int line = 0;
        foreach (string word in words)
        {
            if ((line + word.Length + 1) <= LowLimit)
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
