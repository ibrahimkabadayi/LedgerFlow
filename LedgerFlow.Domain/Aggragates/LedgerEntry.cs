using LedgerFlow.Domain.Events;
using LedgerFlow.Domain.ValueObjects;

namespace LedgerFlow.Domain.Aggragates;

public class LedgerEntry
{
    public Guid Id;
    private readonly Dictionary<string, Money> _balances = new();
    public IReadOnlyDictionary<string, Money> Balances => _balances;
    private List<IDomainEvent> _uncommitedEvents = new();
    public int Version;

    public LedgerEntry(){ }

    public void Create(Guid Id)
    {
        var entry = new LedgerEntry();
        var @event = new LedgerEntryCreated(Id, DateTime.UtcNow);
        entry.Apply(@event);
        Version++;
        _uncommitedEvents.Add(@event);
    }

    public void Apply(AssetDepositedRecorded @event)
    {
        var current = _balances[@event.Money.Currency];
        _balances[@event.Money.Currency] = current.Add(@event.Money);
        Version++;
    }

    public void Apply(AssetWithdrawnRecorded @event)
    {
        var current = _balances[@event.Money.Currency];
        _balances[@event.Money.Currency] = current.Subtract(@event.Money);
        Version++;
    }

    public void Apply(LedgerEntryCreated @event)
    {
        Id = @event.LedgerEntryId;
        Version++;
    }

    public void Apply(TradeRecorded @event)
    {
        Id = @event.LedgerEntryId;
        Version++;
    }

    public void ApplyDeposit(Money money)
    {
        var @event = new AssetDepositedRecorded(Id, money, DateTime.UtcNow);
        Apply(@event);
        _uncommitedEvents.Add(@event);
    }

    public void ApplyWithdraw(Money money)
    {
        var @event = new AssetWithdrawnRecorded(Id, money, DateTime.UtcNow);
        Apply(@event);
        _uncommitedEvents.Add(@event);
    }
    
    public void ApplyTrade(Money bought, Money sold)
    {
        var @event = new TradeRecorded(Id, bought, sold, DateTime.UtcNow);
        Apply(@event);
        _uncommitedEvents.Add(@event);
    }
    
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _uncommitedEvents.AsReadOnly();

    public void ClearUncommitedEvents() => _uncommitedEvents.Clear();

    public static LedgerEntry Replay(IEnumerable<IDomainEvent> history)
    {
        var entry = new LedgerEntry(); 
        foreach (var @event in history)
        {
            switch (@event)
            {
                case LedgerEntryCreated e: entry.Apply(e); break;
                case AssetDepositedRecorded e: entry.Apply(e); break;
                case AssetWithdrawnRecorded e: entry.Apply(e); break;
                case TradeRecorded e: entry.Apply(e); break;
            }
        }
        return entry;
    } 
}
