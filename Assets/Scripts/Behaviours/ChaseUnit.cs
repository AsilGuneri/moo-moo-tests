using System.Text;
using Bonsai.Core;
using UnityEngine;

namespace Bonsai.Standard
{
    [BonsaiNode("Tasks/", "Timer")]
    public class ChaseUnit : Task
    {
        [SerializeField] private int timeOutLoopCount;
        private TargetController targetController;
        private bool isSearching = false;
        private int counter = 0;
        public override void OnStart()
        {
            targetController = Actor.GetComponent<TargetController>();
        }
        public override void OnEnter()
        {
            Debug.Log("TestBehaviourEnter");
        }
        public override Status Run()
        {
            GameObject closestEnemy = UnitManager.Instance.GetClosestUnit(Actor.transform.position);
            targetController.Target = closestEnemy;
            return Status.Success;

            if (targetController.HasTarget) { Debug.Log("Return successs"); return Status.Success;  }
            if (!targetController.HasTarget && isSearching) { Debug.Log("Return running"); return Status.Running; }
            if (!targetController.HasTarget && counter >= timeOutLoopCount) { Debug.Log("Return failure"); return Status.Failure; }
            Debug.Log("Return failure failsafe"); return Status.Failure; 

        }
    }
}