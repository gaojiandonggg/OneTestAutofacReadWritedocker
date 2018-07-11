using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public class ReadSqlServerContext : TeacherClubContext
    {
        public ReadSqlServerContext(DbContextOptions<ReadSqlServerContext> options) : base(options)
        {

        }
    }
}
