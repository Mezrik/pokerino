using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Pokerino.Shared.Entities;
using Pokerino.Server.Helpers;
using Pokerino.Shared.Models.Users;

namespace Pokerino.Server.Services
{
    public interface IUserService
    {
        AuthResponse? Authenticate(AuthRequest model);
        IEnumerable<User> GetAll();
        User? GetById(int id);
        void Create(UserCreateRequest model);
        void Update(int id, UserUpdateRequest model);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        private readonly IJwtUtils _jwtUtils;

        public UserService(IJwtUtils jwtUtils, DataContext context, IMapper mapper)
        {
            _jwtUtils = jwtUtils;
            _context = context;
            _mapper = mapper;
        }

        public AuthResponse? Authenticate(AuthRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            if (user == null) return null;

            if (model.Password is null || !PasswordHasher.VerifyPassword(model.Password, user.Password, user.PasswordSalt))
                return null;

            var token = _jwtUtils.GenerateJwtToken(user);

            return new AuthResponse(user, token);
        }

        public void Create(UserCreateRequest model)
        {
            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            if (_context.Users.Any(x => x.Username == model.Username))
                throw new AppException("User with the username '" + model.Username + "' already exists");

            var user = _mapper.Map<User>(model);

            byte[] salt;
            user.Password = PasswordHasher.HashPassword(model.Password, out salt);
            user.PasswordSalt = salt;

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(int id, UserUpdateRequest model)
        {
            var user = GetUser(id);

            if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] salt;
                user.Password = PasswordHasher.HashPassword(model.Password, out salt);
                user.PasswordSalt = salt;
            }

            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return GetUser(id);
        }


        private User GetUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}

