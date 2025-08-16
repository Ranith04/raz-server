using Dapper;
using Npgsql;
using RazServer.Settings;
using System.Data;

namespace RazServer.Repositories;

public interface IBaseRepository
{
    public NpgsqlConnection NewConnection { get; }
    public Task RunInTransaction(Func<NpgsqlConnection, NpgsqlTransaction, Task> Run, ILogger? logger = null);
    public Task<T> RunInTransaction<T>(Func<NpgsqlConnection, NpgsqlTransaction, Task<T>> Run, ILogger? logger = null);
}

public class BaseRepository : IBaseRepository
{
    protected readonly IConfiguration _configuration;
    private readonly string ConnectionString;

    public BaseRepository(IConfiguration config)
    {
        _configuration = config;

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        this.ConnectionString = _configuration.GetSection(nameof(PostgresSettings)).Get<PostgresSettings>()!.ConnectionString;
    }

    protected BaseRepository(IServiceProvider serviceProvider)
    {
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        this.ConnectionString = _configuration.GetSection(nameof(PostgresSettings)).Get<PostgresSettings>()!.ConnectionString;
    }

    public NpgsqlConnection NewConnection => new(ConnectionString);

    public async Task<T> RunInTransaction<T>(Func<NpgsqlConnection, NpgsqlTransaction, Task<T>> Run, ILogger? logger = null)
    {
        using var con = NewConnection;

        await con.OpenAsync();

        using var tran = await con.BeginTransactionAsync();

        try
        {
            var res = await Run(con, tran);
            await tran.CommitAsync();
            return res;
        }
        catch (Exception e)
        {
            if (logger is not null)
            {
                logger.LogError(e, "Rolling back transaction:");
            }
            else
            {
                Console.WriteLine("Rolling back transaction:");
                Console.WriteLine(e);
            }

            await tran.RollbackAsync();
            throw;
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public Task RunInTransaction(Func<NpgsqlConnection, NpgsqlTransaction, Task> Run, ILogger? logger = null)
    {
        return RunInTransaction(async (con, tran) =>
        {
            await Run(con, tran);
            return 0;
        }, logger);
    }
}
