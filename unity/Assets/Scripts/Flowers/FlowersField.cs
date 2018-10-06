using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlowersField : MonoBehaviour {


	#region Fields

	#endregion

	#region Properties
	public List<FlowerBase> Flowers { get; private set; }
	#endregion

	#region Methods


	private void Start()
	{
		Flowers = FindObjectsOfType<FlowerBase>().ToList();
	}

	/// <summary>
	/// Returns a flower which is not the target of any swarm
	/// </summary>
	/// <returns>returns a random not target flower or null if there are no flowers available</returns>
	public FlowerBase GetUntargetedFlower()
	{
		FlowerBase flower = null;

		var untargetdFlowerList = Flowers.FindAll(f => f.IsTargeted == false);

		if(untargetdFlowerList.Count > 0)
		{
			flower = untargetdFlowerList[Random.Range(0, untargetdFlowerList.Count)];
		}

		return flower;
		
	}

	#endregion

}
