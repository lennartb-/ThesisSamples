using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = SampleAnalyzer.Test.CSharpCodeFixVerifier<
    SampleAnalyzer.NewlineAnalyzer,
    SampleAnalyzer.NewlineAnalyzerCodeFixProvider>;

namespace SampleAnalyzer.Test
{
    [TestClass]
    public class NewlineAnalyzerUnitTest
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class Testclass
        {   
            public string Test = ""new\nline"";
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
