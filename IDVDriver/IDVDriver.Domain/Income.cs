using System;

namespace IDVDriver.Domain
{
    public class Income
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}