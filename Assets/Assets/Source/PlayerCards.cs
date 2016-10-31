using UnityEngine;
using System.Collections;

public class PlayerCards : MonoBehaviour {

    public int ownerID; // Fix so we know who picked it up.
    public bool showAll = false; // Card shouldn't be viewed to everyone.
    public bool inspectCard = false;

    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;

    void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (inspectCard)
        {
            MoveCard2D();
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    //TODO: Fix so fast movements of the mouse doesn't "drop" the card.
    void MoveCard2D()
    {
        if (Input.GetMouseButton(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z += 0.1f;
            transform.position = Vector2.Lerp(transform.position, mousePosition, 1.0f);
        }
        
    }

    void OnMouseOver()
    {
        if (!inspectCard)
            inspectCard = true;
    }

    void OnMouseExit()
    {
        if (inspectCard)
            inspectCard = false;
    }
}
