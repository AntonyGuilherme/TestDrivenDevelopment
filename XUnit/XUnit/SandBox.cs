using System;
using System.Reflection;

namespace XUnit
{
    public class SandBox
    {

        [XTest]
        public void Test()
        {

        }


        public static void Main42()
        {
            Type sandType = typeof(SandBox);
            
            foreach (var methodInfo in sandType.GetMethods()) 
            {
                Console.WriteLine(methodInfo.GetCustomAttribute(typeof(XTest)));
            }

            Console.ReadKey();
        }

    }


    public class XTest : Attribute
    {
    }
}
