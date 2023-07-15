using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FormationBase : MonoBehaviour
{
    public List<FormationPoint> FormationPoints { get => formationPoints; }
    public string FormationName { get => formationName; }

    [SerializeField] protected string formationName;
    [SerializeField][Range(0, 1)] protected float _noise = 0;
    [SerializeField] protected float Spread = 1;

    protected List<FormationPoint> formationPoints;

    public abstract IEnumerable<Vector3> EvaluatePoints();

    public Vector3 GetNoise(Vector3 pos)
    {
        var noise = Mathf.PerlinNoise(pos.x * _noise, pos.z * _noise);

        return new Vector3(noise, 0, noise);
    }
    public virtual void InitializeFormation()
    {
        var points = EvaluatePoints().ToList();
        formationPoints = points.Select(point => new FormationPoint(point, null)).ToList();
    }
    // New Method
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var point in EvaluatePoints())
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }
}

public class FormationPoint
{
    public Vector3 position;
    public Transform tenant;
    public FormationPoint(Vector3 position, Transform tenant)
    {
        this.position = position;
        this.tenant = tenant;
    }
}
