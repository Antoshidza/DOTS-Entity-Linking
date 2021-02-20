using TonyMax.Extensions.DOTS.Linking.Components;
using Unity.Collections;
using Unity.Entities;

namespace TonyMax.Extensions.DOTS.Linking
{
    public static class LinkMethodsExtensions
    {
        #region Link
        #region EntityManager
        #region Link
        //Methods in this region just add already existed link to LinkDataElement buffer

        /// <summary>
        /// Registrate already exists link.
        /// When link is registrated there is a possible to unlink it.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkDataBuffer">Link buffer wich will store new link data</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void Link<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                DynamicBuffer<LinkDataElement> linkDataBuffer,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
#if UNITY_EDITOR
            if(!entityManager.Exists(linkOwner))
                throw new System.Exception($"You trying register link on {linkOwner} (owner) but entity doesn't exists.");
#endif
            linkDataBuffer.Add(new LinkDataElement { linkOwner = linkOwner, linkType = ComponentType.ReadOnly<TLink>(), isRemovable = isRemovable });
        }
        /// <summary>
        /// Registrate already exists link.
        /// When link is registrated there is a possible to unlink it.
        /// Also method will search LinkDataElement buffer on linked entity and if there isn't on it then will add buffer.
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void Link<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
            var linkDataBuffer = entityManager.HasComponent<LinkDataElement>(linkedEntity) ?
                entityManager.GetBuffer<LinkDataElement>(linkedEntity) :
                entityManager.PrepareEntityForLinking(linkedEntity);

            entityManager.Link<TLink>(linkOwner, linkDataBuffer, isRemovable);
        }
        #endregion

        #region Add Link
        //Methods in this region will add link component to link's owner entity and then like methods in "Link" region
        //will add link data to LinkDataElement buffer

        /// <summary>
        /// Add given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkDataBuffer">Link buffer wich will store new link data</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void AddLink<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                TLink link,
                                DynamicBuffer<LinkDataElement> linkDataBuffer,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
#if UNITY_EDITOR
            if(!entityManager.Exists(linkOwner))
                throw new System.Exception($"You trying add link on {linkOwner} (owner) but entity doesn't exists.");
#endif
            entityManager.AddComponentData(linkOwner, link);
            entityManager.Link<TLink>(linkOwner, linkDataBuffer, isRemovable);
        }

        /// <summary>
        /// Add given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// Also method will search LinkDataElement buffer on linked entity and if there isn't on it then will add buffer.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void AddLink<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                TLink link,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
#if UNITY_EDITOR

            if(!entityManager.Exists(linkOwner))
                throw new System.Exception($"You trying register link on {linkOwner} (owner) but entity doesn't exists.");
#endif
            entityManager.AddComponentData(linkOwner, link);
            entityManager.Link<TLink>(linkOwner, linkedEntity, isRemovable);
        }
        #endregion

        #region Set Link
        //Methods in this region will set link component to link's owner entity and then like methods in "Link" region
        //will add link data to LinkDataElement buffer

        /// <summary>
        /// Sets given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="linkDataBuffer">Link buffer wich will store new link data</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void SetLink<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                TLink link,
                                DynamicBuffer<LinkDataElement> linkDataBuffer,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
#if UNITY_EDITOR
            if(!entityManager.Exists(linkOwner))
                throw new System.Exception($"You trying register link on {linkOwner} (owner) but entity doesn't exists.");
#endif
            entityManager.SetComponentData(linkOwner, link);
            entityManager.Link<TLink>(linkOwner, linkDataBuffer, isRemovable);
        }

        /// <summary>
        /// Sets given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// Also method will search LinkDataElement buffer on linked entity and if there isn't on it then will add buffer.
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void SetLink<TLink>(this EntityManager entityManager,
                                Entity linkOwner,
                                TLink link,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
#if UNITY_EDITOR
            if(!entityManager.Exists(linkOwner))
                throw new System.Exception($"You trying register link on {linkOwner} (owner) but entity doesn't exists.");
#endif
            entityManager.SetComponentData(linkOwner, link);
            entityManager.Link<TLink>(linkOwner, linkedEntity, isRemovable);
        }
        #endregion
        #endregion

        #region EntityCommandBuffer
        #region Link
        //Methods in this region just add already existed link to LinkDataElement buffer

        /// <summary>
        /// Registrate already added link.
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
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
            ecb.AppendToBuffer(linkedEntity, new LinkDataElement { linkOwner = linkOwner, linkType = ComponentType.ReadOnly<TLink>(), isRemovable = isRemovable });
        }
        #endregion

        #region Add Link
        //Methods in this region will add link component to link's owner entity and then like methods in "Link" region
        //will add link data to LinkDataElement buffer

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
        public static void AddLink<TLink>(this ref EntityCommandBuffer ecb,
                                Entity linkOwner,
                                TLink link,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
            ecb.AddComponent(linkOwner, link);
            ecb.Link<TLink>(linkOwner, linkedEntity, isRemovable);
        }
        #endregion

        #region Set Link
        //Methods in this region will set link component to link's owner entity and then like methods in "Link" region
        //will add link data to LinkDataElement buffer

        /// <summary>
        /// Set given link to linkOwner entity and then registrate that link.
        /// When link is registrated there is a possible to unlink it.
        /// <para>Conditions:</para> 
        /// <list type="bullet">
        ///    <item>Linked entity has LinkDataElement buffer</item>
        /// </list>
        /// </summary>
        /// <param name="linkOwner">Entity which has link component</param>
        /// <param name="isRemovable">Defines link component will be removed or just reseted on unlink</param>
        public static void SetLink<TLink>(this ref EntityCommandBuffer ecb,
                                Entity linkOwner,
                                TLink link,
                                Entity linkedEntity,
                                bool isRemovable = false)
            where TLink : struct, IComponentData
        {
            ecb.SetComponent(linkOwner, link);
            ecb.Link<TLink>(linkOwner, linkedEntity, isRemovable);
        }
        #endregion
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

        #region PrepareLinkedEntity
        public static DynamicBuffer<LinkDataElement> PrepareEntityForLinking(this EntityManager entityManager, Entity entity, int bufferCapacity = 1)
        {
            entityManager.AddComponentData(entity, new LinkedEntityTag());
            var linkDataBuffer = entityManager.AddBuffer<LinkDataElement>(entity);
            linkDataBuffer.EnsureCapacity(bufferCapacity);

            return linkDataBuffer;
        }
        public static void PrepareEntityForLinking(this ref EntityCommandBuffer ecb, Entity entity, int bufferCapacity = 1)
        {
            var linkDataBuffer = ecb.AddBuffer<LinkDataElement>(entity);
            linkDataBuffer.EnsureCapacity(bufferCapacity);
            ecb.AddComponent<LinkedEntityTag>(entity);
        }
        #endregion
    }
}
