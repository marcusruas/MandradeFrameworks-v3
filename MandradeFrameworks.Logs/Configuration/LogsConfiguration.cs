using MandradeFrameworks.Logs.Models;
using System;
using Microsoft.Data.SqlClient;
using Dapper;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using Serilog.Exceptions;

namespace MandradeFrameworks.Logs.Configuration
{
    public static class LogsConfiguration
    {
        /// <summary>
        /// Método para adicionar logs em tabela SQL na aplicação.
        /// </summary>
        /// <param name="configuration">Objeto de configurações para aplicação de logs em SQL</param>
        public static void AdicionarLogsSQL(SQLLogsConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
                throw new ArgumentException($"Connection String da base de logs não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(configuration.Tabela))
                throw new ArgumentException("Nome da tabela de logs não pode ser vazio.");

            if (configuration.CriarTabelaSeNaoExistir)
                GerarTabelaSQL(configuration);

            CriarInstanciaSerilog(configuration);
        }

        private static void GerarTabelaSQL(SQLLogsConfiguration configs)
        {
            var consulta = QueryCriacaoTabela(configs);

            try
            {
                using var connection = new SqlConnection(configs.ConnectionString);
                connection.Execute(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu uma falha ao realizar o cadastro da tabela SQL. Detalhes: {ex}");
            }
        }

        private static string QueryCriacaoTabela(SQLLogsConfiguration configs)
        {
            string tabelaComSchema = $"[{configs.Schema}].[{configs.Tabela}]";

            return $@"
                IF (SELECT OBJECT_ID('{tabelaComSchema}')) IS NULL 
                BEGIN
                    CREATE TABLE {tabelaComSchema} (
                       [Id] int IDENTITY(1,1) NOT NULL,
                       [Message] nvarchar(500) NULL,
                       [MessageTemplate] nvarchar(500) NULL,
                       [Level] nvarchar(128) NULL,
                       [TimeStamp] datetime NOT NULL,
                       [Exception] nvarchar(max) NULL,
                       [Properties] nvarchar(max) NULL

                       CONSTRAINT [PK_{configs.Tabela}] PRIMARY KEY CLUSTERED ([Id] ASC) 
                    );

                    CREATE INDEX IX_Consulta_Simplificada ON {tabelaComSchema} (TimeStamp, Message)
                    CREATE INDEX IX_Consulta_Nivel ON {tabelaComSchema} (TimeStamp, Level)
                END
            ";
        }

        private static void CriarInstanciaSerilog(SQLLogsConfiguration configs)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.MSSqlServer(
                    connectionString: configs.ConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions()
                    {
                        TableName = configs.Tabela,
                        SchemaName = configs.Schema
                    }
                )
                .CreateLogger();
        }
    }
}
