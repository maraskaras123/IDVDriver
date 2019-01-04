using System;
using IDVDriver.BusinessContracts;
using IDVDriver.BusinessLogic;
using IDVDriver.Domain;
using IDVDriver.Utils;
using NUnit.Framework;

namespace IDVDriver.Tests
{
    [TestFixture]
    public class ExpenseTests : IDVDriverTestBase
    {
        private IExpenseService expenseService;
        private Expense expense1, expense2;
        private Guid userId = Guid.NewGuid();
        
        [SetUp]
        protected override void SetUp()
        {
            expenseService = new ExpenseService
            {
                ConnectionString = ConnectionString
            };

            using (var context = new IDVContext(ConnectionString))
            {
                context.Users.Add(new User { Id = userId });
                context.SaveChanges();
            }

            expense1 = new Expense
            {
                UserId = userId,
                Date = new DateTime(2018, 12, 1),
                Amount = 54.23,
                Type = ExpenseType.Fuel
            };

            expense2 = new Expense
            {
                UserId = userId,
                Date = new DateTime(2018, 12, 13),
                Amount = 45.99,
                Type = ExpenseType.Service
            };
        }

        [TearDown]
        protected override void Dispose()
        {
            using (var context = new IDVContext(ConnectionString))
            {
                context.Expenses.RemoveRange(context.Expenses);
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();
            }
        }

        [Test]
        [Category("Expense")]
        public override void Can_insert_item_to_database()
        {
            expense1.Id = expenseService.CreateExpense(expense1);

            expense1.Id.ShouldNotBeNull();
        }

        [Test]
        [Category("Expense")]
        public override void Can_get_item_by_id()
        {
            expense1.Id = expenseService.CreateExpense(expense1);
            var expenseFromDb = expenseService.GetExpense(expense1.Id);

            expenseFromDb.ShouldNotBeNull();
        }

        [Test]
        [Category("Expense")]
        public override void Can_get_items()
        {
            expense1.Id = expenseService.CreateExpense(expense1);
            expense2.Id = expenseService.CreateExpense(expense2);

            var expensesFromDb = expenseService.GetExpenses(userId);

            expensesFromDb.Count.ShouldEqual(2);
            expensesFromDb[0].Amount.ShouldEqual(54.23);
            expensesFromDb[1].Amount.ShouldEqual(45.99);
        }

        [Test]
        [Category("Expense")]
        public void Can_get_items_in_date_range()
        {
            expense1.Id = expenseService.CreateExpense(expense1);
            expense2.Id = expenseService.CreateExpense(expense2);

            var expensesFromDb = expenseService.GetExpenses(userId, new DateTime(2018, 12, 10));

            expensesFromDb.Count.ShouldEqual(1);
            expensesFromDb[0].Id.ShouldEqual(expense2.Id);
        }

        [Test]
        [Category("Expense")]
        public void Can_get_items_by_type()
        {
            expense1.Id = expenseService.CreateExpense(expense1);
            expense2.Id = expenseService.CreateExpense(expense2);

            var expensesFromDb = expenseService.GetExpenses(userId, type: ExpenseType.Service);

            expensesFromDb.Count.ShouldEqual(1);
            expensesFromDb[0].Id.ShouldEqual(expense2.Id);
        }

        [Test]
        [Category("Expense")]
        public override void Can_delete_item()
        {
            expense1.Id = expenseService.CreateExpense(expense1);
            expenseService.DeleteExpense(expense1.Id);

            var expensesFromDb = expenseService.GetExpenses(userId);
            expensesFromDb.Count.ShouldEqual(0);
        }

        [Test]
        [Category("Expense")]
        public override void Can_update_item()
        {
            expense1.Id = expenseService.CreateExpense(expense1);

            expense1.Amount = 20.00;

            var success = expenseService.UpdateExpense(expense1);

            if (!success) throw new NotImplementedException();
            
            var expenseFromDb = expenseService.GetExpense(expense1.Id);
            expenseFromDb.Amount.ShouldEqual(20.0);
        }
    }
}