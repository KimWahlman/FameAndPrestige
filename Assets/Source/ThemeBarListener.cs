using UnityEngine;
using System.Collections;

public class ThemeBarListener : MonoBehaviour {

	private bool flag = true;
	public TextMesh text;
	public float wait = 0.7f;
	public UIManager manager;

	void OnMouseDown(){
		Debug.Log ("Clicked");
		Debug.Log (text.text);
		manager.ShowBubble (text.text);
		/*if (flag) {
			//GetComponent<Animator> ().SetBool ("openMe", true);

			flag = false;
		} else {
			//GetComponent<Animator> ().SetBool ("openMe", false);
			//text.gameObject.SetActive (false);
			flag = true;
		}*/
	}
		
}
