using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.Extensions.Configuration;


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
            //Console.WriteLine($"userId: {userEntity.UserId}");
            //List<Claim> claims = new List<Claim> {
            //   new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()),
            //    new Claim(ClaimTypes.Email, userEntity.Email),
            //    new Claim("firstName",  userEntity.UserId.ToString()),
            //};



            //var tokenSetting = _configuration?.GetSection("AppSettings:Token");
            //if (tokenSetting == null || string.IsNullOrEmpty(tokenSetting.Value))
            //{
            //    throw new Exception("Token configuration is missing or empty.");
            //}

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSetting.Value));


            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //var token = new JwtSecurityToken(
            //        claims: claims,
            //        expires: DateTime.Now.AddDays(1),
            //        signingCredentials: creds
            //    );

            //var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            //return jwt;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSetting = _configuration?.GetSection("AppSettings:Token");
            var key = Encoding.UTF8.GetBytes(tokenSetting.Value); // Move this to a configuration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()),
                new Claim(ClaimTypes.Email, userEntity.Email),
                new Claim("firstName", userEntity.FirstName),
                    // Add more claims as needed
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
            var userEntity = await _UserRegistrationDAL.GetUserByEmail(loginModel.Username); // Assuming the 'Username' field in LoginModel is actually the user's email

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
                    // Add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }


    }
}

