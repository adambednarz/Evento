﻿using Evento.Infrastructure.Dto;
using Evento.Infrastructure.Extensions;
using Evento.Infrastructure.Services.Interfaces;
using Evento.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Evento.Infrastructure.Services
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings _jwtSettings;
        public JwtHandler(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public JwtDto CreateToken(Guid userId, string role)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),      // => unikalny identyfikator tokena
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),                            // => data wydania tokena
            };

            var expires = now.AddMinutes(_jwtSettings.ExpiryMinutes);

            //============== Konfiguracja ===========
            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: _jwtSettings.SigningCredentials
                );

            //============== Generowanie na bazie konfiguracji ===========
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtDto
            {
                Token = token,
                Expires = expires.ToTimestamp()
            };
        }
    }
}
