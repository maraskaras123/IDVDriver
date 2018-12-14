using System;
using System.Data;
using System.Data.SqlClient;
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
        private Guid userId = new Guid();
        private string connectionString = "";
        
        [SetUp]
        protected override void SetUp()
        {
            incomeService = new IncomeService
            {
                ConnectionString = connectionString
            };

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

        protected override void Dispose()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand
                {
                    CommandText = typeof(Income).CreateDeleteAll(),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Test]
        [Category("Income")]
        public override void Can_insert_item_to_database()
        {
            income1.Id = incomeService.CreateIncome(income1);

            income1.Id.ShouldNotEqual(1);
        }

        public override void Can_get_item_by_id()
        {
            income1.Id = incomeService.CreateIncome(income1);
            var incomeFromDb = incomeService.GetIncome(income1.Id);

            incomeFromDb.ShouldNotBeNull();
        }

        public override void Can_get_items()
        {
            income1.Id = incomeService.CreateIncome(income1);
            income2.Id = incomeService.CreateIncome(income2);

            var incomesFromDb = incomeService.GetIncomes(userId);

            incomesFromDb.Count.ShouldEqual(2);
            incomesFromDb[0].Amount.ShouldEqual(54.23);
            incomesFromDb[1].Amount.ShouldEqual(45.99);
        }

        public override void Can_delete_item()
        {
            income1.Id = incomeService.CreateIncome(income1);
            incomeService.DeleteIncome(income1.Id);

            var incomesFromDb = incomeService.GetIncomes(userId);
            incomesFromDb.Count.ShouldEqual(0);
        }

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