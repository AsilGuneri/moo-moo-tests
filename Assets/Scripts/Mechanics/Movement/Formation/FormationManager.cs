using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class FormationManager : Singleton<FormationManager>
{
    public List<FormationBase> Formations { get => formations; }

    [SerializeField] private List<FormationBase> formations;

    private void Start()
    {
        InitializeFormations();
    }

    private void InitializeFormations()
    {
        foreach (var formation in Formations)
        {
            formation.ResetFormationPoints();
        }
    }
    public bool IsFormationAvailable(MinionType minionType)
    {
        var formation = GetFormation(minionType);
        foreach(var point in formation.FormationPoints)
        {
            if (point.isOccupied == false)
            {
                return true;
            }
        }
        return false;
    }
    public void MoveFormationToMousePos(MinionType minionType)
    {
        var formation = GetFormation(minionType);
        



    }
    public FormationPoint UseAvailablePoint(MinionType type)
    { 
        var point = GetFirstAvailablePoint(type);
        point.isOccupied = true;
        return point;
    }
    public void LeavePoint(FormationPoint point)
    {
        point.isOccupied = false;
    }
    private FormationPoint GetFirstAvailablePoint(MinionType type)
    {
        var formation = GetFormation(type);
        foreach(var point in formation.FormationPoints)
        {
            if (!point.isOccupied)
            {
                return point;
            }
        }
        Debug.LogError("Formation point returned null");
        return null;
    }
    private FormationBase GetFormation(MinionType type)
    {
        foreach (var formation in Formations)
        {
            if (formation.MinionType == type)
            {
                return formation;
            }
        }
        Debug.LogError("Formation returned null");
        return null;
    }
}
