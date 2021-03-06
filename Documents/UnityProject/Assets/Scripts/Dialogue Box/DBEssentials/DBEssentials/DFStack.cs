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
	/// Stack implementation using list of DialogueFrames.
	/// </summary>
	public class DFStack {
		List<DialogueFrame> DFrameList;

		/// <summary>
		/// Initializes a new instance of the <see cref="DBEssentials.DFStack"/> class.
		/// </summary>
		public DFStack(){
			DFrameList = new List<DialogueFrame>();
		}

		/// <summary>
		/// Push the specified DialogueFrame to the top of the stack.
		/// </summary>
		/// <param name="DFrame">Dialogue frame.</param>
		public void push (DialogueFrame DFrame){
			DFrameList.Add (DFrame);
		}

		/// <summary>
		/// Returns the top element of the stack, without removing it.
		/// </summary>
		public DialogueFrame top (){
			return DFrameList [DFrameList.Count-1];
		}

		/// <summary>
		/// Returns the top element of the stack and removes it.
		/// </summary>
		public DialogueFrame pop (){
			DialogueFrame temp = DFrameList[DFrameList.Count-1];
			DFrameList.RemoveAt(DFrameList.Count-1);
			return temp;
		}

		/// <summary>
		/// Returns the amount of elements currently in the stack.
		/// </summary>
		public int Size(){
			return DFrameList.Count;
		}

		/// <summary>
		/// Gets a value indicating whether this stack is empty.
		/// </summary>
		/// <value><c>true</c> if this stack is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty {
			get{
				return (DFrameList.Count == 0);
			}
			
		}
	}
}

