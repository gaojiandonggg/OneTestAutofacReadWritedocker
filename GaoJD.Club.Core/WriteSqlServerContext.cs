using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public class WriteSqlServerContext : TeacherClubContext
    {
        public WriteSqlServerContext(DbContextOptions<WriteSqlServerContext> options) : base(options)
        {

        }
    }
}
