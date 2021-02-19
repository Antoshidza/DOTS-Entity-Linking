using TonyMax.Extensions.DOTS.Linking.Components;
using Unity.Collections;
using Unity.Entities;

namespace TonyMax.Extensions.DOTS.Linking
{
    public static class LinkMethodsExtensions
    {
        #region Link
        #region EntityManager
        /// <summary>
        /// Registrate already added link.
        /// When link is registrated there is a possible to unlink it.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkType">Type of link component</param>
        /// <param name="linkDataBuffer">Link buffer wich will store new link data</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void RegisterLink(this EntityManager entityManager,
                                Entity linkOwner,
                                ComponentType linkType,
                                DynamicBuffer<LinkDataElement> linkDataBuffer,
                                bool isRemovable = false)
        {
            linkDataBuffer.Add(new LinkDataElement { linkOwner = linkOwner, linkType = linkType, isRemovable = isRemovable });
        }

        /// <summary>
        /// Add given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkDataBuffer">Link buffer wich will store new link data</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void Link<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                TLink link,
                                DynamicBuffer<LinkDataElement> linkDataBuffer,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
            entityManager.AddComponentData(linkOwner, link);
            entityManager.RegisterLink(linkOwner, ComponentType.ReadOnly<TLink>(), linkDataBuffer, isRemovable);
        }

        /// <summary>
        /// Registrate already added link.
        /// When link is registrated there is a possible to unlink it.
        /// Also method will search LinkDataElement buffer on linked entity and if there isn't on it then will add buffer.
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkType">Type of link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void RegisterLink(this EntityManager entityManager,
                                Entity linkOwner,
                                ComponentType linkType,
                                Entity linkedEntity,
                                bool isRemovable = false)
        {
            var linkedEntityGroup = entityManager.HasComponent<LinkDataElement>(linkedEntity) ?
                entityManager.GetBuffer<LinkDataElement>(linkedEntity) :
                entityManager.AddBuffer<LinkDataElement>(linkedEntity);

            entityManager.RegisterLink(linkOwner, linkType, linkedEntityGroup, isRemovable);
        }

        /// <summary>
        /// Add given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// Also method will search LinkDataElement buffer on linked entity and if there isn't on it then will add buffer.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void Link<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                TLink link,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
            entityManager.AddComponentData(linkOwner, link);
            entityManager.RegisterLink(linkOwner, ComponentType.ReadOnly<TLink>(), linkedEntity, isRemovable);
        }
        #endregion

        #region EntityCommandBuffer
        /// <summary>
        /// Registrate already added link.
        /// When link is registrated there is a possible to unlink it.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Linked entity has LinkDataElement buffer</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkType">Type of link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void RegisterLink(this ref EntityCommandBuffer ecb,
                                Entity linkOwner,
                                ComponentType linkType,
                                Entity linkedEntity,
                                bool isRemovable = false)
        {
#if UNITY_EDITOR
            
            if(!World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<LinkDataElement>(linkedEntity))
                throw new System.Exception($"You trying link {linkedEntity} to {linkOwner} (owner) but {linkedEntity} has no LinkDataElement buffer, which used to contain link data.");
#endif
            ecb.AppendToBuffer(linkedEntity, new LinkDataElement { linkOwner = linkOwner, linkType = linkType, isRemovable = isRemovable });
        }

        /// <summary>
        /// Add given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Linked entity has LinkDataElement buffer</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void Link<TLink>(this ref EntityCommandBuffer ecb,
                                Entity linkOwner,
                                TLink link,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
#if UNITY_EDITOR

            if(!World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<LinkDataElement>(linkedEntity))
                throw new System.Exception($"You trying link {linkedEntity} to {linkOwner} (owner) but {linkedEntity} has no LinkDataElement buffer, which used to contain link data.");
#endif
            ecb.AddComponent(linkOwner, link);
            ecb.RegisterLink(linkOwner, ComponentType.ReadOnly<TLink>(), linkedEntity, isRemovable);
        }
        #endregion
        #endregion

        #region UnLink
        #region EntityManager
        /// <summary>
        /// Unlink entities using EntityManager.
        /// After unlinking owner's link component will be clear or removed depending on link kind.
        /// Removes link data from LinkDataElement buffer.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Link owner exists</item>
        ///    <item>Entities was linked</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        public static void Unlink(this EntityManager entityManager, DynamicBuffer<LinkDataElement> linkDataBuffer, Entity linkOwner, Entity linkedEntity)
        {
            var linkData = GetLinkData(linkDataBuffer, linkOwner, linkedEntity, out var linkDataIndex);

            linkDataBuffer.RemoveAt(linkDataIndex);
            entityManager.Unlink(linkData);
        }
        /// <summary>
        /// Unlink entities using EntityManager.
        /// After unlinking owner's link component will be clear or removed depending on link kind.
        /// Removes link data from LinkDataElement buffer.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Link owner exists</item>
        ///    <item>Entities was linked</item>
        ///    <item>Linked entity has LinkDataElement buffer</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        public static void Unlink(this EntityManager entityManager, Entity linkOwner, Entity linkedEntity)
        {
            var linkDataBuffer = GetLinkDataBuffer(entityManager, linkedEntity);
            entityManager.Unlink(linkDataBuffer, linkOwner, linkedEntity);
        }
        /// <summary>
        /// Unlink entities using EntityManager by given link data (stored in LinkDataElement buffer).
        /// After unlinking owner's link component will be clear or removed depending on link kind.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Link owner exists and has link component</item>
        /// </list>
        /// </summary
        public static void Unlink(this EntityManager entityManager, LinkDataElement linkData)
        {
            entityManager.RemoveComponent(linkData.linkOwner, linkData.linkType);
            if(!linkData.isRemovable)
                entityManager.AddComponent(linkData.linkOwner, linkData.linkType);
        }

        private static DynamicBuffer<LinkDataElement> GetLinkDataBuffer(EntityManager entityManager, Entity linkedEntity)
        {
#if UNITY_EDITOR
            if(!entityManager.HasComponent<LinkDataElement>(linkedEntity))
                throw new System.Exception($"{linkedEntity} has no LinkDataElement buffer, but you try to unlink this entity.");
#endif
            return entityManager.GetBuffer<LinkDataElement>(linkedEntity);
        }
        #endregion

        #region EntityCommandBuffer
        /// <summary>
        /// Unlink entities using EntityCommnadBuffer.
        /// After unlinking owner's link component will be clear or removed depending on link kind.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Link owner exists</item>
        ///    <item>Entities was linked</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        public static void Unlink(this ref EntityCommandBuffer ecb, DynamicBuffer<LinkDataElement> linkDataBuffer, Entity linkOwner, Entity linkedEntity)
        {
            var linkData = GetLinkData(linkDataBuffer, linkOwner, linkedEntity, out var linkDataIndex);

            linkDataBuffer.RemoveAt(linkDataIndex);
            ecb.Unlink(linkData);
        }
        /// <summary>
        /// Unlink entities using EntityCommnadBuffer.
        /// After unlinking owner's link component will be clear or removed depending on link kind.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Link owner exists</item>
        ///    <item>Entities was linked</item>
        ///    <item>Linked entity has LinkDataElement buffer</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        public static void Unlink(this ref EntityCommandBuffer ecb, SystemBase system, Entity linkOwner, Entity linkedEntity)
        {
            var linkDataBuffer = GetLinkDataBuffer(system, linkedEntity);
            ecb.Unlink(linkDataBuffer, linkOwner, linkedEntity);
        }
        /// <summary>
        /// Unlink entities using EntityCommandBuffer by given link data (stored in LinkDataElement buffer).
        /// After unlinking owner's link component will be clear or removed depending on link kind.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Link owner exists and has link component</item>
        /// </list>
        /// </summary>
        public static void Unlink(this ref EntityCommandBuffer ecb, LinkDataElement linkData)
        {
            ecb.RemoveComponent(linkData.linkOwner, linkData.linkType);
            if(!linkData.isRemovable)
                ecb.AddComponent(linkData.linkOwner, linkData.linkType);
        }

        private static DynamicBuffer<LinkDataElement> GetLinkDataBuffer(SystemBase system, Entity linkedEntity)
        {
#if UNITY_EDITOR
            if(!system.EntityManager.HasComponent<LinkDataElement>(linkedEntity))
                throw new System.Exception($"{linkedEntity} has no LinkDataElement buffer, but you try to unlink this entity.");
#endif
            return system.GetBuffer<LinkDataElement>(linkedEntity);
        }
        #endregion

        #region Common
        private static LinkDataElement GetLinkData(DynamicBuffer<LinkDataElement> linkDataBuffer, Entity linkOwner, Entity linkedEntity, out int index)
        {
            var linkDataArray = linkDataBuffer.AsNativeArray();

            index = linkDataArray.IndexOf(linkOwner);
#if UNITY_EDITOR
            if(index == -1)
                throw new System.Exception($"You trying unlink {linkedEntity} from {linkOwner} but {linkedEntity} has no link data for {linkOwner}.");
#endif
            return linkDataArray[index];
        }
        #endregion
        #endregion
    }
}
