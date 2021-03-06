﻿
using System;
using UnityEngine;

public class DropOffCandyAction : GoapAction
{
	private bool droppedOffCandy = false;
	private CandyStash CandyStash;
	
	public DropOffCandyAction () {
		addPrecondition ("hasCandy", true); 
		addPrecondition ("inPunishment", false); // first get spanked
		addEffect ("hasCandy", false); 
		addEffect ("collectCandy", true); 
	}
	
	
	public override void reset ()
	{
		droppedOffCandy = false;
		CandyStash = null;
	}
	
	public override bool isDone ()
	{
		return droppedOffCandy;
	}
	
	public override bool requiresInRange ()
	{
		return true; 
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
	//	Debug.LogError("DOES THIS EVEN RUN");
		CandyStash[] candystocker = (CandyStash[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(CandyStash) );

		CandyStash closest = null;
		float closestDist = 0;
		
		foreach (CandyStash supply in candystocker) {
			if (closest == null) {

				closest = supply;

				closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
			} else {

				float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					Debug.LogError("closest2"+closest.gameObject);
					closest = supply;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;
		
		CandyStash = closest;
		target = CandyStash.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		CandyBag backpack = (CandyBag)agent.GetComponent(typeof(CandyBag));
		if (backpack.numCandy == null) backpack.numCandy = 0;
		target.GetComponent<CandyStash>().numCandy +=backpack.numCandy;
		//CandyStash.numCandy += backpack.numCandy;
		droppedOffCandy = true;
		backpack.numCandy = 0;
		
		return true;
	}
}