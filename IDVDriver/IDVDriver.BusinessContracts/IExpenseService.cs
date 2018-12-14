using System;
using System.Collections.Generic;
using IDVDriver.Domain;

namespace IDVDriver.BusinessContracts
{
    public interface IExpenseService
    {
        int CreateExpense(Expense expense);
        bool UpdateExpense(Expense expense);
        bool DeleteExpense(int expenseId);
        Expense GetExpense(int expenseId);
        List<Expense> GetExpenses(Guid userId, DateTime? from = null, DateTime? to = null, 
            ExpenseType type = ExpenseType.Fuel);
    }
}