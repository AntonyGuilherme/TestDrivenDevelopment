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
            //suite.Add(new TestCaseTest("Should_Run_A_Test"));
            //suite.Add(new TestCaseTest("Should_Execute_The_Test_SetUp"));
            //suite.Add(new TestCaseTest("Should_Execute_The_Tear_Down"));
            //suite.Add(new TestCaseTest("Should_Execute_The_Tear_Down_Even_If_Test_Fails"));
            //suite.Add(new TestCaseTest("Should_Return_The_Tests_Result"));
            //suite.Add(new TestCaseTest("Should_Return_The_Tests_Result_With_Failure_Tests"));
            //suite.Add(new TestCaseTest("Should_Execute_MultipleTests"));
            

            Console.WriteLine(new TestCaseTest().RunAll().Summary);

            Console.ReadLine();
        }
    }


    class TestCaseTest: TestCase
    {
        [XTest]
        public void Should_Run_A_Test()
        {
            var testSetUp = new WasRun();
            XAssert.AreEqual(null, testSetUp.Log);
            testSetUp.RunOnly("TestMethod", new TestResult());
            XAssert.AreEqual("SetUp TestMethod TearDown", testSetUp.Log);
        }

        [XTest]
        public void Should_Execute_The_Test_SetUp()
        {
            var testRunTest = new WasRun();
            XAssert.AreEqual(null, testRunTest.Log);
            testRunTest.RunOnly("TestMethod", new TestResult());
            XAssert.AreEqual("SetUp TestMethod TearDown", testRunTest.Log);
        }

        [XTest]
        public void Should_Execute_The_Tear_Down() 
        {
            var testTearDown = new WasRun();
            XAssert.AreEqual(null, testTearDown.Log);
            testTearDown.RunOnly("TestMethod", new TestResult());
            XAssert.AreEqual("SetUp TestMethod TearDown", testTearDown.Log);
        }

        [XTest]
        public void Should_Execute_The_Tear_Down_Even_If_Test_Fails()
        {
            var testTearDown = new WasRun();
            XAssert.AreEqual(null, testTearDown.Log);
            testTearDown.RunAll();
            XAssert.AreEqual("SetUp TestMethodBroken TearDown", testTearDown.Log);
        }

        [XTest]
        public void Should_Return_The_Tests_Result()
        {
            var testTheTestResult = new WasRun();
            XAssert.AreEqual("1 Run, 0 Failed", testTheTestResult.RunOnly("TestMethod", new TestResult()).Summary);
        }

        [XTest]
        public void Should_Return_The_Tests_Result_With_Failure_Tests()
        {
            var testTheTestResult = new WasRun();
            XAssert.AreEqual("1 Run, 1 Failed", testTheTestResult.RunOnly("TestMethodBroken", new TestResult()).Summary);
        }

        [XTest]
        public void Should_Not_Execute_Any_Test_Without_Attribute_XTest()
        {
            TestWihtAttributeXTest test = new TestWihtAttributeXTest();
            TestResult result = test.RunAll();

            XAssert.AreEqual("1 Run, 0 Failed", result.Summary);
        }

        [XTest]
        public void Should_Execute_All_Test_With_Attribute_XTest()
        {
            TestWihtAttributeXTest test = new TestWihtAttributeXTest();
            TestResult result = test.RunAll();

            XAssert.AreEqual("1 Run, 0 Failed", result.Summary);
        }
    }

    class TestWihtAttributeXTest : TestCase
    {

        public override bool ShutUp => true;

        public void TestWihoutXTestAttrbiute()
        {

        }

        [XTest]
        public void TestWihtXTestAttrbiute()
        {

        }
    }

    class WasRun : TestCase
    {
        public override bool ShutUp => true;

        public string Log { get; internal set; }

        protected override void SetUp()
        {
            Log = nameof(SetUp);
        }

        [XTest]
        public void TestMethod()
        {
            Log = string.Format("{0} {1}", Log, nameof(TestMethod));
        }

        [XTest]
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
        public virtual bool ShutUp { get; protected set; } = false; 

        protected virtual void SetUp() { }

        public TestResult RunOnly(string methodName, TestResult result)
        {
            SetUp();
            Type wasRunType = GetType();
            MethodInfo testMethod = wasRunType.GetMethod(methodName);

            try
            {
                testMethod.Invoke(this, null);
                if (!ShutUp)
                    Console.WriteLine("{0} - passed", methodName);
            }
            catch (Exception)
            {
                result.TestFailed();
                if (!ShutUp)
                    Console.WriteLine("{0} - failed", methodName);
            }
            finally
            {
                result.TestRan();
            }

            TearDown();

            return result;
        }

        public TestResult RunAll()
        {
            TestResult result = new TestResult();
            Type classType = GetType();

            foreach (MethodInfo method in classType.GetMethods())
            {
                if (method.GetCustomAttribute(typeof(XTest)) != null)
                {
                    RunOnly(method.Name, result);
                }
            }

            return result;
        }

        protected virtual void TearDown() { }
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
