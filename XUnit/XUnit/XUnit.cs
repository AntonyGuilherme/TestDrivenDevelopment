using System;
using System.Collections.Generic;
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
            var suite = new TestSuite();

            suite.Add(new TestCaseTest("Should_Run_A_Test"));
            suite.Add(new TestCaseTest("Should_Execute_The_Test_SetUp"));
            suite.Add(new TestCaseTest("Should_Execute_The_Tear_Down"));
            suite.Add(new TestCaseTest("Should_Execute_The_Tear_Down_Even_If_Test_Fails"));
            suite.Add(new TestCaseTest("Should_Return_The_Tests_Result"));
            suite.Add(new TestCaseTest("Should_Return_The_Tests_Result_With_Failure_Tests"));
            suite.Add(new TestCaseTest("Should_Execute_MultipleTests"));

            Console.WriteLine(suite.Run().Summary);

            Console.ReadLine();
        }
    }


    class TestCaseTest: TestCase
    {
        public TestCaseTest(string testMethodName) : base(testMethodName) { }

        public void Should_Run_A_Test()
        {
            var testSetUp = new WasRun("TestMethod");
            XAssert.AreEqual(null, testSetUp.Log);
            testSetUp.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testSetUp.Log);
        }

        public void Should_Execute_The_Test_SetUp()
        {
            var testRunTest = new WasRun("TestMethod");
            XAssert.AreEqual(null, testRunTest.Log);
            testRunTest.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testRunTest.Log);
        }

        public void Should_Execute_The_Tear_Down() 
        {
            var testTearDown = new WasRun("TestMethod");
            XAssert.AreEqual(null, testTearDown.Log);
            testTearDown.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testTearDown.Log);
        }

        public void Should_Execute_The_Tear_Down_Even_If_Test_Fails()
        {
            var testTearDown = new WasRun("TestMethodBroken");
            XAssert.AreEqual(null, testTearDown.Log);
            testTearDown.Run();
            XAssert.AreEqual("SetUp TestMethodBroken TearDown", testTearDown.Log);
        }

        public void Should_Return_The_Tests_Result()
        {
            var testTheTestResult = new WasRun("TestMethod");
            XAssert.AreEqual("1 Run, 0 Failed", testTheTestResult.Run().Summary);
        }

        public void Should_Return_The_Tests_Result_With_Failure_Tests()
        {
            var testTheTestResult = new WasRun("TestMethodBroken");
            XAssert.AreEqual("1 Run, 1 Failed", testTheTestResult.Run().Summary);
        }

        public void Should_Execute_MultipleTests() 
        {
            var suite = new TestSuite();
            suite.Add(new WasRun("TestMethod"));
            suite.Add(new WasRun("TestMethodBroken"));
            var result = suite.Run();
            XAssert.AreEqual("2 Run, 1 Failed", result.Summary);
        }
    }

    class TestSuite
    {
        private readonly List<TestCase> _tests = new List<TestCase>();

        public void Add(TestCase wasRun)
        {
            _tests.Add(wasRun);
        }

        public TestResult Run()
        {
            var testResult = new TestResult();

            foreach (var test in _tests)
                test.Run(testResult);

            return testResult;
        }
    }

    class WasRun : TestCase
    {
        public WasRun(string testMethodName) : base(testMethodName) { }

        public override bool ShutUp => true;

        public string Log { get; internal set; }

        protected override void SetUp() 
        {
            Log = nameof(SetUp);
        }

        public void TestMethod()
        {
            Log = string.Format("{0} {1}", Log, nameof(TestMethod));
        }

        public void TestMethodBroken()
        {
            Log = string.Format("{0} {1}", Log, nameof(TestMethodBroken));

            throw new NotSupportedException();
        }

        protected override void TearDown()
        {
            Log = string.Format("{0} {1}", Log, nameof(TearDown));
        }
    }

    abstract class TestCase
    {
        public TestCase(string testMethodName)
        {
            TestMethodName = testMethodName;
        }

        public virtual bool ShutUp { get; protected set; } = false; 

        protected virtual void SetUp() { }

        public TestResult Run(TestResult result)
        {            
            SetUp();
            Type wasRunType = GetType();
            MethodInfo testMethod = wasRunType.GetMethod(TestMethodName);

            try
            {
                testMethod.Invoke(this, null);
                if(!ShutUp)
                    Console.WriteLine("{0} - passed", TestMethodName);
            }
            catch (Exception)
            {
                result.TestFailed();
                if (!ShutUp)
                    Console.WriteLine("{0} - failed", TestMethodName);
            }
            finally 
            { 
                result.TestRan(); 
            }
            
            TearDown();
            
            return result;
        }

        public TestResult Run()
        {
            return Run(new TestResult());
        }

        protected virtual void TearDown() { }

        protected string TestMethodName { get; }
    }

    public class TestResult
    {
        private int NumberOftestsThatWasRan { get; set; }
        private int NumberOfTestsThatFailed { get; set; }
        public string Summary => $"{NumberOftestsThatWasRan} Run, {NumberOfTestsThatFailed} Failed";

        public void TestRan() 
        {
            NumberOftestsThatWasRan++;
        }

        public void TestFailed()
        {
            NumberOfTestsThatFailed++;
        }
    }

    public static class XAssert 
    {
        public static void IsThruty(bool value) 
        {
            if (!value) 
                throw new ArgumentException(string.Format("Expected thruty but recieve {0}", value));
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
