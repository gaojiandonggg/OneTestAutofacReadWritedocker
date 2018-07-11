using GaoJD.Club.BusinessEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.Core
{
    public class TeacherClubContext : DbContext
    {


        public TeacherClubContext(DbContextOptions options)
           : base(options)
        {

        }

        public DbSet<User> User { get; set; }

    }

    public enum WriteAndRead
    {
        Write,
        Read
    }
}
