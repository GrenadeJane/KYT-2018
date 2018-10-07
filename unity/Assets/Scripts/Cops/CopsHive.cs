using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CopsHive : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] BoxCollider collider;

    float heightCollider;
    public void CheckPosition(Vector3 pos)
    {
        if (pos.y < heightCollider * 2)
            pos.y = heightCollider * 2;
        else
        {
            RaycastHit hit;
               Ray ray = new Ray(pos, Vector3.down);
               Debug.Log("pos" + (pos));
            int layerground = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out hit, layerground))
               {
                Debug.DrawLine(pos, hit.point, Color.red, 3);
                pos.y = (hit.point + Vector3.up * heightCollider).y;
                   Debug.Log("y" + (hit.point));
               }
           }

        transform.position = Vector3.Lerp(transform.position, pos, 0.2F);
    }
    // Use this for initialization
    void Start()
    {
        heightCollider = collider.center.y + collider.size.y / 2;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BuildingBase.OnBuildingClick.Invoke(gameObject);
    }
}