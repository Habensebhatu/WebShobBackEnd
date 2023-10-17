using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;



namespace business_logic_layer
{
    public class UserRegistrationBLL
    {
        private readonly UserRegistrationDAL _UserRegistrationDAL;
        private readonly IConfiguration _configuration;
        public UserRegistrationBLL(IConfiguration configuration)
        {
            _UserRegistrationDAL = new UserRegistrationDAL();
            _configuration = configuration;

        }

        public async Task<string> RegisterUser(UserRegistrationModel userViewModel)
        {
            string password = userViewModel.Password;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var userEntity = new UserRegistrationEntityModel
            {
                UserId = Guid.NewGuid(),
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Password = hashedPassword,
                Email = userViewModel.Email,
                Gender = (Data_layer.Context.Data.Gender)userViewModel.Gender,
                PhoneNumber = userViewModel.PhoneNumber,


                Address = new Address
                {
                    Street = userViewModel.Address.Street,
                    Number = userViewModel.Address.Number,
                    Residence = userViewModel.Address.Residence,
                    ZipCode = userViewModel.Address.ZipCode
                }
            };

            await _UserRegistrationDAL.AddUser(userEntity);
            string token = CreateToken(userEntity);

            return token;



        }

        private string CreateToken(UserRegistrationEntityModel userEntity)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSetting = _configuration?.GetSection("AppSettings:Token");
            var key = Encoding.UTF8.GetBytes(tokenSetting.Value); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()),
                new Claim(ClaimTypes.Email, userEntity.Email),
                new Claim("firstName", userEntity.FirstName),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<string> LoginUser(Login loginModel)
        {
            var userEntity = await _UserRegistrationDAL.GetUserByEmail(loginModel.Username); 

            if (userEntity == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, userEntity.Password);

            if (!isValidPassword)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSetting = _configuration?.GetSection("AppSettings:Token");
            var key = Encoding.UTF8.GetBytes(tokenSetting.Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()),
                new Claim(ClaimTypes.Email, userEntity.Email),
                new Claim("firstName", userEntity.FirstName),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<List<UserRegistrationModel>> GetAllUsers()
        {
            var users = await _UserRegistrationDAL.GetAllUsers();
            return users.Select(u => new UserRegistrationModel
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Gender = (business_logic_layer.ViewModel.Gender)u.Gender,
                PhoneNumber = u.PhoneNumber,
                Address = new Addres
                {
                    Street = u.Address.Street,
                    Number = u.Address.Number,
                    Residence = u.Address.Residence,
                    ZipCode = u.Address.ZipCode
                }
            }).ToList();
        }



    }
}

