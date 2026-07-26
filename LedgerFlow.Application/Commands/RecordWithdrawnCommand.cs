using LedgerFlow.Domain.ValueObjects;
using MediatR;

namespace LedgerFlow.Application.Commands;

public record RecordWithdrawnCommand(
    Guid WalletId,
    Money Money
    ) : IRequest;
