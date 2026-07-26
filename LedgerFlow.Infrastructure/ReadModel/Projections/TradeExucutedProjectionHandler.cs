using LedgerFlow.Application.Commands;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class TradeExucutedProjectionHandler : IRequestHandler<RecordTradeCommand>
{
    public Task Handle(RecordTradeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
