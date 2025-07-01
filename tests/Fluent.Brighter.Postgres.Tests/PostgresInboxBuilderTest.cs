using System.Data.Common;

using Paramore.Brighter;
using Paramore.Brighter.Inbox;
using Paramore.Brighter.Inbox.Postgres;
using Paramore.Brighter.PostgreSql;

using Xunit;

namespace Fluent.Brighter.Postgres.Tests;

public class PostgresInboxBuilderTest
{
     private const string TestConnectionString = "Host=localhost;Username=test;Password=test;Database=testdb";
 
     [Theory] 
     [InlineData(InboxScope.All)]
     [InlineData(InboxScope.Commands)]
     [InlineData(InboxScope.Events)]
     public void Scope_WhenSet_SetsValueCorrectly(InboxScope scope)
     {
         var builder = new PostgresInboxBuilder();
         var result = builder.Scope(scope);

            Assert.Equal(scope, GetPrivateField<InboxScope>(builder, "_scope"));
            Assert.Same(builder, result); // Fluent chaining
     }

     [Fact]
     public void OnceOnly_WhenSet_SetsValueCorrectly()
     {
         var builder = new PostgresInboxBuilder();
         var result = builder.OnceOnly(false);

         Assert.False(GetPrivateField<bool>(builder, "_onceOnly"));
         Assert.Same(builder, result);
     }

     [Theory]
     [InlineData(OnceOnlyAction.Throw)]
     [InlineData(OnceOnlyAction.Warn)]
     public void ActionOnExists_WhenSet_SetsValueCorrectly(OnceOnlyAction action)
     {
         var builder = new PostgresInboxBuilder();
         var result = builder.ActionOnExists(action);

         Assert.Equal(action, GetPrivateField<OnceOnlyAction>(builder, "_actionOnExists"));
         Assert.Same(builder, result);
     }

     [Fact]
     public void Context_WhenSet_SetsValueCorrectly()
     {
         var builder = new PostgresInboxBuilder();
         Func<Type, string> contextFunc = _ => "customContext";
         var result = builder.Context(contextFunc);

         Assert.Same(contextFunc, GetPrivateField<Func<Type, string>>(builder, "_context"));
         Assert.Same(builder, result);
     }

     [Fact]
     public void Configuration_WithRelationalConfig_SetsValue()
     {
         var config = new RelationalDatabaseConfiguration(TestConnectionString);
         var builder = new PostgresInboxBuilder().Connection(config);

         Assert.Same(config, GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration"));
     }

     [Fact]
     public void Configuration_WithAction_BuildsFromBuilder()
     {
         var builder = new PostgresInboxBuilder();
         builder.Connection(cfg => cfg.ConnectionString("newConn"));

         var config = GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration");
         Assert.NotNull(config);
         Assert.Equal("newConn", config.ConnectionString);
     }

     [Fact]
     public void ConfigurationIfIsMissing_WhenNull_SetsValue()
     {
         var defaultConfig = new RelationalDatabaseConfiguration("defaultConn");
         var builder = new PostgresInboxBuilder();

         builder.SetConnectionIfIsMissing(defaultConfig);

         Assert.Same(defaultConfig, GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration"));
     }

     [Fact]
     public void ConfigurationIfIsMissing_WhenAlreadySet_DoesNotOverwrite()
     {
         var initialConfig = new RelationalDatabaseConfiguration("initialConn");
         var defaultConfig = new RelationalDatabaseConfiguration("defaultConn");

         var builder = new PostgresInboxBuilder().Connection(initialConfig);
         builder.SetConnectionIfIsMissing(defaultConfig);

         Assert.Same(initialConfig, GetPrivateField<RelationalDatabaseConfiguration>(builder, "_configuration"));
     }

     [Fact]
     public void UseUnitOfWork_WhenTrue_SetsFlag()
     {
         var builder = new PostgresInboxBuilder().UseUnitOfWork(true);
         Assert.True(GetPrivateField<bool>(builder, "_useUnitOfWork"));
     }

     [Fact]
     public void EnableUnitOfWork_SetsUseUnitOfWorkToTrue()
     {
         var builder = new PostgresInboxBuilder().EnableUnitOfWork();
         Assert.True(GetPrivateField<bool>(builder, "_useUnitOfWork"));
     }

     [Fact]
     public void DisableUnitOfWork_SetsUseUnitOfWorkToFalse()
     {
         var builder = new PostgresInboxBuilder().DisableUnitOfWork();
         Assert.False(GetPrivateField<bool>(builder, "_useUnitOfWork"));
     }

     [Fact]
     public void UnitOfWorkConnectionProvider_SetsProvider()
     {
         var mockProvider = new MockRelationalDbConnectionProvider();
         var builder = new PostgresInboxBuilder().UnitOfWorkConnectionProvider(mockProvider);

         Assert.Same(mockProvider, GetPrivateField<IAmARelationalDbConnectionProvider>(builder, "_unitOfWork"));
     }

     [Fact]
     public void Build_WithDefaultValues_CreatesInboxWithDefaults()
     {
         var config = new RelationalDatabaseConfiguration(TestConnectionString);
         var builder = new PostgresInboxBuilder().Connection(config);
         var inboxConfig = builder.Build();

         Assert.NotNull(inboxConfig);
         Assert.IsType<PostgreSqlInbox>(inboxConfig.Inbox);
         Assert.Equal(InboxScope.All, inboxConfig.Scope);
         Assert.True(inboxConfig.OnceOnly);
         Assert.Equal(OnceOnlyAction.Throw, inboxConfig.ActionOnExists);
     }

     [Fact]
     public void Build_WhenUseUnitOfWorkTrue_UsesProvidedConnectionProvider()
     {
         var config = new RelationalDatabaseConfiguration(TestConnectionString);
         var mockProvider = new MockRelationalDbConnectionProvider();

         var builder = new PostgresInboxBuilder()
             .Connection(config)
             .EnableUnitOfWork()
             .UnitOfWorkConnectionProvider(mockProvider);

         var inboxConfig = builder.Build();
         var inbox = Assert.IsType<PostgreSqlInbox>(inboxConfig.Inbox);

         var inboxField = inbox.GetType().GetField("_connectionProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         Assert.Same(mockProvider, inboxField?.GetValue(inbox));
     }

     [Fact]
     public void Build_WhenUseUnitOfWorkFalse_CreatesNewConnectionProvider()
     {
         var config = new RelationalDatabaseConfiguration(TestConnectionString);
         var builder = new PostgresInboxBuilder().Connection(config).DisableUnitOfWork();

         var inboxConfig = builder.Build();
         var inbox = Assert.IsType<PostgreSqlInbox>(inboxConfig.Inbox);

         var inboxField = inbox.GetType().GetField("_connectionProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         Assert.IsType<PostgreSqlConnectionProvider>(inboxField?.GetValue(inbox));
     }

     // Helper to access private fields via reflection
     private static T GetPrivateField<T>(object obj, string fieldName)
     {
         var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         return (T)field?.GetValue(obj)!;
     }
     
     // Mock implementation for testing
     public class MockRelationalDbConnectionProvider : IAmARelationalDbConnectionProvider
     {
         public DbConnection GetConnection()
         {
             throw new NotImplementedException();
         }

         public Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken)
         {
             throw new NotImplementedException();
         }
     }
}

