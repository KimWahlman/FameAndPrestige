using UnityEngine;
using System.Collections;

public class UIText : MonoBehaviour {

    public GameObject mPointText, mActionsAvailableText;
    private int points = 0, actionPoints = 0;
	// Use this for initialization
	void Start () {	}
	
	// Update is called once per frame
	void Update () {
        mPointText = GameObject.Find("Score");
        mActionsAvailableText = GameObject.Find("AP");

        mPointText.GetComponent<TextMesh>().text = "Points: " + points;
        mActionsAvailableText.GetComponent<TextMesh>().text = "Actions available: " + actionPoints;

    }
}
