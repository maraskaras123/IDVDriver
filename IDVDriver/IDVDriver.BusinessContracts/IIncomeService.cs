using System;
using System.Collections.Generic;
using IDVDriver.Domain;

namespace IDVDriver.BusinessContracts
{
    public interface IIncomeService
    {
        int CreateIncome(Income income);
        bool UpdateIncome(Income income);
        bool DeleteIncome(int incomeId);
        Income GetIncome(int incomeId);
        List<Income> GetIncomes(Guid userId, DateTime? from = null, DateTime? to = null);
    }
}
