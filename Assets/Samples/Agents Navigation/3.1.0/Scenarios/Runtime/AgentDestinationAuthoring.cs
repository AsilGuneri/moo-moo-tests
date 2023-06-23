using UnityEngine;
using ProjectDawn.Navigation.Hybrid;

namespace ProjectDawn.Navigation.Sample.Scenarios
{
    [RequireComponent(typeof(AgentAuthoring))]
    [DisallowMultipleComponent]
    public class AgentDestinationAuthoring : MonoBehaviour
    {
        private void Start()
        {
            //SetDestination();
        }

        public void SetDestination(Vector3 position)
        {
            var agent = transform.GetComponent<AgentAuthoring>();
            var body = agent.EntityBody;
            body.SetDestination(position);
            agent.EntityBody = body;
        }
        public void Stop()
        {
            var agent = transform.GetComponent<AgentAuthoring>();
            var body = agent.EntityBody;
            body.Stop();
            agent.EntityBody = body;
        }
    }
}
