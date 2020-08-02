namespace Compentio.Ankara.Services
{
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<IEnumerable<dynamic>> GetRoles();
        Task<IEnumerable<User>> GetUsers(IEnumerable<long> departmentIds, bool? active = false);
        Task AddUser(User user, string userLogin);
        Task RemoveUser(long userId, string userLogin);
        Task ModifyUser(User user, string userLogin);
    }
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICurrentUserContext _userContext;

        public UsersService(IUsersRepository usersRepository, ICurrentUserContext userContext)
        {
            _usersRepository = usersRepository;
            _userContext = userContext;
        }
        public async Task<IEnumerable<User>> GetUsers(IEnumerable<long> departmentIds, bool? active = false)
        {
             return await _usersRepository.GetUsers(departmentIds, active).ConfigureAwait(false);
        }
        public async Task AddUser(User user, string userLogin)
        {
            await _usersRepository.AddUser(user, userLogin).ConfigureAwait(false);
        }
        public async Task RemoveUser(long userId, string userLogin)
        {
            await _usersRepository.RemoveUser(userId, userLogin).ConfigureAwait(false);
        }
        public async Task ModifyUser(User user, string userLogin)
        {
            await _usersRepository.ModifyUser(user, userLogin).ConfigureAwait(false);
        }
        public async Task<IEnumerable<dynamic>> GetRoles()
        {
            var roles = new List<dynamic>();
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                roles.Add(new
                { 
                    Id = (int)Enum.Parse<Roles>(item.ToString()), 
                    Name = item.ToString() 
                });
            }

            return await Task.FromResult(roles.ToArray()).ConfigureAwait(false);
        }
       
    }
}
