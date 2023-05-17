using System.Text;
using Bonsai.Core;
using UnityEngine;

namespace Bonsai.Standard
{
    [BonsaiNode("Tasks/", "Timer")]
    public class TestTask : Task
    {
        public override void OnEnter()
        {
            Debug.Log("TestBehaviourEnter");
        }
        public override Status Run()
        {
            Debug.Log("TestBehaviourRunMethod");
            return Status.Success;
        }
    }
}
