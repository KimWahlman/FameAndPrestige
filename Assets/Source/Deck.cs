using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

    public GameObject prefab;
    public bool mouseOver = false;

    // Use this for initialization
    void Start ()
    {
        Debug.Log(mouseOver);
    }
	
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
            Instantiate(prefab, new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
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
