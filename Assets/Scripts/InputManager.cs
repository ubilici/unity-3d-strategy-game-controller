using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public List<Unit> selectedUnits;

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
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(selectedUnits.Count != 0)
                {
                    foreach (Unit unit in selectedUnits)
                    {
                        unit.Move(hit.point);
                    }
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

}
