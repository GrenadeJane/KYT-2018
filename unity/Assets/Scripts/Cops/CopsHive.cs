using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CopsHive : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] BoxCollider collider;

    float heightCollider;
    public void CheckPosition(Ray ray)
    {
        // force positive position

        Vector3 pos = transform.position;
        RaycastHit hit;

        int layerground = LayerMask.GetMask("Floor");
        if (Physics.Raycast(ray, out hit, layerground))
        {
            pos = hit.point;
            pos.y = (hit.point + Vector3.up * heightCollider).y;
        }

        int layerMaskBounds = LayerMask.GetMask("Bounds");
        if (Physics.Raycast(ray, out hit, layerMaskBounds))
        {
            RaycastHit hitBound;
            Ray bound = new Ray(hit.point + hit.normal * 2.0f, Vector3.down);
            if (Physics.Raycast(bound, out hitBound, layerground))
            {
                pos = hitBound.point;
                Debug.Log("place building");
                pos.y = (hitBound.point + Vector3.up * heightCollider).y;
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