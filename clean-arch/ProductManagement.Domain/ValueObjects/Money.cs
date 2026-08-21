public class Money
{
    public decimal Amount { get; private init; }
    public string Currency { get; private init; } = null!;

    public Money(decimal amount, string currency)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required");

        Amount = amount;
        Currency = currency;
    }

    private Money() { }   // 🆕 EF Core uses this via reflection — never called by your own code

    public override bool Equals(object? obj) =>
        obj is Money other && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
}