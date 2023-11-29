using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

public class Transaction
{
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string CustomerName { get; set; }
}

public class Program
{
    static void Main()
    {
        List<Transaction> transactions = new List<Transaction>
        {
            new Transaction { Date = new DateTime(2023, 1, 15), Amount = 100.50, CustomerName = "Customer1" },
            new Transaction { Date = new DateTime(2023, 2, 8), Amount = 75.20, CustomerName = "Customer2" },
            new Transaction { Date = new DateTime(2023, 2, 8), Amount = 55.20, CustomerName = "Customer3" },
            new Transaction { Date = new DateTime(2023, 3, 22), Amount = 150.75, CustomerName = "Customer4" },
            new Transaction { Date = new DateTime(2023, 4, 10), Amount = 120.30, CustomerName = "Customer5" },
            new Transaction { Date = new DateTime(2023, 4, 10), Amount = 110.30, CustomerName = "Customer6" },
            new Transaction { Date = new DateTime(2023, 5, 5), Amount = 90.00, CustomerName = "Customer7" },
            new Transaction { Date = new DateTime(2023, 5, 5), Amount = 9.00, CustomerName = "Customer8" },
            new Transaction { Date = new DateTime(2023, 6, 18), Amount = 200.50, CustomerName = "Customer9" },
        };

        // Sorteer transacties oplopend op datum en print
        SortTransactions(transactions, CompareByDate);
        PrintTransactions("Transactions sorted by ascending date:", transactions);

        // Sorteer de eerder gesorteerde lijst aflopend op amount en print
        SortTransactions(transactions, CompareByDate, CompareByAmountDescending);
        PrintTransactions("Transactions sorted by descending amount for transactions with the same date:", transactions);
    }

    // Gebruik gemaakt van het zogeheten Bubble Sort algoritme
    // Loop door de lijsten en controleer of deze voldoet aan de sort voorwaarden
    // Doet hij dat niet, dan wordt er geswapt en wordt de juiste sortering terug gegeven.
    // Gebruik generic type T, zodat de functie voor meerdere doeleinden kan worden gebruikt.
    private static void SortTransactions<T>(List<T> list, params Comparison<T>[] comparisons)
    {
        int n = list.Count;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = n - 1; j > i; j--)
            {
                int comparison = 0;

                foreach (var compareMethod in comparisons)
                {
                    comparison = compareMethod(list[j - 1], list[j]);

                    // Als de comparison niet 0 is, dan gaat de code door naar de volgende if, zodat er gesorteerd kan worden.
                    if (comparison != 0)
                    {
                        break;
                    }
                }

                // Resultaat > 0? -> Sortering is nodig, dus swappen
                if (comparison > 0)
                {
                    Swap(list, j - 1, j);
                }
            }
        }
    }

    // Wissel elementen in een lijst, zonder dat je het specifieke type weet (generic T) -> zodat de functie voor meerdere doeleinden kan worden gebruikt.
    private static void Swap<T>(List<T> list, int index1, int index2)
    {
        // Haal element op index 1 op en stop in tijdelijke var
        T temp = list[index1];
        // Vervang het element op index1 met die van index2
        list[index1] = list[index2];
        // Plaats op index2 het originele element uit index1
        list[index2] = temp;
    }

    // Leek me netjes om dit in een losse methode te doen
    private static void PrintTransactions(string header, List<Transaction> transactions)
    {
        Console.WriteLine(header);
        foreach (var transaction in transactions)
        {
            Console.WriteLine($"Date: {transaction.Date}, Amount: {transaction.Amount}, Customer: {transaction.CustomerName}");
        }
        Console.WriteLine();
    }

    public static int CompareByDate(Transaction t1, Transaction t2)
    {
        return t1.Date.CompareTo(t2.Date);
    }

    public static int CompareByAmountDescending(Transaction t1, Transaction t2)
    {
        return t2.Amount.CompareTo(t1.Amount);
    }

    // Wat tests om de sorteer functies te testen
    [TestClass]
    public class ProgramTests
    {
        private static List<Transaction> transactions;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 1, 15), Amount = 100.50, CustomerName = "Customer1" },
                new Transaction { Date = new DateTime(2023, 2, 8), Amount = 75.20, CustomerName = "Customer2" },
                new Transaction { Date = new DateTime(2023, 2, 8), Amount = 55.20, CustomerName = "Customer3" },
                new Transaction { Date = new DateTime(2023, 3, 22), Amount = 150.75, CustomerName = "Customer4" },
                new Transaction { Date = new DateTime(2023, 4, 10), Amount = 120.30, CustomerName = "Customer5" },
                new Transaction { Date = new DateTime(2023, 4, 10), Amount = 110.30, CustomerName = "Customer6" },
                new Transaction { Date = new DateTime(2023, 5, 5), Amount = 90.00, CustomerName = "Customer7" },
                new Transaction { Date = new DateTime(2023, 5, 5), Amount = 9.00, CustomerName = "Customer8" },
                new Transaction { Date = new DateTime(2023, 6, 18), Amount = 200.50, CustomerName = "Customer9" },
            };
        }

        [TestMethod]
        public void TestTransactionSortByDate()
        {
            Program.SortTransactions(transactions, Program.CompareByDate);

            for (int i = 0; i < transactions.Count - 1; i++)
            {
                Assert.IsTrue(transactions[i].Date <= transactions[i + 1].Date);
            }
        }

        [TestMethod]
        public void TestTransactionSortByAmountDescending()
        {
            Program.SortTransactions(transactions, Program.CompareByAmountDescending);

            for (int i = 0; i < transactions.Count - 1; i++)
            {
                Assert.IsTrue(transactions[i].Amount >= transactions[i + 1].Amount);
            }
        }

        [TestMethod]
        public void TestTransactionSortByDateAndAmount()
        {
            Program.SortTransactions(transactions, Program.CompareByDate);
            Program.SortTransactions(transactions, Program.CompareByAmountDescending);

            for (int i = 0; i < transactions.Count - 1; i++)
            {
                Assert.IsTrue(transactions[i].Date <= transactions[i + 1].Date);
                if (transactions[i].Date == transactions[i + 1].Date)
                {
                    Assert.IsTrue(transactions[i].Amount >= transactions[i + 1].Amount);
                }
            }
        }
    }
}
