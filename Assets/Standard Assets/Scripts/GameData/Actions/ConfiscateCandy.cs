using System;
using UnityEngine;
using System.Collections.Generic;
public class ConfiscateCandy : GoapAction
{
	public bool takenCandy = false;
	private CandyCollector targetKid;
	
	private float startTime = 0;
	public float workDuration = 5; 
	
	public ConfiscateCandy () {
	
	
		addEffect ("ruinFun", true); 
	}
	
	
	public override void reset ()
	{
		takenCandy = false;
		targetKid = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return takenCandy;
	}
	
	public override bool requiresInRange ()
	{
		return true; // yes we need to embrace the kid
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest chopping block that we can chop our wood at
		CandyCollector[] Damnkids = (CandyCollector[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(CandyCollector) );

		CandyCollector closest = null;
		float closestDist = 0;
		
		foreach (CandyCollector evilChild in Damnkids) {
			if (!evilChild.GetComponent<CandyBag>().Spanked){
				if (closest == null) {
					// first one, so choose it for now
				if (evilChild != this)
					closest = evilChild;
					closestDist = (evilChild.gameObject.transform.position - agent.transform.position).magnitude;
				} else {
					// is this one closer than the last?
					float dist = (evilChild.gameObject.transform.position - agent.transform.position).magnitude;
					if (dist < closestDist) {
						// we found a closer one, use it
						closest = evilChild;
						closestDist = dist;
					}
				}
			}}
		if (closest == null)
			return false;

		targetKid = closest;
		//targetCandyGiver.KidsIGaveCandiesToAlready.Add(this.gameObject.GetComponent<CandyCollector>());
		target = targetKid.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{ target.GetComponent<CandyBag>().Spanked = true;
		//target.GetComponent<GetCandyAction>().reset();
		//target.GetComponent<DropOffCandyAction>().reset();
		//target.GetComponent<GoapAgent>().ResetActions();
	  
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished doing the right thing
			CandyBag backpack = (CandyBag)agent.GetComponent(typeof(CandyBag));
			backpack.numCandy += 5;
			takenCandy = true;
			
		}
		return true;
	}
	
}