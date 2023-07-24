using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class FormationBase : MonoBehaviour
{
    public Color editorColor;

    public MinionType MinionType { get => minionType; }
    public List<FormationPoint> FormationPoints { get => formationPoints; }

    [SerializeField][Range(0, 1)] protected float _noise = 0;
    [SerializeField] protected float Spread = 1;

    [SerializeField] protected MinionType minionType;
    protected List<FormationPoint> formationPoints = new List<FormationPoint>();

    public abstract IEnumerable<Vector3> EvaluatePoints();

    public Vector3 GetNoise(Vector3 pos)
    {
        var noise = Mathf.PerlinNoise(pos.x * _noise, pos.z * _noise);

        return new Vector3(noise, 0, noise);
    }
    public virtual void ResetFormationPoints()
    {
        formationPoints.Clear();
        var points = EvaluatePoints().ToList();
        foreach(var point in points)
        {
            formationPoints.Add(new FormationPoint(point, false));
        }
    }
    public async void MoveFormation(Vector3 position, float speed = 5)
    {
        var distance = Extensions.GetDistance(transform.position, position);
        var time = distance / speed;
        float counter = 0;
        int delay = 500;
        while (counter < time)
        {
            counter += delay;
            transform.position = Vector3.Lerp(transform.position, position, counter / time);
            ResetFormationPoints();
            await Task.Delay(delay);
        }
    }
    // New Method
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = editorColor;
        foreach (var point in EvaluatePoints())
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }
}
[Serializable]
public class FormationPoint
{
    public Vector3 position;
    public bool isOccupied;
    public FormationPoint(Vector3 position, bool isOccupied)
    {
        this.position = position;
        this.isOccupied = isOccupied;
    }
}
public enum MinionType
{
    Basic,
    Guardian,
    Attacker,
    Commander
}