using TonyMax.Extensions.DOTS.Linking.Components;
using Unity.Entities;
using Unity.Collections;

namespace TonyMax.Extensions.DOTS.Linking.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public class UnlinkOnDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //Unlink all from entity that being destroyed
            Entities
                .WithNone<LinkedEntityTag>()
                .ForEach((Entity unlinkingEntity, in DynamicBuffer<LinkDataElement> linkDataBuffer) =>
                {
                    var linkDataArray = linkDataBuffer.ToNativeArray(Allocator.Temp);
                    for(int i = 0; i < linkDataArray.Length; i++)
                    {
                        var linkData = linkDataArray[i];

                        if(EntityManager.Exists(linkData.linkOwner))
                            EntityManager.Unlink(linkData);
                    }

                    EntityManager.RemoveComponent<LinkDataElement>(unlinkingEntity);
                })
                .WithStructuralChanges()
                .WithoutBurst()
                .Run();
        }
    }
}
