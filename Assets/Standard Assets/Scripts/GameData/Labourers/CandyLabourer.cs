using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

/**
 * A general labourer class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 */
public abstract  class CandyLabourer : MonoBehaviour, IGoap
{   List <CandyGiverComponent> PeopleThatGaveCandyAlready = new List<CandyGiverComponent>();
   bool imcalculatingthepathnigger = false;
	public CandyBag backpack;
	public float moveSpeed = 15;
	public Vector3 targetPosition;
	private Seeker seeker;
	private CharacterController controller;
	//The calculated path
	public Path path = null;
	//The AI's speed per second
	public float speed = 100;
	public float nextWaypointDistance = .3f;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	void Start ()
	{	seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		if (backpack == null)
			backpack = gameObject.AddComponent <CandyBag>( ) as CandyBag;
	}
	
	
	public void Update () {
		
	}
	/**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
	public HashSet<KeyValuePair<string,object>> getWorldState () {
		HashSet<KeyValuePair<string,object>> worldData = new HashSet<KeyValuePair<string,object>> ();
		
		worldData.Add(new KeyValuePair<string, object>("hasCandy", (backpack.numCandy > 0) ));

		return worldData;
	}
	
	/**
	 * Implement in subclasses
	 */
	public abstract HashSet<KeyValuePair<string,object>> createGoalState ();
	
	
	public void planFailed (HashSet<KeyValuePair<string, object>> failedGoal)
	{
		// Not handling this here since we are making sure our goals will always succeed.
		// But normally you want to make sure the world state has changed before running
		// the same goal again, or else it will just fail.
	}
	
	public void planFound (HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
	{
		// Yay we found a plan for our goal
		Debug.Log ("<color=green>Plan found</color> "+GoapAgent.prettyPrint(actions));
	}
	
	public void actionsFinished ()
	{
		// Everything is done, we completed our actions for this gool. Hooray!
		Debug.Log ("<color=blue>Actions completed</color>");
	}
	
	public void planAborted (GoapAction aborter)
	{
		// An action bailed out of the plan. State has been reset to plan again.
		// Take note of what happened and make sure if you run the same goal again
		// that it can succeed.
		Debug.Log ("<color=red>Plan Aborted</color> "+GoapAgent.prettyPrint(aborter));
	}
	
	public bool moveAgent(GoapAction nextAction) {
		if (path!=null){

			gameObject.transform.LookAt( path.vectorPath[currentWaypoint]);
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, path.vectorPath[currentWaypoint], moveSpeed*Time.deltaTime);
			//Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			//dir *= speed * Time.deltaTime;
			//controller.SimpleMove (dir);
			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
				currentWaypoint++;
				//	return;
			}
		}
		if (path == null && !imcalculatingthepathnigger){
			imcalculatingthepathnigger = true;
			seeker.StartPath (transform.position, nextAction.target.transform.position, OnPathComplete);}
		targetPosition=nextAction.target.transform.position;
		//		PathRequestManager.RequestPath(transform.position,nextAction.target.transform.position, OnPathFound);
		// move towards the NextAction's target
		//	float step = moveSpeed * Time.deltaTime;
		//	gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);
		//		Debug.Log ("target:"+nextAction.target.gameObject + " position: "+nextAction.target.transform.position+" and distant:" +Vector2.Distance(gameObject.transform.position, nextAction.target.transform.position));
		Debug.Log(Vector2.Distance(gameObject.transform.position, nextAction.target.transform.position));
		
		if (Vector2.Distance(gameObject.transform.position, nextAction.target.transform.position) < 0.5f ) {
			Debug.Log ("ARRIVED");
			path=null;

			currentWaypoint = 0;
			imcalculatingthepathnigger = false;
			//			Debug.Log("walao");
			// we are at the target location, we are done
			nextAction.setInRange(true);

			return true;
		} else{
			return false;}
		
		//		if (path == null) {
		//			//We have no path to move after yet
		//			return;
		//		}
		//		if (currentWaypoint >= path.vectorPath.Count) {
		//			Debug.Log ("End Of Path Reached");
		//			return;
		//		}
		//Direction to the next waypoint
		
	}
	public void OnPathComplete (Path p) {
		Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			imcalculatingthepathnigger =false;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
}