using Microsoft.EntityFrameworkCore;
using ParrotWingsApi.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace ParrotWingsApi.Context;

/// <summary>
/// Контекст доступа к БД.
/// </summary>
[ExcludeFromCodeCoverage]
public class EntityContext : DbContext
{
    public EntityContext(DbContextOptions options) 
        : base(options) { }

    /// <summary>
    /// Список пользователей
    /// </summary>
    public virtual DbSet<User>? Users { get; set; }

    /// <summary>
    /// Список транзакций
    /// </summary>
    public virtual DbSet<Transaction>? Transactions { get; set; }

    /// <summary>
    /// Список обращений
    /// </summary>
    public virtual DbSet<Appeal>? Appeals { get; set; }
}