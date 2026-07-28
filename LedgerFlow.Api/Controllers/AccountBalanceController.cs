using Microsoft.AspNetCore.Mvc;
using MediatR;
using LedgerFlow.Application.Queries.GetAccountBalance;
using LedgerFlow.Application.Queries.GetAccountBalances;

namespace LedgerFlow.Api.Controllers;

[Route("api/account-balances")]
[ApiController]
public class AccountBalanceController(IMediator mediator) : ControllerBase
{
    [HttpGet("{walletId}")]
    public async Task<IActionResult> GetAccountBalance(Guid walletId)
    {
        var request = new GetAccountBalancesQuery(walletId);
        var balances = await mediator.Send(request);
        return Ok(balances);
    }

    [HttpGet("{walletId}/{currency}")]
    public async Task<IActionResult> GetAccountBalanceForOneCurrency(Guid walletId, string currency)
    {
        var request = new GetAccountBalanceQuery(walletId, currency);
        var balance = await mediator.Send(request);
        return Ok(balance);
    }
} 
