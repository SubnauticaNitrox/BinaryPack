#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using BinaryPack.Attributes;

#nullable enable

namespace BinaryPack.Models
{
    /// <summary>
    /// A model that represents some packets that are sent in a multiplayer game using the <see cref="ForceUnionAttribute"/>
    /// </summary>
    [Serializable]
    public sealed class AbstractListItemModel : IEquatable<AbstractListItemModel>
    {
        public EntPacket? Packet { get; set; }

        static AbstractListItemModel()
        {
            BinaryConverter.RegisterUnion<EntPacket>(typeof(WorkingCellEntities), typeof(FailingCellEntities));
            BinaryConverter.RegisterUnion<Entity>(typeof(GroupWorldEntity), typeof(GroupUnUnionWorldEntity), typeof(WorldEntity), typeof(UnUnionWorldEntity));
        }

        public void Initialize()
        {
            GroupWorldEntity e = new("Placeholder Entity");
            List<WorldEntity> worldEntities = new() { e };
            Packet = new WorkingCellEntities(worldEntities);
        }

        public void InitializeFail()
        {
            GroupUnUnionWorldEntity e = new("Placeholder Entity");
            List<UnUnionWorldEntity> worldEntities = new() { e };
            Packet = new FailingCellEntities(worldEntities);
        }

        ///<inheritdoc/>
        public bool Equals(AbstractListItemModel? other)
        {
            if (other == null) return false;
            return Packet.Equals(other.Packet);
        }

        public abstract record Entity(string Name);

        [ForceUnion]
        public record WorldEntity(string Name) : Entity(Name);
        public record UnUnionWorldEntity(string Name) : Entity(Name);

        [ForceUnion]
        public record GroupWorldEntity(string Name) : WorldEntity(Name);
        public record GroupUnUnionWorldEntity(string Name) : UnUnionWorldEntity(Name);

        public abstract record EntPacket;

        public record WorkingCellEntities(List<WorldEntity> Entities) : EntPacket
        {
            public virtual bool Equals(WorkingCellEntities? other)
            {
                if (other == null) return false;
                return GetType() == other.GetType() && Entities.SequenceEqual(other.Entities);
            }
        }

        public record FailingCellEntities(List<UnUnionWorldEntity> Entities) : EntPacket
        {
            public virtual bool Equals(FailingCellEntities? other)
            {
                if (other == null) return false;
                return GetType() == other.GetType() && Entities.SequenceEqual(other.Entities);
            }
        }
    }
}
#endif
