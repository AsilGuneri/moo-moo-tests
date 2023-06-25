﻿using Unity.Entities;
using Unity.Mathematics;

namespace ProjectDawn.Navigation
{
    /// <summary>
    /// Agent's body movement.
    /// </summary>
    public struct AgentBody : IComponentData
    {
        /// <summary>
        /// Force that agent will use to move towards. Calculated from agent position and destination.
        /// </summary>
        public float3 Force;
        /// <summary>
        /// The velocity agent is moving.
        /// </summary>
        public float3 Velocity;
        /// <summary>
        /// Destination that agent will attempt to reach.
        /// </summary>
        public float3 Destination;
        /// <summary>
        /// The distance between the agent's position and the destination.
        /// </summary>
        public float RemainingDistance;
        /// <summary>
        /// This property holds the stop or resume condition of the agent.
        /// </summary>
        public bool IsStopped;

        /// <summary>
        /// Sets properties for new agent destination.
        /// </summary>
        public void SetDestination(float3 value)
        {
            Destination = value;
            IsStopped = false;
        }
        //BU İKİ AQ EVLADI İLE BÜTÜN İŞİMİZ.
        /// <summary>
        /// Sets properties for agent to stop.
        /// </summary>
        public void Stop()
        {
            IsStopped = true;
            Velocity = 0;
        }
    }
}
