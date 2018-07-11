using GaoJD.Club.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public class DbContextFactory
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public static DbContext GetContext(WriteAndRead type)
        {
            if (type == WriteAndRead.Read)
                //return new ReadSqlServerContext(new DbContextOptions<ReadSqlServerContext>());
                return HttpContext.Current.RequestServices.GetService(typeof(ReadSqlServerContext)) as ReadSqlServerContext;
            else
                //return new WriteSqlServerContext(new DbContextOptions<WriteSqlServerContext>());
                return HttpContext.Current.RequestServices.GetService(typeof(WriteSqlServerContext)) as WriteSqlServerContext;
        }
        /// <summary>
        /// 获取上下文操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="OpertionType"></param>
        /// <returns></returns>
        public static TEntity CallContext<TEntity>(WriteAndRead type) where TEntity : DbContext
        {
            var DbContext = GetContext(type);
            return (TEntity)DbContext;
        }
    }
}
