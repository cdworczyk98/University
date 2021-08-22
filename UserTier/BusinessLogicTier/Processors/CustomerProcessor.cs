using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessTier;
using BusinessLogicTier.Models;

namespace BusinessLogicTier.Processors
{
    public class CustomerProcessor
    {
        public static int Create(int customerId, string firstName, string lastName, string email, string phoneNo, string address, int loyaltyCard)
        {
            CustomerModel data = new CustomerModel
            {
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNo = phoneNo,
                Address = address,
                LoyaltyCard = loyaltyCard

            };

            string sql = @"insert into dbo.tblCustomer (CustomerId, FirstName, LastName, Email, PhoneNo, Address, LoyaltyCard)
                            values (@CustomerId, @FirstName, @LastName, @Email, @PhoneNo, @Address, @LoyaltyCard);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<CustomerModel> Load(int id = -1)
        {
            string sql;

            if (id == -1)
                sql = @"select Id, CustomerId, FirstName, LastName, Email, PhoneNo, Address, LoyaltyCard from dbo.tblCustomer;";
            else
                sql = sql = @"select Id, CustomerId, FirstName, LastName, Email, PhoneNo, Address, LoyaltyCard from dbo.tblCustomer where CustomerId=" + id + ";";

            return SqlDataAccess.LoadData<CustomerModel>(sql);
        }

        public static int Edit(int customerId, string firstName, string lastName, string email, string phoneNo, string address, int loyaltyCard)
        {
            CustomerModel data = new CustomerModel
            {
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNo = phoneNo,
                Address = address,
                LoyaltyCard = loyaltyCard

            };

            string sql = @"UPDATE dbo.tblCustomer SET CustomerId=@CustomerId, FirstName=@FirstName, LastName=@LastName, Email=@Email, PhoneNo=@PhoneNo, Address=@Address, LoyaltyCard=@LoyaltyCard WHERE CustomerId=@CustomerId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int Delete(int id)
        {
            string sql = @"DELETE FROM dbo.tblCustomer WHERE CustomerId=" + id + ";";

            return SqlDataAccess.DeleteData(sql);
        }
    }
}
