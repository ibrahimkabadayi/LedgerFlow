namespace LedgerFlow.Application.Common.Models;

public class AccountBalanceView
{
    public Guid WalletId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
