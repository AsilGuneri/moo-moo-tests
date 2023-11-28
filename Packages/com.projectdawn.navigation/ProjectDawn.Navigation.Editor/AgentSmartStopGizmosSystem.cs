using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using static Unity.Entities.SystemAPI;
using static Unity.Mathematics.math;
using ProjectDawn.LocalAvoidance;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst.Intrinsics;

namespace ProjectDawn.Navigation.Editor
{
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(AgentSystemGroup))]
    [UpdateAfter(typeof(AgentTransformSystemGroup))]
    [UpdateAfter(typeof(AgentSmartStopSystem))]
    public partial struct AgentSmartStopGizmosSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var gizmos = GetSingletonRW<GizmosSystem.Singleton>();

            new SmartStopGizmosJob
            {
                Gizmos = gizmos.ValueRW.CreateCommandBuffer(),
            }.Schedule();
        }

        [BurstCompile]
        unsafe partial struct SmartStopGizmosJob : IJobEntity
        {
            public GizmosCommandBuffer Gizmos;

            public void Execute(in DrawGizmos drawGizmos, in AgentBody body, in GiveUpStopTimer giveUp, in AgentSmartStop smartStop, in LocalTransform transform)
            {
                if (body.IsStopped)
                    return;

                if (!smartStop.GiveUpStop.Enabled || giveUp.Progress == 0.0f)
                    return;

                Gizmos.DrawNumber(transform.Position, 1.0f - giveUp.Progress, Color.red);
            }
        }
    }
}
