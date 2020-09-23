using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.Dto;
using Evento.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repo, IJwtHandler jwtHandler, IMapper mapper)
        {
            _userRepository = repo;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }
        public async Task RegisterAsync(Guid userId, string email, string name, string password, string role = "user")
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
            {
                throw new Exception("User with this email already exist");
            }
            user = new User(userId, role, name, email, password);
            await _userRepository.AddAsync(user);
        }
        public async Task<TokenDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null || user.Password != password)
            {
                throw new Exception("Invalid credentials");
            }
            var jwt = _jwtHandler.CreateTokem(user.Id, user.Role);
            return new TokenDto
            {
                Token = jwt.Token,
                Expires = jwt.Expires,
                Role = user.Role
            };
        }

        public async Task<AcountDto> GetAcountAsync(Guid userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            return _mapper.Map<AcountDto>(user); 
        }
    }
}
