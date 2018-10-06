using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CopsZone : MonoBehaviour
{

    #region Events



    #endregion

    #region Parameters

    [SerializeField] float startRange;
    [SerializeField] int baseCountCops;
    [SerializeField] float legalAlcoholAmount;

    [SerializeField] GameObject hive;

    [Header("test")]
    [SerializeField] GameObject beeContainer;
    #endregion

    #region RuntimeData

    List<GameObject> beeList = new List<GameObject>();
    List<CopsGesture> copsList;

    int _countPoliTest;
    public int CountPoliTest
    {
        get { return _countPoliTest;  }
        set { _countPoliTest = value; if (_countPoliTest <= 0) GoToHive(); }
    }


    #endregion



    /// <summary>
    ///  
    /// </summary>
    void CheckEnterZone()
    {
        float rangeSqr = startRange * startRange; 
        foreach ( GameObject bee in beeList)
        {
            Vector3 beePos = bee.transform.position;
            float dis = (beePos - transform.position).sqrMagnitude;
            if (dis < rangeSqr)
            {
                // bee is in the range
                bee.GetComponent<MeshRenderer>().material.color = Color.red; // bee control
                ProceedToCheck();
            }
        }
    }


    // Get the amount of alcool in the swarm + stop them to move
    void ProceedToCheck() // param current swarm
    {
        // :: Remove 
        float alcoholAmount = Random.Range(0.0f, legalAlcoholAmount + 3);
        if (alcoholAmount >= legalAlcoholAmount)
        {
            // send back to home
            CountPoliTest--;
        }
    }


    void GoToHive()
    {
        // move the swarm to hive
    }


    // Use this for initialization
    void Start () {

        foreach (Transform child in beeContainer.transform)
        {
            beeList.Add(child.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        CheckEnterZone();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, startRange);
    }
}
