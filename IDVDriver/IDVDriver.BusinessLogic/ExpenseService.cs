using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = SqlHelper.CreateInsert(typeof(Expense)),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = expense.UserId;
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = expense.Date;
                    cmd.Parameters.Add("@amount", SqlDbType.Float).Value = expense.Amount;
                    cmd.Parameters.Add("@type", SqlDbType.SmallInt).Value = expense.Type;
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

        public bool UpdateExpense(Expense expense)
        {
            throw new NotImplementedException();
        }

        public bool DeleteExpense(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Expense GetExpense(int expenseId)
        {
            var expense = new Expense();
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = SqlHelper.CreateGet(typeof(Expense)),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = expenseId;
                    connection.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return expense;
                        
                        expense.Id = (int) reader[0];
                        expense.UserId = (Guid) reader[1];
                        expense.Amount = (double) reader[2];
                        expense.Date = (DateTime) reader[3];
                        expense.Type = (ExpenseType) reader[4];
                    }
                }
            }

            return expense;
        }

        public List<Expense> GetExpenses(Guid userId, DateTime? @from = null, DateTime? to = null, ExpenseType type = ExpenseType.Fuel)
        {
            var expenses = new List<Expense>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = SqlHelper.CreateGetAll(typeof(Expense)),
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;
                    cmd.Parameters.Add("@from", SqlDbType.Date).Value = 
                        from ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    cmd.Parameters.Add("@to", SqlDbType.Date).Value = 
                        to ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 1);
                    cmd.Parameters.Add("@type", SqlDbType.SmallInt).Value = type;
                    connection.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            expenses.Add(new Expense
                            {
                                Id = (int) reader[0],
                                UserId = (Guid) reader[1],
                                Amount = (double) reader[2],
                                Date = (DateTime) reader[3],
                                Type = (ExpenseType) reader[4]
                            });
                        }
                    }
                }
            }

            return expenses;
        }
    }
}