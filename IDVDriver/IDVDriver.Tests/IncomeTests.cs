using System;
using IDVDriver.BusinessContracts;
using IDVDriver.BusinessLogic;
using IDVDriver.Domain;
using IDVDriver.Utils;
using NUnit.Framework;

namespace IDVDriver.Tests
{
    [TestFixture]
    public class IncomeTests : IDVDriverTestBase
    {
        private IIncomeService incomeService;
        private Income income1, income2;
        private Guid userId = Guid.NewGuid();
        
        [SetUp]
        protected override void SetUp()
        {
            incomeService = new IncomeService
            {
                ConnectionString = ConnectionString
            };

            using (var context = new IDVContext(ConnectionString))
            {
                context.Users.Add(new User { Id = userId });
                context.SaveChanges();
            }

            income1 = new Income
            {
                UserId = userId,
                Date = new DateTime(2018, 12, 1),
                Amount = 54.23
            };

            income2 = new Income
            {
                UserId = userId,
                Date = new DateTime(2018, 12, 13),
                Amount = 45.99
            };
        }
        [TearDown]
        protected override void Dispose()
        {
            using (var context = new IDVContext(ConnectionString))
            {
                context.Incomes.RemoveRange(context.Incomes);
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();
            }
        }

        [Test]
        [Category("Income")]
        public override void Can_insert_item_to_database()
        {
            income1.Id = incomeService.CreateIncome(income1);

            income1.Id.ShouldNotBeNull();
        }

        [Test]
        [Category("Income")]
        public override void Can_get_item_by_id()
        {
            income1.Id = incomeService.CreateIncome(income1);
            var incomeFromDb = incomeService.GetIncome(income1.Id);

            incomeFromDb.ShouldNotBeNull();
        }

        [Test]
        [Category("Income")]
        public override void Can_get_items()
        {
            income1.Id = incomeService.CreateIncome(income1);
            income2.Id = incomeService.CreateIncome(income2);

            var incomesFromDb = incomeService.GetIncomes(userId);

            incomesFromDb.Count.ShouldEqual(2);
            incomesFromDb[0].Amount.ShouldEqual(54.23);
            incomesFromDb[1].Amount.ShouldEqual(45.99);
        }

        [Test]
        [Category("Income")]
        public void Can_get_items_in_date_range()
        {
            income1.Id = incomeService.CreateIncome(income1);
            income2.Id = incomeService.CreateIncome(income2);

            var incomesFromDb = incomeService.GetIncomes(userId, new DateTime(2018, 12, 10));

            incomesFromDb.Count.ShouldEqual(1);
            incomesFromDb[0].Id.ShouldEqual(income2.Id);
        }

        [Test]
        [Category("Income")]
        public override void Can_delete_item()
        {
            income1.Id = incomeService.CreateIncome(income1);
            incomeService.DeleteIncome(income1.Id);

            var incomesFromDb = incomeService.GetIncomes(userId);
            incomesFromDb.Count.ShouldEqual(0);
        }

        [Test]
        [Category("Income")]
        public override void Can_update_item()
        {
            income1.Id = incomeService.CreateIncome(income1);

            income1.Amount = 20.00;

            var success = incomeService.UpdateIncome(income1);

            if (!success) throw new NotImplementedException();
            
            var incomeFromDb = incomeService.GetIncome(income1.Id);
            incomeFromDb.Amount.ShouldEqual(20.0);
        }
    }
}