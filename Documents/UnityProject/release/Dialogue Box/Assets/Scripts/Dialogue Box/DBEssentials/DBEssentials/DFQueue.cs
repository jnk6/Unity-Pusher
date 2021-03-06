//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;


namespace DBEssentials
{
	/// <summary>
	/// Queue implementation using list of DialogueFrames.
	/// </summary>
	public class DFQueue {
		protected List<DialogueFrame> DFrameList;

		/// <summary>
		/// Initializes a new instance of the <see cref="DBEssentials.DFQueue"/> class.
		/// </summary>
		public DFQueue (){
			DFrameList = new List<DialogueFrame>();
		}

		/// <summary>
		/// Enqueue the specified DialogueFrame.
		/// </summary>
		/// <param name="DFrame">Dialogue frame.</param>
		public void enqueue (DialogueFrame DFrame){
			DFrameList.Add (DFrame);
		}

		/// <summary>
		/// Returns the DialogueFrame element at the front of the queue, without removing it.
		/// </summary>
		public DialogueFrame front (){
			return DFrameList[0];
		}

		/// <summary>
		/// Dequeue the front element.
		/// </summary>
		public DialogueFrame dequeue (){
			
			DialogueFrame temp = DFrameList [0];
			DFrameList.RemoveAt (0);
			return temp;
		}

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public int Size {
			get {
				return DFrameList.Count;
			}
			
		}

		public DFQueue Copy(){
			DFQueue temp = new DFQueue();
			for (int i = 0; i < DFrameList.Count; ++i) {
				temp.enqueue (new DialogueFrame(DFrameList[i].Portrait, DFrameList[i].CharName,DFrameList[i].DialogueText));
			}
			return temp;
		}

		/// <summary>
		/// Gets a value indicating whether the queue is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty {
			get {
				return (DFrameList.Count == 0);
			}
			
		}
	}
}

