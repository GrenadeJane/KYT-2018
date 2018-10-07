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
        // force positive position
        pos.y = heightCollider;

        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;

        int layerground = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hit, layerground))
        {
            pos.y = (hit.point + Vector3.up * heightCollider).y;
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