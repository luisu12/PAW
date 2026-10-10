using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;

namespace PAW.Repositories;

/// <summary>
/// Interface for basic repository operations.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepositoryBase<T>
{
    /// <summary>
    /// Method that updates the entity if exists otherwise inserts a new record
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <param name="isUpdating">The indicator that tells if I need to update or create</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<bool> UpsertAsync(T entity, bool isUpdating);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> CreateAsync(T entity);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// Reads all entities of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    Task<IEnumerable<T>> ReadAsync();

    /// <summary>
    /// Finds an entity from the list of objects by int id
    /// </summary>
    /// <param name="id">integer</param>
    /// <returns>Entity by id</returns>
    Task<T> FindAsync(int id);

    /// <summary>
    /// Finds an entity from the list of objects by decimal id
    /// </summary>
    /// <param name="id">decimal id</param>
    /// <returns>Entity by id</returns>
    Task<T> FindAsync(decimal id);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// Updates multiple entities asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> UpdateManyAsync(IEnumerable<T> entities);

    /// <summary>
    /// Checks if an entity exists asynchronously.
    /// </summary>
    /// <param name="entity">The entity to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
    Task<bool> ExistsAsync(T entity);
}

/// <summary>
/// Base class for repository operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly ProductDbContext _context;
    protected ProductDbContext DbContext => _context;
    protected DbSet<T> DbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    public RepositoryBase()
    {
        _context = new ProductDbContext();
        DbSet<T> _sdbSet = _context.Set<T>();
    }

    public async Task<bool> UpsertAsync(T entity, bool isUpdating)
    {
        return isUpdating
            ? await UpdateAsync(entity)
            : await CreateAsync(entity);
    }

    /// <summary>
    /// Creates an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be saved in the database.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> CreateAsync(T entity)
    {
        try
        {
            await _context.AddAsync(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Updates an existing entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _context.Update(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            // Include inner exception details in PAWException so callers see diagnostic information during debugging.
            var details = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            var full = $"Repository UpdateAsync failed: {details} -- Stack: {ex.StackTrace}";
            throw new PAWException(new Exception(full, ex));
        }
    }

    /// <summary>
    /// Updates multiple entities of type T asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
    {
        try
        {
            _context.UpdateRange(entities);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Deletes an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            _context.Remove(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Reads all entities of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<IEnumerable<T>> ReadAsync()
    {
        try
        {
            return await _context.Set<T>().ToListAsync();
        }
        catch (Exception ex)
        {
            // If a SqlNullValueException occurred during materialization, collect quick diagnostics
            try
            {
                var isSqlNull = ex is System.Data.SqlTypes.SqlNullValueException || ex.InnerException is System.Data.SqlTypes.SqlNullValueException;
                if (isSqlNull)
                {
                    try
                    {
                        var entityType = _context.Model.FindEntityType(typeof(T));
                        var tableName = entityType?.GetTableName();
                        var schema = entityType?.GetSchema();
                        var fullName = tableName != null ? (string.IsNullOrEmpty(schema) ? $"[{tableName}]" : $"[{schema}].[{tableName}]") : typeof(T).Name;

                        using var conn = _context.Database.GetDbConnection();
                        if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = $"SELECT TOP 5 * FROM {fullName}";
                        using var reader = await cmd.ExecuteReaderAsync();
                        var rowsInfo = new System.Text.StringBuilder();
                        var rowIndex = 0;
                        while (await reader.ReadAsync() && rowIndex < 5)
                        {
                            rowsInfo.AppendLine($"Row {rowIndex}:");
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var name = reader.GetName(i);
                                var isDbNull = reader.IsDBNull(i);
                                rowsInfo.Append($"  {name}={(isDbNull ? "NULL" : reader.GetValue(i)?.ToString())}\n");
                            }
                            rowIndex++;
                        }

                        var detail = $"SqlNullValueException while reading {typeof(T).Name} from {fullName}. Sample rows:\n{rowsInfo}";
                        // Additional quick checks for common numeric columns when reading UserRole
                        if (typeof(T).Name.IndexOf("UserRole", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            try
                            {
                                using var countCmd = conn.CreateCommand();
                                countCmd.CommandText = $"SELECT COUNT(*) FROM {fullName} WHERE RoldID IS NULL OR UserID IS NULL";
                                var nullCount = (long)countCmd.ExecuteScalar();
                                detail += $"\nUserRole null count (RoldID or UserID): {nullCount}";
                            }
                            catch { }
                        }

                        System.Diagnostics.Debug.WriteLine(detail);
                        throw new PAWException(new Exception(detail, ex));
                    }
                    catch (Exception inner)
                    {
                        // If diagnostics collection fails, fall back to original exception
                        throw new PAWException(new Exception($"ReadAsync failed and diagnostics collection failed: {inner.Message}", ex));
                    }
                }
            }
            catch { }

            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Reads an entity of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<T> FindAsync(decimal id)
    {
        try
        {
            return await _context.Set<T>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    public async Task<T> FindAsync(int id)
    {
        try
        {
            return await _context.Set<T>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Checks if an entity of type T exists asynchronously.
    /// </summary>
    /// <param name="entity">The entity to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
    public async Task<bool> ExistsAsync(T entity)
    {
        try
        {
            var items = await ReadAsync();
            return items.Any(x => x.Equals(entity));
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Saves changes to the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    protected async Task<bool> SaveAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public virtual async Task<object> GetData()
    {
        //llame suppliers y products
        await System.Threading.Tasks.Task.Run(() => { });
        return null;
    }
}
