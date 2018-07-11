using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;

namespace GaoJD.Club.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository()
        {

        }

        public User GetUser(string userName, string userPwd)
        {
            using (TeacherClubContext content = DbContextFactory.CallContext<ReadSqlServerContext>(WriteAndRead.Read))
            {
                return content.Set<User>().FirstOrDefault(it => it.UserName == userName && it.Password == userPwd);
            }
        }


    }
}
