using UnityEngine;
using System.Collections;

public class ThemeBarListener : MonoBehaviour {

	private bool flag = true;
	public TextMesh text;

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
		yield return new WaitForSeconds(0.4f);
		text.gameObject.SetActive (true);
	}
		
}
