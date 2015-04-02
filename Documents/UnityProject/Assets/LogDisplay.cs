using UnityEngine;
using System.Collections;

public class LogDisplay : MonoBehaviour {
	Canvas canvas;
	string title;
	GUIText content;

	void Awake(){

		//canvas = gameObject.AddComponent<Canvas>();
	}

	void Start(){
		title = "There is currently nothing in your log.";
		//content = transform.GetChild ().GetComponent<GUIText>();
		content.text = "This is the content";
	}

	void OnGUI(){
	
	}	

}
