using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Maydear.Extensions.EntityFrameworkCore.Test
{
    [TestFixture]
    public class TestRepositoryUnitTest
    {
        private readonly TestRepository repository;


        public TestRepositoryUnitTest()
        {
            repository = ServiceCollectionFactory.GetRequiredService<TestRepository>();
        }

        /// <summary>
        /// 首次执行的时候进行初始化
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
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

        /// <summary>
        /// 执行完清理信息
        /// </summary>
        [OneTimeTearDown]
        public void Cleanup()
        {
            repository.Remove(a => true);
            System.Console.WriteLine("CleanUp");
        }

        [TestCaseSource(typeof(TestDataClass), "TestAddData")]
        public bool AddTest(Test test)
        {
            try
            {
                bool result = repository.Add(test);

                System.Console.WriteLine(result.ToString());
                return result;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return false;
            }
        }

        [TestCaseSource(typeof(TestDataClass), "TestExistsData")]
        public bool ExistsTest(int id)
        {
            return repository.Exists(a => a.Id == id);
        }
    }

    public class TestDataClass
    {
        public static IEnumerable TestExistsData
        {
            get
            {
                yield return new TestCaseData(1).Returns(true);
                yield return new TestCaseData(2).Returns(true);
                yield return new TestCaseData(200).Returns(false);
            }
        }
        public static IEnumerable TestAddData
        {
            get
            {
                yield return new TestCaseData(new Test()
                {
                    Id = 100,
                    Name = "name100"
                }).Returns(true);
                yield return new TestCaseData(new Test()
                {
                    Id = 101,
                    Name = "name101"
                }).Returns(true);
            }
        }
    }
}