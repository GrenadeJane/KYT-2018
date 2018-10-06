using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CopsZone : MonoBehaviour
{

    #region Types
    enum CopState
    {
        Idle = 0,
        GoToHive
    }

    #endregion
    
    #region Events



    #endregion

    #region Parameters

    [SerializeField] float startRange;
    [SerializeField] float timeCheck;
    [SerializeField] float timeToReload;
    [SerializeField] int baseCountCops;
    [SerializeField] int basePoliTest = 10;
    [SerializeField] float legalAlcoholAmount;

    [SerializeField] GameObject mainHive;
    [SerializeField] CopsSwarmManager swarmManager;

    // :: FAKE [Header("test")]
    [SerializeField] GameObject beeContainer;
    #endregion

    #region RuntimeData

    // :: FAKE [Header("test")]
    List<FestBeeSwarm> beeList = new List<FestBeeSwarm>();
    List<FestBeeSwarm> beeListChecking = new List<FestBeeSwarm>();

    int _countPoliTest;

    CopState state = CopState.Idle;
        
    #endregion


    /// <summary>
    ///  Check If a bee // swarmBase enter into the range of the cops
    /// </summary>
    void CheckEnterZone()
    {
        float rangeSqr = startRange;
        foreach (FestBeeSwarm swarm in HiveMain.m_Instance.m_festbeeSwarmList)
        {
            if (swarm.State == FestBeesSwarmState.BeenChecked)
                continue;

            float rangeSqrSwarm = swarm.m_SwarmRadius ;
            Vector3 beePos = swarm.transform.position;
            Vector3 dir = (beePos - transform.position);

            float dis = dir.magnitude;
            bool ischecked = beeListChecking.Contains(swarm);
            if (dis < ( rangeSqr + rangeSqrSwarm ))
            {
                if ( !ischecked && _countPoliTest > 0)
                {
                    beeListChecking.Add(swarm);
                    // bee is in the range
                    swarm.IsChecked();
                    _countPoliTest--;

                    SendCopToPlace(swarm, transform.position + dir.normalized *(dis - rangeSqrSwarm));
                }
               

            } else if (ischecked)
            {
                beeListChecking.Remove(swarm);
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
                cop.SwarmTarget.GoToHive();

                // send back to home
                Debug.Log("count politest" + _countPoliTest);
                if (_countPoliTest <= 0 && state != CopState.GoToHive)
                    GoToHive();
            }
            else
                cop.SwarmTarget.EndChecked();
        });
    }

    /// <summary>
    /// Send the swarmbase to the main hive
    /// </summary>
    void GoToHive()
    {
        state = CopState.GoToHive;
        swarmManager.SetTargetPosition( mainHive.transform.position);
        swarmManager.Swarm.OnTargetReached = null;
        swarmManager.Swarm.OnTargetReached = ReturnFromHive;
    }

    void ReturnFromHive()
    {
        CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(timeToReload), () =>
        {
            _countPoliTest = basePoliTest;
            swarmManager.SetTargetPosition(transform.position);
            swarmManager.Swarm.OnTargetReached = null;
            swarmManager.Swarm.OnTargetReached = EndHiveAction;
        });
    }

    void EndHiveAction()
    {
        state = CopState.Idle;
    }

    #region MonoBehaviour

    void Start ()
    {
        _countPoliTest = basePoliTest;
        swarmManager.Swarm.m_SwarmRadius = startRange;

        beeList = new List<FestBeeSwarm>(GameObject.FindObjectsOfType<FestBeeSwarm>());
	}
    

    void Update ()
    {
        switch( state )
        {
            case CopState.Idle :
                {
                    CheckEnterZone();
                }
                break;
            case CopState.GoToHive:
                {

                }
                break;
        }
    }

    #endregion
}
