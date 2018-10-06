using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxDanceFloor : MonoBehaviour
{
    public Material[] materials;
    public float delaySwitch = 1f;

    public Color[] colors;

    float timing = 0f;

	void Start ()
    {
        if(materials.Length == 0)
        {
            Renderer renderer = this.GetComponent<Renderer>();
            materials = renderer.materials;
        }
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timing += Time.deltaTime;

        if(timing >= delaySwitch)
        {
            timing = 0f;
            for (int i = 0; i < materials.Length; i++)
            {
                int iRandom = Random.Range(0,colors.Length);
                materials[i].SetColor("_EmissionColor",colors[iRandom]);
            }
        }
    }
}
