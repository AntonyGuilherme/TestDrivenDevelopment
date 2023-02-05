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
            TestCaseTest.Should_Run_A_Test();
            TestCaseTest.Should_Execute_The_Test_SetUp();
            TestCaseTest.Should_Execute_The_Tear_Down();
            TestCaseTest.Should_Execute_The_Tear_Down_Even_If_Test_Fails();
            TestCaseTest.Should_Return_The_Tests_Result();
            TestCaseTest.Should_Return_The_Tests_Result_With_Failure_Tests();
            

            Console.ReadKey();
        }
    }


    class TestCaseTest
    {
        public static void Should_Run_A_Test()
        {
            var testSetUp = new WasRun("TestMethod");
            XAssert.AreEqual(null, testSetUp.Log);
            testSetUp.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testSetUp.Log);
        }

        public static void Should_Execute_The_Test_SetUp()
        {
            var testRunTest = new WasRun("TestMethod");
            XAssert.AreEqual(null, testRunTest.Log);
            testRunTest.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testRunTest.Log);
        }

        public static void Should_Execute_The_Tear_Down() 
        {
            var testTearDown = new WasRun("TestMethod");
            XAssert.AreEqual(null, testTearDown.Log);
            testTearDown.Run();
            XAssert.AreEqual("SetUp TestMethod TearDown", testTearDown.Log);
        }

        public static void Should_Execute_The_Tear_Down_Even_If_Test_Fails()
        {
            var testTearDown = new WasRun("TestMethodBroken");
            XAssert.AreEqual(null, testTearDown.Log);
            testTearDown.Run();
            XAssert.AreEqual("SetUp TestMethodBroken TearDown", testTearDown.Log);
        }

        public static void Should_Return_The_Tests_Result()
        {
            var testTheTestResult = new WasRun("TestMethod");
            XAssert.AreEqual("1 Run, 0 Failed", testTheTestResult.Run().Summary);
        }

        public static void Should_Return_The_Tests_Result_With_Failure_Tests()
        {
            var testTheTestResult = new WasRun("TestMethodBroken");
            XAssert.AreEqual("1 Run, 1 Failed", testTheTestResult.Run().Summary);
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

    abstract class TestCase<ClassThatImplementsTestCase>
    {
        public TestCase(string testMethodName)
        {
            TestMethodName = testMethodName;
        }

        protected virtual void SetUp() { }

        public TestResult Run()
        {
            var result = new TestResult();
            
            SetUp();

            Type wasRunType = typeof(ClassThatImplementsTestCase);
            MethodInfo testMethod = wasRunType.GetMethod(TestMethodName);

            try
            {
                testMethod.Invoke(this, null);
                Console.WriteLine("{0} - passed", TestMethodName);
            }
            catch (Exception)
            {
                result.TestFailed();
                Console.WriteLine("{0} - failed", TestMethodName);
            }
            finally 
            { 
                result.TestRan(); 
            }
            
            TearDown();
            
            return result;
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
