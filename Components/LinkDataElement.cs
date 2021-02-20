using System;
using Unity.Entities;

namespace TonyMax.Entities.Linking
{
    public struct LinkDataElement : ISystemStateBufferElementData, IEquatable<Entity>
    {
        public ComponentType linkType;
        public Entity linkOwner;
        public bool isRemovable;

        public bool Equals(Entity other)
        {
            return linkOwner == other;
        }
    }
}
