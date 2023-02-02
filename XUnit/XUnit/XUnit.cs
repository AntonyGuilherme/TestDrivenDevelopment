using System;
using System.Reflection;

namespace XUnit
{
    public class XUnit
    {
        public static object wasRun { get; private set; }

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
            var test = new WasRun("TestMethod");
            XAssert.IsThruty(!test.TestMethodWasRun);
            test.Run();
            XAssert.IsThruty(test.TestMethodWasRun);
            Console.WriteLine("Test Passed");
            Console.ReadLine();
        }
    }

    class WasRun : TestCase<WasRun>
    {
        public WasRun(string testMethodName) : base(testMethodName) { }

        public bool TestMethodWasRun { get; internal set; }


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

        public void Run()
        {
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
            if(value is false) 
                throw new ArgumentException(String.Format("Expected thruty but recieve {0}", value));
        }
    }
}
