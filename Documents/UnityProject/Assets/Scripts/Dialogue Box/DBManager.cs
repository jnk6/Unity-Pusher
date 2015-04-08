/*=====================================================
 * Dialogue Box Manager version 0.1.0
 * 
 * Manages the dialogue system (or attempts to)
 *
 *=====================================================
 **/
using DBEssentials;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DBManager : MonoBehaviour {

	DisplayManager dbdisplay; //Handles GUI Stuff
	Log log; //Handles Log logic
	DFQueue curDFQ; //A queue that holds the string of dialogue frames representing 
					//the current conversation
	/**
	 * For more (rough) documentation on Log and DFQueue, and other DBEssential classes,
	 * look inside their script files.
	 **/

	Button[] menuButtons;
	MenuButtonActions menuActions;

	public Dictionary<string, string[]> menuButtonActionDict; // used for menu button actions. The string key is the menu ID
												  // and the string[] value are the button action names 
	public Dictionary<string, Sprite> portdict; // dictionary used to match up character names
												// with their sprite
	public Dictionary<string, MenuButtonActions> menuActionDict; // the key is the menu ID and MenuAction
													// will have the functions that is called when
	bool fastForward;	//boolean used for fastforward function

	void Start(){
		fastForward = false;
		menuActions = GetComponent<MenuButtonActions> ();

		//button tempfix
		menuButtons = new Button[4];

		//you may later decide to do handle a character's properties
		//in a seperate class altogether, but for now to make the dialogue
		//box work you must add every character name as a key to this dictionary
		//and a reference to his/her portrait sprite as the value (null if there
		// is no portrait).
		portdict = new Dictionary<string, Sprite> ();

		portdict.Add ("", null); //the empty name, empty portrait character used mostly for events
		portdict.Add ("menu", null);
		portdict.Add ("Medical Examiner", null);
		portdict.Add ("Man 1", null);
		portdict.Add ("Man 2", null);
		portdict.Add ("Woman 1", null);
		portdict.Add ("Computer", null);

		menuButtonActionDict = new Dictionary<string, string[]> ();
		menuButtonActionDict.Add("Hello World!", new string[] {"Hello World!", "Hello World!", "Hello World!", "CloseMenu"});

		dbdisplay = GameObject.FindWithTag ("dbdisplay").GetComponent<DisplayManager>();
		log = new Log (5);
		curDFQ = new DFQueue(); 
		
		//Gets your dialogue txt file using the file path.
		TextAsset file = Resources.LoadAssetAtPath<TextAsset> ("Assets/Resources/story_scripts/example/example.txt") as TextAsset;
		if (file == null) {
			Debug.LogError("No file found. Please check file path or file does not exist");		
		}

		//convert your txt file to a DFQueue and assign it to curDFQ
		//Whatever is in the front of your current Dialogue Frame Queue is what shows up
		//on display in the dialogue box.
		curDFQ = textToDFrames (file);

	}

	void Update(){
		Debug.Log ("Current Log State: " + log.State);

		//At any given time, display the DFrame at the front of the current DFQueue, if it is not
		//empty.
		if (!curDFQ.IsEmpty) {

			if (curDFQ.front ().Type == DialogueFrame.dftype.choice){
				HandleMenu (curDFQ.front ());
			}
			else{
				dbdisplay.updateDialogueBox (curDFQ.front());
			}
		}

		//probably not the most efficient way to implement this, but definitely the simplest
		//if the next dialogue frame is a choice dialogue or the queue is empty, disable the forward
		//buttons
		if (curDFQ.IsEmpty) {
			//set all forward buttons' interactable to false
			dbdisplay.ForwardButtonIsOn = false;
			fastForward = false;

			if (log.State == Log.state.opened){
				dbdisplay.ForwardButtonIsOn = true;
			}
		}
		else {
			if ( curDFQ.front ().Type == DialogueFrame.dftype.choice ){
				dbdisplay.ForwardButtonIsOn = false;
				fastForward = false;
				dbdisplay.BackwardButtonIsOn = false;
			}
			else{
				dbdisplay.ForwardButtonIsOn = true;
				dbdisplay.BackwardButtonIsOn = true;
			}
		}

		if (log.State == Log.state.closed) {
			dbdisplay.HideLog = true;		
		}
		else{
			dbdisplay.HideLog = false;
		}
		//this statement has to be after the curDFQ empty check or else you might try
		//to dequeue an empty queue
		if (fastForward == true) {
			Forward();		
		}
	}
	
	DFQueue textToDFrames(TextAsset file) {

		if (file == null) {
			Debug.LogError ("File does not exist!");
			return null;
		}

		DFQueue dfq = new DFQueue();
		string script_title;
		bool titleFlag = false;
		int maxChar = 182;
		string[] tkn = file.text.Split (new string[] {"\n"}, StringSplitOptions.None);

		for (int i = 0; i < tkn.Length; ++i) {
			if (tkn[i].Trim() == "" || tkn[i].Substring(0,3)== ";;;"){
				continue;
			}

			if (tkn[i].Substring(0,3) == "&&&" && titleFlag == false){
				script_title = tkn[i];
				titleFlag=true;
				continue;
			}

			string[] subtkn = tkn[i].Split(new string[]{"@@@"}, StringSplitOptions.None);
			string name = subtkn[0].Trim();
			string text = subtkn[1].Trim();
			//Debug.Log (name);
			if (name.Length > 6 && name.Substring(0,6)== "#MENU#"){
				string[] nameText = text.Split (new string[]{"$button$"}, StringSplitOptions.None);
				string[] buttonLabels = nameText[1].Split(new string[]{","},StringSplitOptions.None);

				DialogueFrame menuFrame = new DialogueFrame (portdict["menu"], name.Substring(6), nameText[0]);
				menuFrame.ButtonLabels = buttonLabels;
				menuFrame.Type = DialogueFrame.dftype.choice;
				dfq.enqueue (menuFrame);
				continue;
			}
			
			while (true){
				if (text.Length < maxChar){
					dfq.enqueue ( new DialogueFrame ( portdict[name] , name, text));
					//Debug.Log ("Dframe " + ". " +name + " is speaking. " + text);
					text = "";
					break;
				}
				else {
					bool cont = (isLetterOrNum(text[maxChar]));
					string temp = text.Substring(0, maxChar)+ (cont?"...":"");
					dfq.enqueue ( new DialogueFrame(portdict[name] , name, temp));
					//Debug.Log ("Dframe " + ". " +name + " is speaking. " + temp);
					text = (cont?"...":"")+text.Substring(maxChar);

				}
			}
		}
		
		return dfq;

	}

	bool isLetterOrNum(char c){
		return ((c > 47 && c < 58) || (c > 64 && c < 91) || (c > 96 && c < 123));//ASCII
	}

	//handles menu logic, if a particular dialogue frame has a type of choice
	void HandleMenu(DialogueFrame dframe){
		GameObject[] temp;
		//for menu dframes, charName is the unique menu "ID" which will be used to
		//identify which menu you are trying to reference and the text is used for the
		//menu title

		temp = dbdisplay.updateMenu (dframe);
		for (int i = 0; i < temp.Length; ++i) {

			menuButtons[i] = temp[i].GetComponent<Button>();
			Debug.Log ( (menuButtons[i] == null)? true: false);
		}

		menuButtons [0].onClick.AddListener(delegate {
			menuActions.RunButtonFunction(menuButtonActionDict[dframe.CharName][0]);
		});
		menuButtons [1].onClick.AddListener(delegate {
			menuActions.RunButtonFunction(menuButtonActionDict[dframe.CharName][1]);
		});
		menuButtons [2].onClick.AddListener(delegate {
			menuActions.RunButtonFunction(menuButtonActionDict[dframe.CharName][2]);
		});
		menuButtons [3].onClick.AddListener (delegate {
			menuActions.RunButtonFunction (menuButtonActionDict [dframe.CharName] [3]);
		});

		dbdisplay.HideMenu = false;

		//the buttons themselves should close the menu, after it is clicked and move
		//the queue forward
	}

	public void removeEventListeners(){
		for (int i = 0; i < menuButtons.Length; i++) {
			menuButtons[i].onClick.RemoveAllListeners();
		}
	}
	
	public void Forward(){
		if (log.State == Log.state.closed) {
			if (curDFQ.IsEmpty){
				return;
			}
			if (curDFQ.front ().Type == DialogueFrame.dftype.choice){
				curDFQ.dequeue();
			}else{
				log.insert (curDFQ.dequeue ());
			}

		} 
		else{
			if (log.hasFront()){
				log.moveForward();
				dbdisplay.updateLog( log.read ());
			}
			else{
				log.State = Log.state.closed;
			}
		}

	}

	public void FastForward(){
		if (log.State == Log.state.opened) {
			log.State = Log.state.closed;
			return;
		}

		//FF toggle
		if (!fastForward) {
			fastForward = true;		
		}
		else{
			fastForward = false;
		}
	}

	public void Backward(){
		if (log.State == Log.state.closed) {
			dbdisplay.updateLog(log.read ()); //read() automatically opens the log state
			//LogDisplay(str);
		}
		else{
			if (log.hasBack()){
				log.moveBack();
				dbdisplay.updateLog(log.read ());
			}
		}
	}

	//public methods

	/**
	 * Your code should call this function everytime there is a dialogue
	 * interaction. 
	 **/
	public void SetConvo(string fileName){
		curDFQ = textToDFrames (Resources.LoadAssetAtPath<TextAsset> (fileName));

	}


}

