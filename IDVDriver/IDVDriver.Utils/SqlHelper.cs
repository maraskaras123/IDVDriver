using System;
using System.Linq;
using IDVDriver.Domain;

namespace IDVDriver.Utils
{
    public static class SqlHelper
    {
        public static string CreateInsert(Type type)
        {
            var command = $"INSERT INTO {type.Name}s";
            var properties = type.GetProperties().ToList();
            var names = properties.Select(x => x.Name);
            properties.Remove(type.GetProperty("Id"));
            command += string.Join(", ", properties.Select(x => x.Name));
            command += ") OUTPUT INSERTED.Id VALUES (";
            command += string.Join(", ", properties.Select(x => "@" + x));
            command += ");";
            return command;
        }

        public static string CreateGet(Type type)
        {
            return $"SELECT * FROM {type.Name}s WHERE Id = @id;";
        }

        public static string CreateGetAll(Type type)
        {
            var command = $"SELECT * FROM {type.Name}s WHERE Id = @id";
            if (type != typeof(User))
            {
                command +=" AND From = @from AND To = @to";
            }
            if (type == typeof(Expense))
            {
                command += " AND Type = @type";
            }
            return command + ";";
        }
    }
}