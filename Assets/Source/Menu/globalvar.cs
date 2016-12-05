using UnityEngine;
using System.Collections;

public class globalvar : MonoBehaviour {

	public int port;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
