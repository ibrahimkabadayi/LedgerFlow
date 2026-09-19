using LedgerFlow.Domain.Events;
using LedgerFlow.Domain.ValueObjects;

namespace LedgerFlow.Domain.Aggragates;

public class LedgerEntry
{
    public Guid Id { get; private set; }
    private readonly Dictionary<string, Money> _balances = [];
    public IReadOnlyDictionary<string, Money> Balances => _balances;
    private readonly List<IDomainEvent> _uncommitedEvents = [];
    public int Version { get; private set; }

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
        if (_balances.TryGetValue(@event.Money.Currency, out var current))
        {
            _balances[@event.Money.Currency] = current.Add(@event.Money);
        }
        else
        {
            _balances[@event.Money.Currency] = @event.Money;
        }
        Version++;
    }

    public void Apply(AssetWithdrawnRecorded @event)
    {
        if (_balances.TryGetValue(@event.Money.Currency, out var current))
        {
            _balances[@event.Money.Currency] = current.Subtract(@event.Money);
        }
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

        if (_balances.TryGetValue(@event.Sold.Currency, out var currentSold))
        {
            _balances[@event.Sold.Currency] = currentSold.Subtract(@event.Sold);
        }

        if (_balances.TryGetValue(@event.Bought.Currency, out var currentBought))
        {
            _balances[@event.Bought.Currency] = currentBought.Add(@event.Bought);
        }
        else
        {
            _balances[@event.Bought.Currency] = @event.Bought;
        }

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

    public static LedgerEntry Replay(IEnumerable<IDomainEvent> history) => Replay(Guid.Empty, history);

    public static LedgerEntry Replay(Guid walletId, IEnumerable<IDomainEvent> history)
    {
        var entry = new LedgerEntry { Id = walletId }; 
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
