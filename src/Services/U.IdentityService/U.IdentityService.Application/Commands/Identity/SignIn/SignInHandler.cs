using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using U.Common.NetCore.Auth.Models;
using U.Common.NetCore.Auth.Service;
using U.IdentityService.Application.Services;
using U.IdentityService.Domain;
using U.IdentityService.Domain.Exceptions;
using U.IdentityService.Domain.Models;
using U.IdentityService.Persistance.Repositories;

namespace U.IdentityService.Application.Commands.Identity.SignIn
{
    public class SignInHandler : IRequestHandler<SignIn, JsonWebToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IClaimsProvider _claimsProvider;

        public SignInHandler(IClaimsProvider claimsProvider,
            IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService,
            IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            _claimsProvider = claimsProvider;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<JsonWebToken> Handle(SignIn request, CancellationToken cancellationToken)
        {
            var email = request.Email;
            var password = request.Password;

            var user = await _userRepository.GetAsync(email);
            if (user == null || !user.ValidatePassword(password, _passwordHasher))
            {
                throw new IdentityException(Codes.InvalidCredentials,
                    "Invalid credentials.");
            }

            var refreshToken = new RefreshToken(user, _passwordHasher);
            var claims = await _claimsProvider.GetAsync(user.Id);

            var jwt = _jwtService.CreateToken(user.Id.ToString("N"), user.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            await _refreshTokenRepository.AddAndSaveAsync(refreshToken);

            return jwt;
        }
    }
}