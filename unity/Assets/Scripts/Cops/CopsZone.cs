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
    [SerializeField] CopsSwarmManager swarmManager;

    // :: FAKE [Header("test")]
    [SerializeField] GameObject beeContainer;
    #endregion

    #region RuntimeData

    // :: FAKE [Header("test")]
    List<FestBeeSwarm> beeList = new List<FestBeeSwarm>();

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
        foreach (FestBeeSwarm swarm in beeList)
        {
            if (swarm.State == FestBeesSwarmState.BeenChecked)
                continue;

            float rangeSqrSwarm = swarm.m_SwarmRadius * swarm.m_SwarmRadius;
            Vector3 beePos = swarm.transform.position;
            Vector3 dir = (beePos - transform.position);
            float dis = dir.sqrMagnitude;
            if (dis < ( rangeSqr + rangeSqrSwarm ) && _countPoliTest > 0)
            {
                // bee is in the range
             //   swarm.State = FestBeesSwarmState.BeenChecked;
                    _countPoliTest--;

                SendCopToPlace(swarm, transform.position + dir.normalized * (Mathf.Sqrt(dis) - Mathf.Sqrt(rangeSqrSwarm)));
                break;
            }
        }
    }

    void SendCopToPlace(FestBeeSwarm swarm, Vector3 targetPos)
    {
        SwarmObjectCop cop = ((swarmManager.Swarm) as SwarmBaseCop).GetRandomnlyObject();
        if ( cop != null )
        {
            cop.AssignNewTarget(targetPos);
            cop.SwarmTarget = swarm;

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
            //   cop.SwarmTarget.State = FestBeesSwarmState.GoBackToHive;
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

        beeList = new List<FestBeeSwarm>(GameObject.FindObjectsOfType<FestBeeSwarm>());
	}
    

    void Update ()
    {
        if ( !isGoingBackToHive )
            CheckEnterZone();
    }

    #endregion
}
