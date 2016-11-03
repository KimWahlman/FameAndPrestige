using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

    public GameObject prefab;
    public bool mouseOver = false;
    
    /*
     * TODO: Make the drawn card random
     * Hold an array that keep track of how many 
     * of a specific card is still in the deck.
     */

    // Use this for initialization
    void Start () { }
	
	// Update is called once per frame
	void Update ()
    {
        if(mouseOver)
            CreateNewCard();
	}

    void CreateNewCard()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int randomNum = Random.Range(1, 3); // returns a random number between 1 and 2.
            Instantiate(prefab, new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
            prefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/Card" + randomNum, typeof(Sprite)) as Sprite;
        }        
    }

    void OnMouseOver()
    {
        if (!mouseOver) mouseOver = true;
        Debug.Log(mouseOver);
    }

    void OnMouseExit()
    {
        if (mouseOver) mouseOver = false;
        Debug.Log(mouseOver);
    }
}
