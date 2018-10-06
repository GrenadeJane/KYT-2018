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

    [SerializeField] GameObject mainHive;
    [SerializeField] SwarmManager swarmManager;

    // :: FAKE [Header("test")]
    [SerializeField] GameObject beeContainer;
    #endregion

    #region RuntimeData

    // :: FAKE [Header("test")]
    List<GameObject> beeList = new List<GameObject>();

    int _countPoliTest = 1;
    public int CountPoliTest
    {
        get { return _countPoliTest;  }
        set { _countPoliTest = value; if (_countPoliTest <= 0) GoToHive(); }
    }

    bool isGoingBackToHive = false;

    #endregion


    /// <summary>
    ///  Check If a bee // swarmBase enter into the range of the cops
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

    /// <summary>
    /// Get the amount of alcool in the swarm + stop them to move
    /// </summary>
    void ProceedToCheck() // param current swarm
    {
        // :: Remove 
        float alcoholAmount = Random.Range(legalAlcoholAmount, legalAlcoholAmount + 3);
        if (alcoholAmount >= legalAlcoholAmount)
        {
            // send back to home
            CountPoliTest--;
        }
    }

    /// <summary>
    /// Send the swarmbase to the main hive
    /// </summary>
    void GoToHive()
    {
        isGoingBackToHive = true;
        swarmManager.SetTargetPosition(mainHive.transform.position);
    }

    void ReturnFromHive()
    {

    }

    #region MonoBehaviour

    void Start () {

        foreach (Transform child in beeContainer.transform)
        {
            beeList.Add(child.gameObject);
        }
	}
    

    void Update ()
    {
        if ( !isGoingBackToHive )
            CheckEnterZone();
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, startRange);
    }

    #endregion
}
