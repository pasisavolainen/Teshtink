using System;
using System.Data.SqlClient;
using BenchmarkDotNet.Attributes;
using Dapper;

namespace nullextension_benchmark
{
    class ConnectionStrategy
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";

        private static SqlConnection _singleConnection;

        public static void Reset()
        {
            if (_singleConnection != null)
            {
                _singleConnection.Dispose();
                _singleConnection = null;
            }
        }
        public static void SingleConnection(Action<SqlConnection> action)
        {
            if (_singleConnection == null)
                _singleConnection = new SqlConnection(_connectionString);

            action(_singleConnection);
        }
        public static void AlwaysNew(Action<SqlConnection> action)
        {
            using (var c = new SqlConnection(_connectionString))
                action(c);
        }        
    }

    public class ConnectionPoolingBenchmark : IDisposable
    {        
        private void Activity(SqlConnection connection)
        {
            connection.QuerySingle("select 1");
        }

        [Benchmark(Baseline = true)]
        public void OneConnectionOpened()
        {
            ConnectionStrategy.SingleConnection(Activity);
        }

        [Benchmark]
        public void AlwaysNewConnection()
        {
            ConnectionStrategy.AlwaysNew(Activity);
        }

        public void Dispose()
        {
            ConnectionStrategy.Reset();
        }
    }
}
