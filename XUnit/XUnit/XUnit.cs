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
            new ShouldExexcuteTheSetUp().Run();
            new ShouldRunATest().Run();
            new ShouldExecuteTheTearDown().Run();
            
            Console.ReadLine();
        }
    }


    class ShouldExecuteTheTearDown: TestCase<ShouldExecuteTheTearDown> 
    {
        public ShouldExecuteTheTearDown(): base(nameof(Should_Execute_The_Tear_Down)) { }

        public void Should_Execute_The_Tear_Down() 
        {
            var testSetUp = new WasRun("TestMethod");
            XAssert.AreEqual(null, testSetUp.Log);
            testSetUp.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testSetUp.Log);
        }
    }

    class ShouldRunATest : TestCase<ShouldRunATest>
    {
        public ShouldRunATest() : base(nameof(Should_Run_A_Test))
        {
        }

        public void Should_Run_A_Test() 
        {
            var testSetUp = new WasRun("TestMethod");
            XAssert.AreEqual(null, testSetUp.Log);
            testSetUp.Run();
            XAssert.AreEqual("SetUp TestMethod", testSetUp.Log);
        }
    }

    class ShouldExexcuteTheSetUp: TestCase<ShouldExexcuteTheSetUp> 
    {
        public ShouldExexcuteTheSetUp() : base(nameof(Should_Execute_A_Test))
        {
        }

        public void Should_Execute_A_Test() 
        {
            var testRunTest = new WasRun("TestMethod");
            XAssert.AreEqual(null, testRunTest.Log);
            testRunTest.Run();
            XAssert.AreEqual("SetUp TestMethod", testRunTest.Log);
        }
    }

    class WasRun : TestCase<WasRun>
    {
        public WasRun(string testMethodName) : base(testMethodName) { }

        public string Log { get; internal set; }

        protected override void SetUp() 
        {
            Log = nameof(SetUp);
        }

        public void TestMethod()
        {
            Log = string.Format("{0} {1}", Log, nameof(TestMethod));
        }

        protected override void TearDown()
        {
            Log = string.Format("{0} {1}", Log, nameof(TearDown));
        }
    }

    abstract class TestCase<ClassThatImplementsTestCase>
    {
        public TestCase(string testMethodName)
        {
            TestMethodName = testMethodName;
        }

        protected virtual void SetUp() { }

        public void Run()
        {
            SetUp();

            Type wasRunType = typeof(ClassThatImplementsTestCase);
            MethodInfo testMethod = wasRunType.GetMethod(TestMethodName);
            testMethod.Invoke(this, null);

            TearDown();

            Console.WriteLine("{0} - passed", TestMethodName);
        }

        protected virtual void TearDown() { }

        protected string TestMethodName { get; }
    }

    public static class XAssert 
    {
        public static void IsThruty(bool value) 
        {
            if (!value) 
                throw new ArgumentException(String.Format("Expected thruty but recieve {0}", value));
        }

        public static void AreEqual(object expectedValue, object recivedValue) 
        {
            bool bothValueAreNull = expectedValue == null && recivedValue == null;
            bool expectValueIsNullButRecivedValueIsNotNull = expectedValue == null && recivedValue != null;

            if (expectValueIsNullButRecivedValueIsNotNull || !bothValueAreNull && !expectedValue.Equals(recivedValue))
                throw new ArgumentException(String.Format("Expected {0} but recieve {1}", expectedValue, recivedValue));
        }
    }
}
