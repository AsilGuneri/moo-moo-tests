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
            formation.InitializeFormation();
        }
    }
    public Vector3 UseAvailablePoint(string formationName,Transform userTransform)
    {
        var point = GetFirstAvailablePoint(formationName);
        point.tenant = userTransform;
        return point.position;
    }
    private FormationPoint GetFirstAvailablePoint(string formationName)
    {
        var formation = GetFormation(formationName);
        foreach(var point in formation.FormationPoints)
        {
            if (point.tenant == null)
            {
                return point;
            }
        }
        Debug.LogError("Formation point returned null");
        return null;
    }
    private FormationBase GetFormation(string name)
    {
        foreach (var formation in Formations)
        {
            if (formation.FormationName == name)
            {
                return formation;
            }
        }
        Debug.LogError("Formation returned null");
        return null;
    }
}
