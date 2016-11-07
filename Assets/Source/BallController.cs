using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    // Use this for initialization
    Rigidbody rigbodRef;
    public float Speed = 5.0f;
    public int Points = 0;
    private float horizontalMovement;
    private float verticalMovement;
    public GameObject textObj;

    void Start ()
    {
        rigbodRef = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        textObj = GameObject.Find("Score");
        textObj.GetComponent<TextMesh>().text = "Points: " + Points;
    }

    void FixedUpdate()
    {
        rigbodRef.AddForce(horizontalMovement * Speed, 0f, verticalMovement * Speed);
    }

    //void OnCollisionExit
    //void OnCollisionStay
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag("Collectable"))
        {
            Points += 1;
            Destroy(coll.gameObject);
        }
    }
}
