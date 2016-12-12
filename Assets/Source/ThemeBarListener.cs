using UnityEngine;
using System.Collections;

public class ThemeBarListener : MonoBehaviour {

	private bool flag = true;
	public TextMesh text;
	public float wait = 0.7f;

	void OnMouseDown(){
		if (flag) {
			GetComponent<Animator> ().SetBool ("openMe", true);
			StartCoroutine (showText ());
			flag = false;
		} else {
			GetComponent<Animator> ().SetBool ("openMe", false);
			text.gameObject.SetActive (false);
			flag = true;
		}
	}

	IEnumerator showText(){
		yield return new WaitForSeconds(wait);
		text.gameObject.SetActive (true);
	}
		
}
