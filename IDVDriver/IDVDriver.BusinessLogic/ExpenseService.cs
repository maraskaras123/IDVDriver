using System;
using System.Collections.Generic;
using System.Linq;
using IDVDriver.BusinessContracts;
using IDVDriver.Domain;
using IDVDriver.Utils;

namespace IDVDriver.BusinessLogic
{
    public class ExpenseService : IExpenseService
    {
        public string ConnectionString { get; set; }
        
        public int CreateExpense(Expense expense)
        {
            var validator = new ExpenseValidator();
            var results = validator.Validate(expense);
            if (results.IsValid)
            {
                using (var context = new IDVContext(ConnectionString))
                {
                    context.Expenses.Add(expense);
                    context.SaveChanges();
                    return context.Expenses.Last().Id;
                }
            }
            else
            {
                throw new BusinessException("Cannot create expense", results.Errors);
            }
        }

        public bool UpdateExpense(Expense expense)
        {
            var validator = new ExpenseValidator();
            var results = validator.Validate(expense);
            if (results.IsValid)
            {
                using (var context = new IDVContext(ConnectionString))
                {
                    var expenseFromDb = context.Expenses.First(x => x.Id == expense.Id);
                    expenseFromDb.Amount = expense.Amount;
                    expenseFromDb.Date = expense.Date;
                    var count = context.SaveChanges();
                    return count > 0;
                }
            }
            else
            {
                throw new BusinessException("Cannot update expense", results.Errors);
            }
        }

        public bool DeleteExpense(int expenseId)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                var expense = context.Expenses.First(x => x.Id == expenseId);
                context.Expenses.Remove(expense);
                var count = context.SaveChanges();
                return count > 0;
            }
        }

        public Expense GetExpense(int expenseId)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                return context.Expenses.First(x => x.Id == expenseId);
            }
        }

        public List<Expense> GetExpenses(Guid userId, DateTime? from = null, DateTime? to = null, ExpenseType type = ExpenseType.None)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                var expenses = context.Expenses.Where(x => x.UserId == userId).ToList();

                if (from != null)
                {
                    expenses = expenses.Where(x => x.Date >= from).ToList();
                }
                if (to != null)
                {
                    expenses = expenses.Where(x => x.Date <= to).ToList();
                }
                if (type != ExpenseType.None)
                {
                    expenses = expenses.Where(x => x.Type == type).ToList();
                }

                return expenses;
            }
        }
    }
}