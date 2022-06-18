#if NETFRAMEWORK
using BinaryPack.Models.Helpers;
using BinaryPack.Models.Interfaces;
using System;
using System.Collections.Generic;

#nullable enable

namespace BinaryPack.Models
{
    /// <summary>
    /// A model that represents some packets that are sent in a multiplayer game
    /// </summary>
    [Serializable]
    public sealed class AbstractPacketModel : IInitializable, IEquatable<AbstractPacketModel>
    {
        public List<Packet>? Packets { get; set; }

        ///<inheritdoc/>
        public void Initialize()
        {
            Packets = new List<Packet>();
            
            for (int i = 0; i < 3; i++)
            {
                MovementPacket movement = new MovementPacket();
                movement.Initialize();
                Packets.Add(movement);

                ChatMessagePacket chatMessage = new ChatMessagePacket();
                chatMessage.Initialize();
                Packets.Add(chatMessage);
            }
        }

        ///<inheritdoc/>
        public bool Equals(AbstractPacketModel? other)
        {
            if (other == null) return false;
            if (Packets == null && other.Packets == null) return true;
            if (Packets == null || other.Packets == null) return false;
            if (Packets.Count != other.Packets.Count) return false;
            
            for (int i = 0; i < Packets.Count; i++)
            {
                if (!Packets[i].Equals(other.Packets[i])) return false;
            }

            return true;
        }
    }

    /// <summary>
    /// The <see langword="abstract"/> packet type
    /// </summary>
    [Serializable]
    public abstract class Packet : IInitializable, IEquatable<Packet>
    {
        static Packet()
        {
            BinaryConverter.RegisterUnion<Packet>(new Type[] { typeof(MovementPacket), typeof(ChatMessagePacket) });
        }

        public Guid PlayerId { get; set; }

        public DateTime Timestamp { get; set; }

        ///<inheritdoc/>
        public virtual void Initialize()
        {
            PlayerId = Guid.NewGuid();
            Timestamp = RandomProvider.NextDateTime();
        }

        ///<inheritdoc/>
        public virtual bool Equals(Packet? other)
        {
            if (other == null) return false;
            return PlayerId == other.PlayerId && Timestamp == other.Timestamp;
        }
    }

    /// <summary>
    /// A packet that represents player movement
    /// </summary>
    [Serializable]
    public class MovementPacket : Packet, IInitializable, IEquatable<Packet>
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double Pitch { get; set; }

        public double Yaw { get; set; }

        public bool OnGround { get; set; }

        ///<inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();
            X = RandomProvider.NextDouble();
            Y = RandomProvider.NextDouble();
            Z = RandomProvider.NextDouble();
            Pitch = RandomProvider.NextDouble();
            Yaw = RandomProvider.NextDouble();
            OnGround = RandomProvider.NextBool();
        }

        ///<inheritdoc/>
        public override bool Equals(Packet? other)
        {
            if (!base.Equals(other)) return false;
            if (!(other is MovementPacket p)) return false;
            return X == p.X && 
                   Y == p.Y &&
                   Z == p.Z &&
                   Pitch == p.Pitch &&
                   Yaw == p.Yaw &&
                   OnGround == p.OnGround;
        }
    }

    /// <summary>
    /// A packet that represents a chat message sent by a player
    /// </summary>
    [Serializable]
    public class ChatMessagePacket : Packet, IInitializable, IEquatable<Packet>
    {
        public string? Message { get; set; }

        public bool IsCommand { get; set; }

        ///<inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();
            Message = RandomProvider.NextString(20);
            IsCommand = RandomProvider.NextBool();
        }

        ///<inheritdoc/>
        public override bool Equals(Packet? other)
        {
            if (!base.Equals(other)) return false;
            if (!(other is ChatMessagePacket p)) return false;
            return Message == p.Message && IsCommand == p.IsCommand;
        }
    }
}
#endif
