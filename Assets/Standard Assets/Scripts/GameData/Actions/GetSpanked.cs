
using System;
using UnityEngine;
using System.Collections.Generic;
public class GetSpanked : GoapAction
{
	private bool gotSpanked = false;

	
	private float startTime = 0;
	public float workDuration = 2; 
	
	public GetSpanked () {
		addPrecondition ("inPunishment", true); // don't be a jew
		addEffect ("inPunishment", false);
	}
	
	
	public override void reset ()
	{
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return gotSpanked;
	}
	
	public override bool requiresInRange ()
	{
		return false; 
	}
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		
	
		return true;
	}

	
	public override bool perform (GameObject agent)
	{
		//this.gameObject.GetComponent<CandyBag>().Spanked =false;

		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished begging
			CandyBag backpack = (CandyBag)agent.GetComponent(typeof(CandyBag));
			backpack.numCandy = 0;
			backpack.Spanked = false;
			gotSpanked = true;
			Debug.Log("GOT SPANKED");
		}
		return true;
	}
	
}