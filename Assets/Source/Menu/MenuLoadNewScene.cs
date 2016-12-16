using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoadNewScene : MonoBehaviour {
    // Use this for initialization
    public Camera camera;
    void Start () {

    }

    private void OnMouseDown()
    {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hej 2");
                // the object identified by hit.transform was clicked
                // do whatever you want
                SceneManager.LoadScene("MainGameVersion");
            }
        }
    }
}
