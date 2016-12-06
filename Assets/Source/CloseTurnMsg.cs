using UnityEngine;
using System.Collections;

public class CloseTurnMsg : MonoBehaviour {

    float delayBeforeVanish = 10.0f;
    float delay;

    void OnEnable()
    {
        delay = 0;
    }

    void Update()
    {
        delay += Time.deltaTime;
        Debug.Log(delay);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            this.gameObject.SetActive(false);
        }

        if(delay > delayBeforeVanish)
        {
            this.gameObject.SetActive(false);
        }
    }
}
