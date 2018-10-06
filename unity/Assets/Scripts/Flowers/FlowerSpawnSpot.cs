﻿using System.Collections;
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
			m_Flower.transform.position = transform.position;
			m_Flower.transform.rotation = transform.rotation;
		}
	}
	public bool OccupiedByFlower { get; set; }

	#endregion
}
