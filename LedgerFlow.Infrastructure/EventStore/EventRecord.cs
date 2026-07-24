using System;
using System.Collections.Generic;
using System.Text;

namespace LedgerFlow.Infrastructure.EventStore;

public record EventRecord(string EventType, string Payload);


