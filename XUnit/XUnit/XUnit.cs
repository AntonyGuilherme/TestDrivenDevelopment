using System;

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
            var test = new WasRun("testMethod");
            Console.WriteLine(test.wasRun);
            test.TestMethod();
            Console.WriteLine(test.wasRun);
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
        public bool wasRun { get; internal set; }

        internal void TestMethod()
        {
            wasRun= true;
        }
    }
}
