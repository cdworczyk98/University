using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessTier;
using BusinessLogicTier.Models;

namespace BusinessLogicTier.Processors
{
    public class LoginProcessor
    {
        public static List<LoginModel> LoadUser(string username, string password)
        {
            string sql = @"select Username, Password from dbo.tblLogin where Username ='"+username+"' AND Password ='"+password+"';";

            return SqlDataAccess.LoadData<LoginModel>(sql);
        }
    }
}
