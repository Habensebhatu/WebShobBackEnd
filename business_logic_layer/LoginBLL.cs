using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;

namespace business_logic_layer
{
	public class LoginBLL
	{
		private readonly LoginDAL _loginDAL;
		public LoginBLL()
		{
			_loginDAL = new LoginDAL();
		}

		public async Task<LoginModel> Authenticate(string username, string password)
		{
			LoginEnitiyModel loginEnitiy = await _loginDAL.GetUserByEmail(username);

			if (loginEnitiy != null && loginEnitiy.password == password)
			{
				LoginModel user = new LoginModel()
				{
					username = username,
					password = password
				};
				return user;
			}

			return null;
		}
	}
}

