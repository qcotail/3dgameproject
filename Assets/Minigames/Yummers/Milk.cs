using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    [SerializeField] LevelTemplate lvltmp;

    private Collider col;
    private Vector3 startDragPosition;
    private bool isDragging = false;
    public GameObject currentObject;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        if (lvltmp.CanPlay())
        {
            startDragPosition = currentObject.transform.position;
            isDragging = true;
        }
        else
        {
            isDragging = false;
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        currentObject.transform.position = GetMousePositionInWorldSpace();
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        col.enabled = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out IMilkDropArea milkDropArea))
            {
                milkDropArea.OnMilkDrop(this);
            }
            else
            {
                currentObject.transform.position = startDragPosition;
            }
        }
        else
        {
            currentObject.transform.position = startDragPosition;
        }

        col.enabled = true;
        isDragging = false;
    }

    public Vector3 GetMousePositionInWorldSpace()
    {
        float z = Camera.main.WorldToScreenPoint(currentObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, z)
        );
    }

    public Vector3 GetStartPosition()
    {
        return startDragPosition;
    }
}
