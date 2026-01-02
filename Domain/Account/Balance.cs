using System.Numerics;

public class Balance
{
    public decimal Amount { get => _amount; private set => _amount = value; }

    private decimal _amount;

    public Balance(decimal amount)
    {
        Amount = amount;
    }

    public decimal Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor de crédito deve ser positivo.", nameof(amount));

        _amount += amount;
        return Amount;
    }


    public decimal Debit(decimal amount)
    {
        if (amount >= 0)
            throw new ArgumentException("O valor de débito deve ser negativo.", nameof(amount));

        _amount -= amount;
        return Amount;
    }


    public void operator +=(decimal balance) => Credit(balance);

    public void operator -=(decimal balance) => Debit(balance);
}