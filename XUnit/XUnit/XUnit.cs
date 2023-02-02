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
            Console.WriteLine(test.TestMethodWasRun);
            test.Run();
            Console.WriteLine(test.TestMethodWasRun);
            Console.ReadLine();
        }
    }

    class WasRun
    {
        public WasRun(string testMethodName)
        {
            TestMethodName = testMethodName;
        }

        public string TestMethodName { get; }
        public bool TestMethodWasRun { get; internal set; }

        internal void Run()
        {
            Type wasRunType = typeof(WasRun);
            MethodInfo testMethod = wasRunType.GetMethod(TestMethodName);
            testMethod.Invoke(this, null);
        }

        public void TestMethod()
        {
            TestMethodWasRun= true;
        }
    }
}
