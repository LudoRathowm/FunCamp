using System;
using UnityEngine;
using System.Collections.Generic;
public class GoCryToMommyAction : GoapAction
{
	private bool cried = false;
	private MommyHouse myMom;
	
	private float startTime = 0;
	public float workDuration = 1; 
	
	public GoCryToMommyAction () {
		addPrecondition ("inPunishment", true); // first get spanked
		addEffect ("inPunishment", false); // don't be a jew

	}
	
	
	public override void reset ()
	{
		cried = false;
		myMom = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return cried;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near mommy
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		
		MommyHouse[] GenerousPeople = (MommyHouse[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(MommyHouse));
		MommyHouse closest = null;
		float closestDist = 0;
		
		foreach (MommyHouse giver in GenerousPeople) {

				if (closest == null) {
					// first one, so choose it for now
					closest = giver;
					closestDist = (giver.gameObject.transform.position - agent.transform.position).magnitude;
				} else {
					// is this one closer than the last?
					float dist = (giver.gameObject.transform.position - agent.transform.position).magnitude;
					if (dist < closestDist) {
						// we found a closer one, use it
						closest = giver;
						closestDist = dist;
					}
				}
			}
		if (closest == null)
			return false;
		
		myMom = closest;

		target = myMom.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished crying

			cried = true;
			
		}
		return true;
	}
	
}