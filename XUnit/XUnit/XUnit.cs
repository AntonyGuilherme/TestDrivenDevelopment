using System;
using System.Reflection;

namespace XUnit
{
    public class XUnit
    { 
        /*
            # Invoque o método teste
            Invoque setUp primeiro
            Invoque tearDown depois
            Invoque tearDown mesmo se o método teste falhar
            Rode múltiplos testes
            Informe resultados coletados
        */

        public static void Main() 
        {
            new TestCaseTest().Run();

            var testSetUp = new WasRun("TestMethod");
            testSetUp.Run();
            XAssert.IsThruty(testSetUp.SetUpWasRan);
            Console.WriteLine("Test - Run SetUp Passed");
            Console.ReadLine();
        }
    }

    class TestCaseTest: TestCase<TestCaseTest> 
    {
        public TestCaseTest() : base(nameof(Should_Execute_A_Test))
        {
        }

        public void Should_Execute_A_Test() 
        {
            var testRunTest = new WasRun("TestMethod");
            XAssert.IsThruty(!testRunTest.TestMethodWasRun);
            testRunTest.Run();
            XAssert.IsThruty(testRunTest.TestMethodWasRun);
            Console.WriteLine("Test - Run Test Passed");
        }
    }

    class WasRun : TestCase<WasRun>
    {
        public WasRun(string testMethodName) : base(testMethodName) { }

        public bool TestMethodWasRun { get; internal set; }
        public bool SetUpWasRan { get; internal set; }

        public override void SetUp() 
        {
            SetUpWasRan = true;
        }

        public void TestMethod()
        {
            TestMethodWasRun= true;
        }
    }

    abstract class TestCase<ClassThatImplementsTestCase>
    {
        public TestCase(string testMethodName)
        {
            TestMethodName = testMethodName;
        }

        public virtual void SetUp() { }

        public void Run()
        {
            SetUp();

            Type wasRunType = typeof(ClassThatImplementsTestCase);
            MethodInfo testMethod = wasRunType.GetMethod(TestMethodName);
            testMethod.Invoke(this, null);
        }

        protected string TestMethodName { get; }
    }

    public static class XAssert 
    {
        public static void IsThruty(object value) 
        {
            if (value is false) 
                throw new ArgumentException(String.Format("Expected thruty but recieve {0}", value));
        }
    }
}
