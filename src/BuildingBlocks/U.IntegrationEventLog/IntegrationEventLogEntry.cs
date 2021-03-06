﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using U.EventBus.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace U.IntegrationEventLog
{
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry() { }
        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonSerializer.Serialize(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public void DeserializeJsonContent(Type type)
        {
            var deserialized = JsonSerializer.Deserialize(Content, type);

            IntegrationEvent = deserialized as IntegrationEvent;
        }
    }
}
