using AML.TransactionTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RulesEngine.Models;
using System.Text.Json;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Infrastructure.SQLite
{
    public class SQLiteContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public DbSet<CurrencyLookup> Currencies { get; set; }
        public DbSet<TransactionTypeLookup> TransactionTypes { get; set; }
        public virtual DbSet<Workflow> Workflows { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public virtual DbSet<RuleViolation> RuleViolations { get; set; }

        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(p => p.Id)
                .IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Surename)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Address)
                .HasMaxLength(500)
                .IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Deleted)
                .HasDefaultValue(false);
            modelBuilder.Entity<Customer>().Property(p => p.Birthdate)
                .IsRequired();

            modelBuilder.Entity<Transaction>().Property(p => p.Id)
                .IsRequired();
            modelBuilder.Entity<Transaction>().Property(p => p.Type)
                .IsRequired()
                .HasConversion<int>();
            modelBuilder.Entity<Transaction>().Property(p => p.CustomerId)
                .IsRequired();
            modelBuilder.Entity<Transaction>().Property(p => p.TransactionCurrency)
                .IsRequired()
                .HasConversion<int>();
            modelBuilder.Entity<Transaction>().Property(p => p.Date)
                .IsRequired();
            modelBuilder.Entity<Transaction>().Property(p => p.Amount)
                .IsRequired();
            modelBuilder.Entity<Transaction>().Property(p => p.Flagged)
                .HasDefaultValue(false);
            modelBuilder.Entity<Transaction>().Property(p => p.Validated)
                .HasDefaultValue(false);

            modelBuilder.Entity<CurrencyLookup>()
                .HasData(Enum.GetValues(typeof(Currency))
                    .Cast<Currency>()
                    .Select(x => new CurrencyLookup
                    {
                        Currency = x,
                        Name = x.ToString()
                    })
                );

            modelBuilder.Entity<TransactionTypeLookup>()
                .HasData(Enum.GetValues(typeof(TransactionType))
                    .Cast<TransactionType>()
                    .Select(x => new TransactionTypeLookup
                    {
                        Type = x,
                        Name = x.ToString()
                    })
                );

            modelBuilder.Entity<ScopedParam>()
              .HasKey(k => k.Name);

            modelBuilder.Entity<Workflow>(entity => {
                entity.HasKey(k => k.WorkflowName);
                entity.Ignore(b => b.WorkflowsToInject);
            });

            modelBuilder.Entity<Rule>().HasOne<Rule>().WithMany(r => r.Rules).HasForeignKey("RuleNameFK");

            var serializationOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);

            modelBuilder.Entity<Rule>(entity => {
                entity.HasKey(k => k.RuleName);

                var valueComparer = new ValueComparer<Dictionary<string, object>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c);

                entity.Property(b => b.Properties)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, serializationOptions),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, serializationOptions))
                    .Metadata
                    .SetValueComparer(valueComparer);

                entity.Property(p => p.Actions)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, serializationOptions),
                   v => JsonSerializer.Deserialize<RuleActions>(v, serializationOptions));

                entity.Ignore(b => b.WorkflowsToInject);
            });

            modelBuilder.Entity<RuleViolation>().HasKey(k => k.Id);
            modelBuilder.Entity<RuleViolation>().Property(p => p.TransactionId)
                .IsRequired();
            modelBuilder.Entity<RuleViolation>().Property(p => p.RuleName)
                .IsRequired();
            modelBuilder.Entity<RuleViolation>().Property(p => p.Date)
                .IsRequired();
            modelBuilder.Entity<RuleViolation>().Property(p => p.RuleExpression)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
