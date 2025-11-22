using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Collider col;
    private Vector3 startDragPosition;

    void Start()
    {
        col = GetComponent<Collider>();
    }
    private void OnMouseDown()
    {
        startDragPosition = transform.position;
    }
    private void OnMouseDrag()
    {
        transform.position = GetMousePositionInWorldSpace();
    }
    private void OnMouseUp()
    {
        col.enabled = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ICardDropArea cardDropArea))
            {
                cardDropArea.OnCardDrop(this);
            } else
            {
                transform.position = startDragPosition;
            }
        } else
        {
            transform.position = startDragPosition;
        }

        col.enabled = true;
    }

    public Vector3 GetMousePositionInWorldSpace()
    {
        // Keeps the card at its current depth
        float z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, z));
        return p;
    }

}
