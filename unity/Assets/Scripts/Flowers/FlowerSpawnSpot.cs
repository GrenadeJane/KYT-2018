using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawnSpot : MonoBehaviour {

	#region Fields

	private FlowerBase m_Flower;

	#endregion

	#region Properties

	public FlowerBase Flower {
		get
		{
			return m_Flower;
		}
		set
		{
			m_Flower = value;
			if(m_Flower != null)
			{
				m_Flower.transform.position = transform.position;
				m_Flower.transform.rotation = transform.rotation;
				OccupiedByFlower = true;
			}
			else
			{
				OccupiedByFlower = false;
			}

		}
	}
	public bool OccupiedByFlower { get; set; }

	#endregion
}
