using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;

namespace Maydear.Extensions.EntityFrameworkCore.Test
{
    public class TestRepositoryUnitTest
    {
        private TestRepository repository;

        [SetUp]
        public void Setup()
        {
            repository = ServiceCollectionFactory.GetRequiredService<TestRepository>();
            repository.Add(new Test()
            {
                Id = 100,
                Name = "name100"
            });
            List<Test> tests = new List<Test>() {
                new Test() {
                Id = 1,
                Name = "name1"
                },
                new Test() {
                Id = 2,
                Name = "name2"
                },
                new Test() {
                Id = 3,
                Name = "name3"
                },
                new Test() {
                Id = 4,
                Name = "name4"
                },
            };
            repository.AddRange(tests);

            System.Console.WriteLine("SetUp");
        }

        [Test]
        public void AddTest()
        {
            bool result = repository.Add(new Test()
            {
                Id = 101,
                Name = "name101"
            });
            System.Console.WriteLine(result.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsIsTrueTest()
        {
            bool result = repository.Exists(a => a.Id == 1);
            System.Console.WriteLine(result.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsIsFalseTest()
        {
            bool result = repository.Exists(a => a.Id == 200);
            System.Console.WriteLine(result.ToString());

            Assert.IsFalse(result);
        }
    }
}