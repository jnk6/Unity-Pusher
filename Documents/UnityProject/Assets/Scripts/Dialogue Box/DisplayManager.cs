using DBEssentials;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DisplayManager : MonoBehaviour {
	
	GameObject gui;

	//Canvas canvas;
	//CanvasScaler cscaler;
	//GraphicRaycaster grcaster;
	
	//You might have to hook up these objects yourself in the GUI
	//public GameObject dbgo; //dialogue box game object **dont think we need this
	public GameObject dbtitle; //child object for db title label
	public GameObject dbtext; //child object for db text
	public GameObject dbportrait; //child object for db potrait
	public GameObject dbMenu; //handles actions/choices 

	RectTransform rtransform; //transform for dialogue box, i don't know we might not need this
	Text content; //the actual content for the dialogue box
	Text title; //the title 'text' for the dialogue box
	SpriteRenderer spriteRenderer;
	Sprite portrait;
	public GameObject[] DFbuttons;
	Button[] DFButtons;

	Text menuTitle;
	GameObject[] buttons;
	
	public GameObject logtitlego; //object for log title
	public GameObject logtextgo; //child object for text
	GameObject loggo; //to make active or not active
	Text logcontent;
	Text logtitle; 

	GameObject esgo;//event system game object
	EventSystem eventsystem;
	StandaloneInputModule standaloneIM; 
	TouchInputModule touchIM;

	void Awake(){
		//set Actives to false by default
		HideMenu = true;

		gui = gameObject;
		loggo = logtextgo.transform.parent.gameObject;

		dbtext.name = "Content";
		dbtitle.name = "Title";
		dbportrait.name = "Portrait";



		/*
		dbgo.AddComponent<CanvasRenderer> ();
		dbtext.AddComponent<CanvasRenderer> ();
		dbtitle.AddComponent<CanvasRenderer> ();
		dbportrait.AddComponent<CanvasRenderer> ();
		*/


		/*
		rtransform = dbtext.AddComponent<RectTransform> ();
		rtransform.localPosition = new Vector3 (67.5f, -124.2f, 0);
		rtransform.sizeDelta = new Vector2 (340, 70);
		*/

		logcontent = logtextgo.GetComponent<Text> ();
		logtitle = logtitlego.GetComponent<Text> ();
		content = dbtext.GetComponent<Text> ();
		title = dbtitle.GetComponent<Text> ();
		spriteRenderer = dbportrait.GetComponent<SpriteRenderer> ();
		portrait = spriteRenderer.sprite;

		DFButtons = new Button[3];
		for (int i = 0 ; i < DFbuttons.Length; ++i){
			DFButtons[i] = DFbuttons[i].GetComponent<Button>();
		}

		menuTitle = dbMenu.GetComponent<Text> ();

		buttons = new GameObject[dbMenu.transform.childCount];
		for (int i = 0; i < dbMenu.transform.childCount; ++i) {
			buttons[i] = dbMenu.transform.GetChild (i).gameObject;
		}



		logcontent.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		logtitle.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		content.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		title.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");

		// use this for imported fonts 
		//content.font = Resources.Load<Font>("file path/name");

		//set up log GUI
		//**Work In Progress**
		//**make something clean here
	
		//set up the event system and components
		esgo = new GameObject ();
		GameObject.Instantiate (esgo);
		esgo.name = "EventSystem";
		eventsystem = esgo.AddComponent<EventSystem>();
		standaloneIM = esgo.AddComponent<StandaloneInputModule> ();
		touchIM = esgo.AddComponent<TouchInputModule> ();


		//Display initialization
		title.text = "THIS IS THE CHAR NAME :D";
		content.text = "TALKY STUFF ;)";


	}

	//with new speaker
	public void updateDialogueBox(Sprite portrait, string characterName, string textbody){
		this.portrait = portrait;
		title.text = characterName;
		content.text = textbody;
	}

	//if same speaker
	public void updateDialogueBox(string characterName, string textbody){
		if (portrait == null) {
			return;
		}
		title.text = characterName;
		content.text = textbody;
	} 

	//uses dframes
	public void updateDialogueBox(DialogueFrame dframe){
		title.text = dframe.CharName;
		content.text = dframe.DialogueText;
		portrait = dframe.Portrait;
	}

	public GameObject[] updateMenu(string title, string[] buttonLabels){
		if (buttonLabels.Length != 4) {
			Debug.LogError("You must have 4 button labels to update menu");
			return null;		
		}
		menuTitle.text = title;

		for (int i = 0; i < buttonLabels.Length; ++i) {
			buttons[i].GetComponent<Text>().text = buttonLabels[i];
		}

		return buttons;
	}

	public GameObject[] updateMenu (DialogueFrame dframe){
		menuTitle.text = dframe.DialogueText;
		for (int i = 0; i < dframe.ButtonLabels.Length; ++i) {
			buttons[i].transform.GetChild(0).GetComponent<Text>().text = dframe.ButtonLabels[i];
		}

		return buttons;

	}

	public void updateLog(string title, string content){
		logtitle.text = title;
		logcontent.text = content;
	}

	public void updateLog( string content){
		logcontent.text = content;
	}


	public bool HideLog {
		get{ return loggo.activeSelf;}
		set{ loggo.SetActive(!value);}

	}

	public bool HideMenu {
		get{ return dbMenu.activeSelf;}
		set{ dbMenu.SetActive(!value);}
	}

	public bool ForwardButtonIsOn{
		get {
			return ( DFButtons[0].interactable && DFButtons[1].interactable);
		}
		set {
			DFButtons[0].interactable = value;
			DFButtons[1].interactable = value;
		}
	}

	public bool BackwardButtonIsOn{
		get {
			return ( DFButtons[2].interactable);
		}
		set {
			DFButtons[2].interactable = value;
		}

	}

	// Update is called once per frame
	void Update () {
		
	}

}
