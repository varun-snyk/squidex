﻿// ==========================================================================
//  HistoryEventCreatorBase.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using Squidex.Infrastructure;
using Squidex.Infrastructure.CQRS;
using Squidex.Infrastructure.CQRS.Events;

namespace Squidex.Read.History
{
    public abstract class HistoryEventsCreatorBase : IHistoryEventsCreator
    {
        private readonly Dictionary<string, string> texts = new Dictionary<string, string>();
        private readonly TypeNameRegistry typeNameRegistry;

        public IReadOnlyDictionary<string, string> Texts
        {
            get { return texts; }
        }

        protected HistoryEventsCreatorBase(TypeNameRegistry typeNameRegistry)
        {
            Guard.NotNull(typeNameRegistry, nameof(typeNameRegistry));

            this.typeNameRegistry = typeNameRegistry;
        }

        public void AddEventMessage<TEvent>(string message) where TEvent : IEvent
        {
            Guard.NotNullOrEmpty(message, nameof(message));

            texts[typeNameRegistry.GetName<TEvent>()] = message;
        }

        protected HistoryEventToStore ForEvent(IEvent @event, string channel)
        {
            var message = typeNameRegistry.GetName(@event.GetType());

            return new HistoryEventToStore(channel, message);
        }

        public abstract Task<HistoryEventToStore> CreateEventAsync(Envelope<IEvent> @event);
    }
}
