using LedgerFlow.Application.Commands;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class AssetWithdrawnProjectionHandler : IRequestHandler<RecordWithdrawnCommand>
{
    public Task Handle(RecordWithdrawnCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
