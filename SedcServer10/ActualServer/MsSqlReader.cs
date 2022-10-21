using Sedc.Server.Attributes;
using Sedc.Server.Logging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ActualServer
{
    internal class Novel
    {
        [DbOrder(0)]
        public int ID { get; set; }

        [DbOrder(1)]
        public string Title { get; set; }

        [DbOrder(2)]
        public int AuthorID { get; set; }

        [DbOrder(3)]
        public bool IsRead { get; set; }
    }

    internal class Author
    {
        [DbOrder(0)]
        public int ID { get; set; }

        [DbOrder(1)]
        public string Name { get; set; }
    }

    internal class PropertyHelper
    {
        public PropertyInfo Info { get; set; }
        public int Order { get; set; }
    }

    internal class MsSqlReader
    {
        private readonly string ConnectionString = "Server=.;Database=master;User Id=demo;Password=demo;";

        private readonly ILogger Logger;

        public MsSqlReader(ILogger logger)
        {
            Logger = logger;
        }

        private static readonly Dictionary<string, Type> TypeMap = new Dictionary<string, Type>
        {
            { "novels", typeof(Novel) },
            { "authors", typeof(Author) }
        };


        [RouteName("list")]
        public string[] ListDatabases()
        {
            using var sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            using var command = sqlConnection.CreateCommand();
            command.CommandText = "EXEC sp_databases";
            using var reader = command.ExecuteReader();
            var result = new List<string>();
            while (reader.Read())
            {
                var dbName = reader.GetString(0);
                result.Add(dbName);
            }
            return result.ToArray();
        }

        [RouteName("tables")]
        public string[] ListTables(string dbName)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            using var command = sqlConnection.CreateCommand();
            command.CommandText = $"use [{dbName}]";
            command.ExecuteNonQuery();

            command.CommandText = "select * from sys.tables order by name";
            var reader = command.ExecuteReader();
            var result = new List<string>();

            while (reader.Read())
            {
                var tableName = reader.GetString(0);
                result.Add(tableName);
            }
            return result.ToArray();
        }


        [RouteName("data")]
        public object[] ShowData(string dbName, string tableName)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            using var command = sqlConnection.CreateCommand();
            command.CommandText = $"use [{dbName}]";
            command.ExecuteNonQuery();

            command.CommandText = $"select top 10 * from {tableName}";
            var reader = command.ExecuteReader();

            var typeFound = TypeMap.TryGetValue(tableName, out Type type);

            if (!typeFound)
            {
                return new object[0];
            }

            //var listType = typeof(List<>);
            //var specificListType = listType.MakeGenericType(type);
            //var result = (IList) Activator.CreateInstance(specificListType);

            var result = new List<object>();

            var props = type.GetProperties();
            var propHelpers = new List<PropertyHelper>();
            foreach (var prop in props)
            {
                var attribute = prop.GetCustomAttribute<DbOrderAttribute>();
                var phelper = new PropertyHelper
                {
                    Info = prop,
                    Order = attribute.Order,
                };
                propHelpers.Add(phelper);
            }

            while (reader.Read())
            {
                var item = Activator.CreateInstance(type);
                foreach (var phelper in propHelpers)
                {
                    if (phelper.Info.PropertyType == typeof(int))
                    {
                        var value = reader.GetInt32(phelper.Order);
                        phelper.Info.SetValue(item, value);
                    }
                    if (phelper.Info.PropertyType == typeof(string))
                    {
                        var value = reader.GetString(phelper.Order);
                        phelper.Info.SetValue(item, value);
                    }
                    if (phelper.Info.PropertyType == typeof(bool))
                    {
                        var value = reader.GetBoolean(phelper.Order);
                        phelper.Info.SetValue(item, value);
                    }
                }

                result.Add(item);
            }

            return result.ToArray();
        }
    }
}
