namespace IziHardGames.AsyncCommunication.Application.Models
{
    using System;

    public readonly struct ConsumerMetaT1
    {
        public readonly ConsumerMeta meta;
        public readonly Type registeredAs;

        public ConsumerMetaT1(ConsumerMeta meta, Type registeredAs)
        {
            this.registeredAs = registeredAs;
            this.meta = meta;
        }
    }
}