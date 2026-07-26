using LedgerFlow.Domain.ValueObjects;
using MediatR;

namespace LedgerFlow.Application.Commands;

public record RecordDepositCommand(
    Guid WalletId,
    Money Money
    ) : IRequest;
