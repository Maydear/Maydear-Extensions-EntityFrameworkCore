using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maydear.Extensions.EntityFrameworkCore.Test
{
    public class TestRepository : BaseRepository<Test>
    {
        public TestRepository(TestContext testContext, ILoggerFactory loggerFactory) : base(testContext, loggerFactory.CreateLogger<TestRepository>())
        {

        }

        protected new TestContext Context => (TestContext)base.Context;

        protected override DbSet<Test> DbSetEntities => Context.Tests;
    }
}
