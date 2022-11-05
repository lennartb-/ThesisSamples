using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = SampleAnalyzer.Test.CSharpCodeFixVerifier<SampleAnalyzer.Sha1Analyzer, SampleAnalyzer.Sha1AnalyzerCodeFixProvider>;

namespace SampleAnalyzer.Test
{
    [TestClass]
    public class Sha1AnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    class Testclass
    {   
        public static void Main()
        {
            var tc = new Testclass();        
            tc.HashAsSha1(""abc"");         
        }
         
        private string HashAsSha1(string t)
        {         
            return t;
        }

        private string HashAsSha512(string t)
        {         
            return t;
        }        
    }";

            var fixedtest = @"
    class Testclass
    {   
        public static void Main()
        {
            var tc = new Testclass();        
            tc.HashAsSha512(""abc"");         
        }
         
        private string HashAsSha1(string t)
        {         
            return t;
        }

        private string HashAsSha512(string t)
        {         
            return t;
        }        
    }";
            var expected = VerifyCS.Diagnostic().WithLocation(7, 13);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixedtest);
        }
    }
}
