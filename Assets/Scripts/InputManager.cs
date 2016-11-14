using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public List<Unit> selectedUnits;
    public Transform collisionIndicator;

    private Vector3 startPoint;
    private Vector3 endPoint;

    private Vector3 center;
    private Vector3 halfExtends;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform selectedTransform = DetectObject().transform;
            if(selectedTransform != null)
            {
                if (selectedTransform.GetComponent<Unit>())
                {
                    // Multiselect
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        SelectUnit(selectedTransform.GetComponent<Unit>());
                    }
                    // Deselect rest
                    else
                    {
                        DeselectAllUnits();
                        SelectUnit(selectedTransform.GetComponent<Unit>());
                    }
                }
                else
                {
                    DeselectAllUnits();
                }
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                startPoint = hit.point;
                startPoint.y = 0;
            }
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                endPoint = hit.point;
                endPoint.y = 0;
            }

            center = (endPoint + startPoint) / 2;
            halfExtends = new Vector3(Mathf.Abs(endPoint.x - center.x), 10, Mathf.Abs(endPoint.z - center.z));

            SetCollisionIndicatorSize(halfExtends);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(startPoint.x - endPoint.x) > 0.05 && Mathf.Abs(startPoint.z - endPoint.z) > 0.05)
            {
                SelectArea(center, halfExtends);
            }

            collisionIndicator.localScale = Vector3.zero;
            startPoint = Vector3.zero;
            endPoint = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Transform selectedTransform = DetectObject().transform;
            if (selectedTransform.GetComponent<Unit>())
            {
                if (selectedTransform.GetComponent<Unit>().unitFaction == UnitFaction.Enemy)
                {
                    foreach (Unit selectedUnit in selectedUnits)
                    {
                        selectedUnit.Attack(selectedTransform.GetComponent<Unit>());
                    }
                }
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (selectedUnits.Count != 0)
                    {
                        foreach (Unit unit in selectedUnits)
                        {
                            unit.Move(hit.point);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(selectedUnits.Count > 0)
            {
                foreach (Unit selectedUnit in selectedUnits)
                {
                    selectedUnit.Stop();
                }
            }
        }
    }

    RaycastHit DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, 100);

        return hit;
    }

    void DeselectAllUnits()
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Disable();
        }
        selectedUnits.Clear();
    }

    void SelectUnit(Unit selectedUnit)
    {
        if (selectedUnit.unitFaction == UnitFaction.Ally)
        {
            selectedUnits.Add(selectedUnit);
            selectedUnit.Enable();
        }
    }

    void SetCollisionIndicatorSize(Vector3 indicatorSize)
    {
        indicatorSize.y = 0.3f;
        indicatorSize.x = Mathf.Abs(indicatorSize.x);
        indicatorSize.z = Mathf.Abs(indicatorSize.z);

        indicatorSize *= 2;

        collisionIndicator.localScale = indicatorSize;
        collisionIndicator.position = center;
    }

    void SelectArea(Vector3 center, Vector3 halfExtends)
    {
        DeselectAllUnits();
        Collider[] collidedUnits = Physics.OverlapBox(center, halfExtends);
        foreach (var collision in collidedUnits)
        {
            if (collision.GetComponent<Unit>())
            {
                SelectUnit(collision.GetComponent<Unit>());
            }
        }
    }

}
