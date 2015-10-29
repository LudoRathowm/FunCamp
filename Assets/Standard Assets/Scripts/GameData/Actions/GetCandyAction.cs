
using System;
using UnityEngine;
using System.Collections.Generic;
public class GetCandyAction : GoapAction
{
	private bool givenCandy = false;
	private CandyGiverComponent targetCandyGiver; //yum
	
	private float startTime = 0;
	public float workDuration = 1; 
	
	public GetCandyAction () {
		addPrecondition ("inPunishment", false); // first get spanked
		addPrecondition ("hasCandy", false); // don't be a jew
		addEffect ("hasCandy", true);
	}
	
	
	public override void reset ()
	{
		givenCandy = false;
		targetCandyGiver = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return givenCandy;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to be near a generous person
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{

		CandyGiverComponent[] GenerousPeople = (CandyGiverComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(CandyGiverComponent));
		CandyGiverComponent closest = null;
		float closestDist = 0;
		
		foreach (CandyGiverComponent giver in GenerousPeople) {
			if (!giver.KidsIGaveCandiesToAlready.Contains(this.gameObject.GetComponent<CandyCollector>())){
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
			}}
		if (closest == null)
			return false;
		
		targetCandyGiver = closest;
		targetCandyGiver.KidsIGaveCandiesToAlready.Add(this.gameObject.GetComponent<CandyCollector>());
		target = targetCandyGiver.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{Debug.Log("GETCANDY");
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished begging
			CandyBag backpack = (CandyBag)agent.GetComponent(typeof(CandyBag));
			backpack.numCandy += 5;
			givenCandy = true;

		}
		return true;
	}
	
}
