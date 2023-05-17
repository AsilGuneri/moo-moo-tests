using System.Text;
using Bonsai.Core;
using UnityEngine;

namespace Bonsai.Standard
{
    [BonsaiNode("Tasks/", "Timer")]
    public class AttackClosestUnit : Task
    {

        public override void OnEnter()
        {

        }
        public override Status Run()
        {

            return Status.Success;
        }
    }
}
