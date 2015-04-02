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
	/// I just learned this is called a Deque...
	/// Extends DFQueue. Allows enqueueing elements from the front of the queue and dequeueing from the back.
	/// </summary>
	public class ReversableDFQueue : DFQueue {

		/// <summary>
		/// Initializes a new instance of the <see cref="DBEssentials.ReversableDFQueue"/> class.
		/// </summary>
		public ReversableDFQueue (){}

		/// <summary>
		/// Enqueues an element to the front of the queue.
		/// </summary>
		/// <param name="DFrame">D frame.</param>
		public void reverseDequeue(DialogueFrame DFrame){
			DFrameList.Insert (0, DFrame);
		}

		/// <summary>
		/// Dequeues an element at the back of the queue.
		/// </summary>
		/// <returns>DialogueFrame at the back of the queue</returns>
		public DialogueFrame reverseEnqueue(){
			DialogueFrame temp = DFrameList [DFrameList.Count - 1];
			DFrameList.RemoveAt (DFrameList.Count - 1);
			return temp;
		}

		public ReversableDFQueue Copy(){
			ReversableDFQueue temp = new ReversableDFQueue();
			for (int i = 0; i < DFrameList.Count; ++i) {
				temp.enqueue (new DialogueFrame(DFrameList[i].Portrait, DFrameList[i].CharName,DFrameList[i].DialogueText));
			}
			return temp;
		}
	}
}

