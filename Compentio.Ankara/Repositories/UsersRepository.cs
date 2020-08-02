namespace Compentio.Ankara.Repositories
{
    using Compentio.Ankara.Models.Companies;
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Repositories.ConnectionFactory;
    using Compentio.Ankara.Repositories.Mappers;
    using Dapper;
    using LanguageExt;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUsersRepository
    {
        Task<Option<User>> GetUser(string login);
        Task<IEnumerable<User>> GetUsers(IEnumerable<long> departmentIds, bool? active = false);
        Task AddUser(User user, string userLogin);
        Task RemoveUser(long userId, string userLogin);
        Task ModifyUser(User user, string userLogin);
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly IAdoConnectionFactory _connectionFactory;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public UsersRepository(IAdoConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            SqlMapper.AddTypeHandler(new JsonTypeHandler<IEnumerable<UserRole>>());
        }
        public async Task<IEnumerable<User>> GetUsers(IEnumerable<long> departmentIds, bool? active = false)
        {
            using var connection = _connectionFactory.CreateConnection();
            string departments = string.Empty;
            var query = string.Empty;

            if (departmentIds != null)
            {
                foreach (var d in departmentIds)
                {
                    departments += $"{d},";
                }
            }

            query = active.HasValue && active.Value ?
                @"SELECT * FROM app.Users AS u WHERE IsActive = 1 AND GETDATE() Between u.ValidFrom AND u.ValidTo " :
                @"SELECT * FROM [dbo].[workflowuser] AS u
                        WHERE userIsBlocked = 0 AND d.departActive = 1 AND ad.IsActive = 1 AND u.userIsFromAd = 1";
          

            if (departmentIds != null)
            {
                query += " AND d.departId IN (" + departments.TrimEnd(',') + ")";
            }

            var result = await connection.QueryAsync<User, Department, User>(query, (user, department) =>
            {
                user.Department = department;
                return user;
            }, 
            new { IsActive = active }, 
            splitOn: "Name").ConfigureAwait(false);
            
            return result;
        }
        public async Task AddUser(User user, string userLogin)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = @"MERGE [app].[Users] AS target
                            USING(SELECT @UserId, @Roles, @ValidFrom, @ValidTo, @userLogin) AS source (UserId, Roles, ValidFrom, ValidTo, userLogin)
                            ON target.UserId = source.UserId
                            WHEN MATCHED THEN UPDATE SET IsActive = 1, ModifiedBy = @userLogin, ModificationDate = GETDATE()
                            WHEN NOT MATCHED THEN
                            INSERT(UserId, Roles, ValidFrom, ValidTo, IsActive, CreatedBy, CreationDate, ModifiedBy, ModificationDate)
                            VALUES(source.UserId, source.Roles, source.ValidFrom, source.ValidTo, 1, userLogin, GETDATE(), userLogin, GETDATE());";


            await connection.ExecuteAsync(query, new
            {
                UserId = user.Id,
                user.Roles,
                user.ValidFrom,
                user.ValidTo,
                userLogin
            }).ConfigureAwait(false);

            _logger.Info($"Add user Success: {user}. Creator: '{userLogin}'");
        }
        public async Task RemoveUser(long userId, string userLogin)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = @"UPDATE app.Users 
                          SET
                            IsActive = 0,
                            ModifiedBy = @userLogin,
                            ModificationDate = GETDATE()
                         WHERE UserId = @UserId";

            await connection.ExecuteAsync(query, new
            {
                UserId = userId,
                userLogin
            }).ConfigureAwait(false);
        }
        public async Task ModifyUser(User user, string userLogin)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = @"UPDATE app.Users 
                        SET
                        Roles = @Roles,
                        ValidFrom = @ValidFrom,
                        ValidTo = @ValidTo,
                        ModifiedBy = @userLogin,
                        ModificationDate = GETDATE()
                        WHERE UserId = @UserId";

            await connection.ExecuteAsync(query, new
            {
                UserId = user.Id,
                user.Roles,
                user.ValidFrom,
                user.ValidTo,
                userLogin
            }).ConfigureAwait(false);


            _logger.Info($"Modify user Success: {user}. Modified by: '{userLogin}'");
        }
        public async Task<Option<User>> GetUser(string login)
        {
            using var connection = _connectionFactory.CreateConnection();
            // Here queries are modified
            var query = @"SELECT * FROM app.Users AS u
                               WHERE GETDATE() Between appUser.ValidFrom AND appUser.ValidTo) 
                               AND u.userLogin = @login";

            var users = await connection.QueryAsync<User, Department, User>(query, (user, department) =>
            {
                user.Department = department;
                return user;
            }, 
            new { login }, splitOn: "Name").ConfigureAwait(false);

            if (users.Any())
            {
                return new Option<User>(users);
            }

            return Option<User>.None;
        }
    }
}
