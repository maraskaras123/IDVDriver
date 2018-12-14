using System;

namespace IDVDriver.Domain
{
    public class Expense
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public ExpenseType Type { get; set; }
    }
}