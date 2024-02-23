using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<Dnn.Analyzer.DotNetNuke.Security.Permissions.PermissionInfoPermissionIDUsageAnalyzer>;

namespace Dnn.Analyzer.Test.Security.Permissions
{
    [TestClass]
    public class PermissionInfoAnalyzerTests
    {
        [TestMethod]
        public async Task PermissionInfo_PermissionID_Deprecated()
        {
            // Arrange
            var codeSample = @"
                using DotNetNuke;

                namespace Dnn.Analyzer.Test.Security.Permissions.CodeSamples
                {
                    internal class UsingPermissionID
                    {
                        public void UsingPermissionIDMethod()
                        {
                            var permissionInfo = new DotNetNuke.Security.Permissions.PermissionInfo();
                            permissionInfo.PermissionID = 123;
                        }
                    }
                }
                ";

            var fixedCode = @"
                using DotNetNuke.Abstractions.Security.Permissions;
                using DotNetNuke;

                namespace Dnn.Analyzer.Test.Security.Permissions.CodeSamples
                {
                    internal class UsingPermissionID
                    {
                        public void UsingPermissionIDMethod()
                        {
                            var permissionInfo = new DotNetNuke.Security.Permissions.PermissionInfo();
                            (permissionInfo as IPermissionDefinitionInfo).PermissionId = 123;
                        }
                    }
                }
                ";
            var expectedDiagnostic = new DiagnosticResult("DnnAnalyzerPermissionInfo", DiagnosticSeverity.Warning)
                .WithSpan("Test0.cs", 8, 29, 8, 42)
                .WithMessage("PermissionInfo.PermissionID is deprecated. Use IPermissionDefinitionInfo.PermissionId instead.");

            // Act and Assert
            await VerifyCS.VerifyAnalyzerAsync(codeSample, expectedDiagnostic);
        }
    }
}
