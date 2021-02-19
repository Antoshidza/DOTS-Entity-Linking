using TonyMax.Extensions.DOTS.Linking.Components;
using Unity.Entities;

namespace TonyMax.Extensions.DOTS.Linking.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(UnlinkOnDestroySystem))]
    public class RegistrateLinkedEntitiesSystem : SystemBase
    {
        private EntityCommandBufferSystem _ecbSystem;
        private EntityQuery _linkedTagNeedEntities;

        protected override void OnCreate()
        {
            _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            _linkedTagNeedEntities = GetEntityQuery
            (
                ComponentType.ReadOnly<LinkDataElement>(),
                ComponentType.Exclude<LinkedEntityTag>()
            );
        }
        protected override void OnUpdate()
        {
            //If entity got LinkDataElement it means that entity was linked so we need add LinkedEntityTag to be able to react to destroying
            _ecbSystem.CreateCommandBuffer().AddComponent<LinkedEntityTag>(_linkedTagNeedEntities);
        }
    }
}
