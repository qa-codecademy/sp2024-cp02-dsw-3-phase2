using ArtShop.DataAcces;
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
using ArtShop.Mappers.ImageMapper;

namespace ArtShop.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ArtShopDbContext _dbContext;
        public UserService(ArtShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LoginUserResultDto Login(LoginUserDto loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.UserName))
            {
                return new LoginUserResultDto { Success = false, Message = "Username is a required field" };
            }

            if (string.IsNullOrEmpty(loginUser.Password))
            {
                return new LoginUserResultDto { Success = false, Message = "Password is a required field" };
            }

            using (MD5CryptoServiceProvider md5CryptoService = new())
            {
                byte[] password = Encoding.ASCII.GetBytes(loginUser.Password);
                byte[] hashedPassword = md5CryptoService.ComputeHash(password);
                string passwordDb = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

                var user = _dbContext.Users.FirstOrDefault(x =>
                    (x.UserName == loginUser.UserName) && x.Password == passwordDb);

                if (user == null)
                {
                    return new LoginUserResultDto { Success = false, Message = "Username or Password is incorrect" };
                }

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] secretKey = Encoding.ASCII.GetBytes("Our very secret key is at least 32 bytes long");

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.UserData, loginUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim("userNameFullName", $"{user.FirstName} {user.LastName}")
                     })
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);

                return new LoginUserResultDto
                {
                    Success = true,
                    Token = tokenString,
                    UserFullName = $"{user.FirstName} {user.LastName}"
                };
            }
        }

        public RegisterUserResultDto Register(RegisterUserDto registerUser)
        {
            var validObject = ValidateRegister(registerUser);

            if (!validObject.IsValid)
            {
                return new RegisterUserResultDto
                {
                    Success = false,
                    Message = validObject.ValidationMessage
                };
            }

            using (MD5CryptoServiceProvider md5CryptoService = new())
            {
                byte[] password = Encoding.ASCII.GetBytes(registerUser.Password);
                byte[] hashedPassword = md5CryptoService.ComputeHash(password);
                string passwordDb = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

                var user = registerUser.ToUser();
                user.Password = passwordDb;

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }

            return new RegisterUserResultDto
            {
                Success = true,
                Message = "User added successfully"
            };
        }

        public UpdateUserResultDto Update(Guid id, UpdateUserDto updateUser)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return new UpdateUserResultDto
                {
                    Success = false,
                    Message = "User does not exist"
                };
            }

            if (!string.IsNullOrEmpty(updateUser.Email))
            {
                user.Email = updateUser.Email;
            }

            if (!string.IsNullOrEmpty(updateUser.FirstName))
            {
                user.FirstName = updateUser.FirstName;
            }

            if (!string.IsNullOrEmpty(updateUser.LastName))
            {
                user.LastName = updateUser.LastName;
            }

            if (!string.IsNullOrEmpty(updateUser.UserName))
            {
                user.UserName = updateUser.UserName;
            }

            if (!string.IsNullOrEmpty(updateUser.Password))
            {
                if (updateUser.Password.Length < 8)
                {
                    return new UpdateUserResultDto
                    {
                        Success = false,
                        Message = "Password needs to be at least 8 characters long"
                    };
                }

                using (MD5CryptoServiceProvider md5CryptoService = new())
                {
                    byte[] password = Encoding.ASCII.GetBytes(updateUser.Password);
                    byte[] hashedPassword = md5CryptoService.ComputeHash(password);
                    user.Password = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();
                }
            }

            if (!string.IsNullOrEmpty(updateUser.CardNo))
            {
                user.CardNo = updateUser.CardNo;
            }

            if (!string.IsNullOrEmpty(updateUser.ExpireDate))
            {
                user.ExpireDate = updateUser.ExpireDate;
            }

            _dbContext.SaveChanges();

            return new UpdateUserResultDto
            {
                Success = true,
                Message = "User updated successfully"
            };
        }

        public UserInfoDto UserInfo(Guid id)
        {
            if(id == null)
            {
                throw new Exception("User doesent exist");
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            var boughtImages = _dbContext.Images
                                         .Where(x => x.BoughtByUserId == id)
                                         .Select(image => ImageMapper.ToUserInfoImages(image))
                                         .ToList();

            var soldImages = _dbContext.Images
                                        .Where(x => x.SoldByUserId == id)
                                        .Select(image => ImageMapper.ToUserInfoImages(image))
                                        .ToList();

            if (user == null)
            {
                throw new Exception("User Not Found");
            }

            return new UserInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                CardNo = user.CardNo,
                ExpireDate = user.ExpireDate,
                BoughtImages = boughtImages,
                SoldImages = soldImages
            };
        }

        private ValidationObject ValidateRegister(RegisterUserDto userDto)
        {
            if (
                  (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.UserName))
                  || (string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.CardNo))
                  || (string.IsNullOrEmpty(userDto.ExpireDate))
                )
            {
                return new ValidationObject()
                {
                    IsValid = false,
                    ValidationMessage = "Email,UserName, Password,CardNumber and ExpireDate are required fields"
                };
            }
            if (userDto.Email.Length > 150 ||
                userDto.FirstName.Length > 150 ||
                userDto.LastName.Length > 150 ||
                userDto.UserName.Length > 50 ||
                userDto.CardNo.Length > 16
                )
            {
                return new ValidationObject()
                {
                    IsValid = false,
                    ValidationMessage = "Maximum lenght for Email/FirstName/LastName and CardNumber is 150 characters"
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
