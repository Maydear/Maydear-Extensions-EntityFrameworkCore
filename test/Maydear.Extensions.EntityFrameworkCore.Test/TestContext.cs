using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maydear.Extensions.EntityFrameworkCore.Test
{
    public class TestContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }
        public DbSet<Test> Tests { get; set; }
    }
}
