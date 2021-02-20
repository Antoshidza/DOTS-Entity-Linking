using TonyMax.Extensions.DOTS.Linking.Components;
using Unity.Entities;
using Unity.Collections;

namespace TonyMax.Extensions.DOTS.Linking.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    internal class UnlinkOnDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //Unlink all from entity that being destroyed
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            Entities
                .WithNone<LinkedEntityTag>()
                .ForEach((Entity unlinkingEntity, in DynamicBuffer<LinkDataElement> linkDataBuffer) =>
                {
                    for(int i = 0; i < linkDataBuffer.Length; i++)
                    {
                        var linkData = linkDataBuffer[i];

                        if(EntityManager.Exists(linkData.linkOwner))
                            ecb.Unlink(linkData);
                    }

                    EntityManager.RemoveComponent<LinkDataElement>(unlinkingEntity);
                })
                .WithStructuralChanges()
                .WithoutBurst()
                .Run();

            ecb.Playback(EntityManager);
        }
    }
}
