using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormationController : BoxFormation
{
    public List<FormationPoint> FormationPoints { get => formationPoints; }
    private List<FormationPoint> formationPoints;
    private List<Vector3> points;

    private void Start()
    {
        points = EvaluatePoints().ToList();
        formationPoints = points.Select(point => new FormationPoint(point, false)).ToList();
    }

    [ButtonMethod]
    public void MoveToPositions()
    {
        var units = UnitManager.Instance.WaveEnemies;
        for (int i = 0; i < units.Count; i++)
        {
            if (formationPoints[i] == null) return;

            formationPoints[i].isOccupied = true;

            var controller = units[i].Value.gameObject.GetComponent<UnitController>();
            var brain = units[i].Value.gameObject.GetComponent<EnemyBrain>();

            brain.SetBrainActive(false);
            controller.GetComponent<Movement>().ClientMove(Extensions.To2D(formationPoints[i].position));

        }
    }
}

public class FormationPoint
{
    public Vector3 position;
    public bool isOccupied = false;

    public FormationPoint(Vector3 position, bool isOccupied)
    {
        this.position = position;
        this.isOccupied = isOccupied;
    }
}
