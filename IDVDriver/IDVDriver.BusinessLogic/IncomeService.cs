using System;
using System.Collections.Generic;
using System.Linq;
using IDVDriver.BusinessContracts;
using IDVDriver.Domain;
using IDVDriver.Utils;

namespace IDVDriver.BusinessLogic
{
    public class IncomeService : IIncomeService
    {
        public string ConnectionString { get; set; }
        
        public int CreateIncome(Income income)
        {
            var validator = new IncomeValidator();
            var results = validator.Validate(income);
            if (results.IsValid)
            {
                using (var context = new IDVContext(ConnectionString))
                {
                    context.Incomes.Add(income);
                    context.SaveChanges();
                    return context.Incomes.Last().Id;
                }
            }
            else
            {
                throw new BusinessException("Cannot create income", results.Errors);
            } 
        }

        public bool UpdateIncome(Income income)
        {
            var validator = new IncomeValidator();
            var results = validator.Validate(income);
            if (results.IsValid)
            {
                using (var context = new IDVContext(ConnectionString))
                {
                    var incomeFromDb = context.Incomes.First(x => x.Id == income.Id);
                    incomeFromDb.Amount = income.Amount;
                    incomeFromDb.Date = income.Date;
                    var count = context.SaveChanges();
                    return count > 0;
                }
            }
            else
            {
                throw new BusinessException("Cannot update income", results.Errors);
            }
            
        }

        public bool DeleteIncome(int incomeId)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                var income = context.Incomes.First(x => x.Id == incomeId);
                context.Incomes.Remove(income);
                var count = context.SaveChanges();
                return count > 0;
            }
        }

        public Income GetIncome(int incomeId)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                return context.Incomes.First(x => x.Id == incomeId);
            }
        }

        public List<Income> GetIncomes(Guid userId, DateTime? from = null, DateTime? to = null)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                var incomes = context.Incomes.Where(x => x.UserId == userId).ToList();

                if (from != null)
                {
                    incomes = incomes.Where(x => x.Date >= from).ToList();
                }
                if (to != null)
                {
                    incomes = incomes.Where(x => x.Date <= to).ToList();
                }

                return incomes;
            }
        }
    }
}