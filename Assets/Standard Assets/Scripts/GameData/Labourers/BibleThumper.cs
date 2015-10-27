using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BibleThumper : CandyLabourer
{
	
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
		goal.Add(new KeyValuePair<string, object>("ruinFun", true ));
		return goal;
	}
}

