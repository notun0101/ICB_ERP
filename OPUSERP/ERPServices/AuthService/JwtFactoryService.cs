﻿using Microsoft.Extensions.Options;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.ERPServices.AuthService.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.AuthService
{
    public class JwtFactoryService : IJwtFactoryService
    {
        public readonly JwtIssuerOptions _jwtIssuerOption;

        public JwtFactoryService(IOptions<JwtIssuerOptions> jwtIssuerOption)
        {
            _jwtIssuerOption = jwtIssuerOption.Value;
        }

        public async Task<string> GenerateToken(string userName, string id, IList<string> roles)
        {
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOption.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtIssuerOption.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim("id", id),
                new Claim("rol",roles[0])
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtIssuerOption.Issuer,
                audience: _jwtIssuerOption.Audience,
                claims: Claims,
                notBefore: _jwtIssuerOption.NotBefore,
                expires: _jwtIssuerOption.Expiration,
                signingCredentials: _jwtIssuerOption.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}
