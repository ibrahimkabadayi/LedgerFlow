using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LedgerFlow.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency {  get; private set; }

    public Money(decimal amount, string currency)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount can not be smaller or equel to 0.");

        if (string.IsNullOrEmpty(currency))
            throw new ArgumentNullException(nameof(currency), "Currency can not be empty.");

        Amount = amount;
        Currency = currency.ToUpper();
    }

    public Money Add(Money amount)
    {
        if (amount.Amount <= 0)
        {
            throw new ArgumentException("To do addition added amount must be bigger than 0", nameof(amount));
        }

        if (!Currency.Equals(amount.Currency))
        {
            throw new ArgumentException("To do addition currencies must be same.", nameof(amount));
        }

        Amount += amount.Amount;
        return this;
    }

    public Money Subtract(Money amount) 
    {
        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Withdraw amount can not be smaller or equal to 0.", nameof(amount));
        }

        if (Amount < amount.Amount)
        {
            throw new ArgumentException("To do withdraw operation current money must be bigger than withdraw amount.", nameof(amount));
        }

        if (!Currency.Equals(amount.Currency))
        {
            throw new ArgumentException("To do addition currencies must be same.", nameof(amount));
        }

        Amount -= amount.Amount;
        return this;
    }
}
