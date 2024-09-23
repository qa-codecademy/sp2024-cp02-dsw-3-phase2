﻿using ArtShop.DataAcces;
using ArtShop.DTO.UserDTOs;
using ArtShop.Mappers.UserMapper;
using ArtShop.Services.Interfaces;
using ArtShop.Entities.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace ArtShop.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ArtShopDbContext _dbContext;
        public UserService(ArtShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string Login(LoginUserDto loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.UserName) || string.IsNullOrEmpty(loginUser.Password))
            {
                return "UserName and Password are required fields";
            }

            MD5CryptoServiceProvider md5CryptoService = new();

            byte[] password = Encoding.ASCII.GetBytes(loginUser.Password);

            byte[] hashedPassword = md5CryptoService.ComputeHash(password);

            string passwordDb = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

            var user = _dbContext.Users.FirstOrDefault(x => (x.UserName == loginUser.UserName
                                                               || x.Email == loginUser.UserName)
                                                               && x.Password == passwordDb
                                                               );
            if (user == null)
            {
                return "User or Password is incorrect";
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] secretKey = Encoding.ASCII.GetBytes("Our very secret key is at least 32 bytes long");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(15),

                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),

                Subject = new System.Security.Claims.ClaimsIdentity
                (
                    new[]
                    {
                        new Claim(ClaimTypes.Email,loginUser.UserName),
                        new Claim("userNameFullName", $"{user.FirstName} {user.LastName}")
                    }
                )
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public string Register(RegisterUserDto registerUser)
        {
            var validObject = ValidateRegister(registerUser);

            if (validObject.IsValid == false)
            {
                return validObject.ValidationMessage;
            }

            MD5CryptoServiceProvider md5CryptoService = new();

            byte[] password = Encoding.ASCII.GetBytes(registerUser.Password);

            byte[] hashedPassword = md5CryptoService.ComputeHash(password);

            string passwordDb = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

            var user = registerUser.ToUser();

            user.Password = passwordDb;

            _dbContext.Users.Add(user);

            _dbContext.SaveChanges();

            return ("User Added");
        }

        private ValidationObject ValidateRegister(RegisterUserDto userDto)
        {
            if (
                  (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.UserName))
                  || string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.CardNo)
                )
            {
                return new ValidationObject()
                {
                    IsValid = false,
                    ValidationMessage = "Email,UserName and Password are required fields"
                };
            }
            if (userDto.Email.Length > 150 ||
                userDto.FirstName.Length > 150 ||
                userDto.LastName.Length > 150 ||
                userDto.UserName.Length > 50 ||
                userDto.Password.Length > 50||
                userDto.CardNo.Length > 12
                )
            {
                return new ValidationObject()
                {
                    IsValid = false,
                    ValidationMessage = "Maximum lenght for Email/FirstName/LastName is 150 characters"
                };
            }
            if (userDto.Password.Length < 8)
            {
                return new ValidationObject()
                {
                    IsValid = false,
                    ValidationMessage = "Password neads to be at least 8 characters long"
                };
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == userDto.UserName ||
                                                       x.Email == userDto.Email);
            if (user is not null)
            {
                return new ValidationObject()
                {
                    IsValid = false,
                    ValidationMessage = $"User With username : {userDto.UserName} or email {userDto.Email} already exist in the database "
                };
            }

            return new ValidationObject()
            {
                IsValid = true
            };
        }
    }

    public class ValidationObject()
    {
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; }
    }
}