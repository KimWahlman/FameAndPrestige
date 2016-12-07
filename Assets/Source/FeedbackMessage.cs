using UnityEngine;
using System.Collections;

public class FeedbackMessage : MonoBehaviour {

    public float delayBeforeVanish = 10.0f;
    float delay;
    public bool withDelay = false;
    void OnEnable()
    {
        if(withDelay)
            delay = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            this.gameObject.SetActive(false);
        }

        if(withDelay)
        {
            delay += Time.deltaTime;
            if (delay > delayBeforeVanish)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
