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
    [SerializeField] float timeCheck;
    [SerializeField] float timeToReload;
    [SerializeField] int baseCountCops;
    [SerializeField] float legalAlcoholAmount;

    [SerializeField] GameObject mainHive;
    [SerializeField] SwarmManager swarmManager;

    // :: FAKE [Header("test")]
    [SerializeField] GameObject beeContainer;
    #endregion

    #region RuntimeData

    // :: FAKE [Header("test")]
    List<SwarmManager> beeList = new List<SwarmManager>();

    int _countPoliTest = 1;
    public int CountPoliTest
    {
        get { return _countPoliTest;  }
        set { _countPoliTest = value;  }
    }

    bool isGoingBackToHive = false;

    #endregion


    /// <summary>
    ///  Check If a bee // swarmBase enter into the range of the cops
    /// </summary>
    void CheckEnterZone()
    {
        float rangeSqr = startRange * startRange; 
        //foreach ( GameObject bee in beeList)
        //{
        //    Vector3 beePos = bee.transform.position;
        //    float dis = (beePos - transform.position).sqrMagnitude;
        //    if (dis < rangeSqr && _countPoliTest > 0)
        //    {
        //        // bee is in the range

        //        _countPoliTest--;

        //        SendCopToPlace();
        //        break;
        //    }
        //}
    }

    void SendCopToPlace()
    {
        SwarmObjectCop cop = ((swarmManager.Swarm) as SwarmBaseCop).GetRandomnlyObject();
        if ( cop != null )
        {
            cop.AssignNewTarget(Vector3.zero);

            cop.OnTargetReached = null;
            cop.OnTargetReached += ProceedToCheck;
        }
    }

    /// <summary>
    /// Get the amount of alcool in the swarm + stop them to move
    /// </summary>
    void ProceedToCheck(SwarmObject swarmObject) // param current swarm
    {
        SwarmObjectCop cop = swarmObject as SwarmObjectCop;

        CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(timeCheck), () =>
        {
            cop.GoesToUniqueTarget = false;
            // :: Remove 
            float alcoholAmount = Random.Range(legalAlcoholAmount, legalAlcoholAmount + 3);
            if (alcoholAmount >= legalAlcoholAmount)
            {
                // send back to home
                if (_countPoliTest <= 0 && !isGoingBackToHive)
                    GoToHive();
            }
        });
    }

    /// <summary>
    /// Send the swarmbase to the main hive
    /// </summary>
    void GoToHive()
    {
        isGoingBackToHive = true;
        swarmManager.SetTargetPosition(mainHive.transform.position);
        swarmManager.Swarm.OnTargetReached = null;
        swarmManager.Swarm.OnTargetReached = ReturnFromHive;
    }

    void ReturnFromHive()
    {
        CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(timeToReload), () =>
        {
            CountPoliTest = 1;
            swarmManager.SetTargetPosition(transform.position);
            swarmManager.Swarm.OnTargetReached = null;
            swarmManager.Swarm.OnTargetReached = EndHiveAction;
        });
    }

    void EndHiveAction()
    {
        isGoingBackToHive = false;
    }

    #region MonoBehaviour

    void Start ()
    {
        swarmManager.Swarm.m_SwarmRadius = startRange;

     //   beeList = GameObject.FindWithTag("Bee").GetComponentsInChildren<SwarmManager>();
        //foreach (Transform child in beeContainer.transform)
        //{
        //    beeList.Add(child.gameObject);
        //}
	}
    

    void Update ()
    {
        if ( !isGoingBackToHive )
            CheckEnterZone();
    }

    #endregion
}
