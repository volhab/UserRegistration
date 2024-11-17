using dotnet_registration_api.Data.Entities;
using dotnet_registration_api.Data.Models;
using dotnet_registration_api.Data.Repositories;
using dotnet_registration_api.Helpers;
using Mapster;

namespace dotnet_registration_api.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<User>> GetAll()
        {
           return await _userRepository.GetAllUsers();
        }
        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<User> GetByUserName(string name)
        {
            return await _userRepository.GetUserByUsername(name);
        }

        public async Task<User> Login(LoginRequest login)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(User user)
        {
            var u = await _userRepository.CreateUser(user);
            return u;
        }

        public async Task<User> Update(User user)
        {
            await _userRepository.UpdateUser(user);
            return user;
        }
        public async Task<bool> Delete(int id)
        {
            var u = await _userRepository.GetUserById(id);
            if (u == null)
                return false;
            await _userRepository.DeleteUser(id);
            return true;
        }

    }
}
