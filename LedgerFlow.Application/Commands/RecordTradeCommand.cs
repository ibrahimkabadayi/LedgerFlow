using LedgerFlow.Domain.ValueObjects;
using MediatR;

namespace LedgerFlow.Application.Commands;

public record RecordTradeCommand(
    Guid WalletId,
    Money Bought,
    Money Sold
    ) : IRequest;


