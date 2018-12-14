using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = SqlHelper.CreateInsert(typeof(Income)),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = income.UserId;
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = income.Date;
                    cmd.Parameters.Add("@amount", SqlDbType.Float).Value = income.Amount;
                    connection.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return (int) reader[0];
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        public bool UpdateIncome(Income income)
        {
            throw new NotImplementedException();
        }

        public bool DeleteIncome(Income income)
        {
            throw new NotImplementedException();
        }

        public Income GetIncome(int incomeId)
        {
            var income = new Income();
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = SqlHelper.CreateGet(typeof(Income)),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = incomeId;
                    connection.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return income;
                        
                        income.Id = (int) reader[0];
                        income.UserId = (Guid) reader[1];
                        income.Amount = (double) reader[2];
                        income.Date = (DateTime) reader[3];
                    }
                }
            }

            return income;
        }

        public List<Income> GetIncomes(Guid userId, DateTime? from = null, DateTime? to = null)
        {
            var incomes = new List<Income>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = SqlHelper.CreateGetAll(typeof(Income)),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;
                    cmd.Parameters.Add("@from", SqlDbType.Date).Value = 
                        from ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    cmd.Parameters.Add("@to", SqlDbType.Date).Value = 
                        to ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 1);
                    connection.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            incomes.Add(new Income
                            {
                                Id = (int) reader[0],
                                UserId = (Guid) reader[1],
                                Amount = (double) reader[2],
                                Date = (DateTime) reader[3]
                            });
                        }
                    }
                }
            }

            return incomes;
        }
    }
}