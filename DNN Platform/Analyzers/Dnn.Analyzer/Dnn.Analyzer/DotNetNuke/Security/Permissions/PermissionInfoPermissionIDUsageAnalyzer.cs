using System;
using System.Collections.Immutable;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Dnn.Analyzer.DotNetNuke.Security.Permissions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class PermissionInfoPermissionIDUsageAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DnnAnalyzerPermissionInfo";

        private static readonly string Title = "PermissionInfo.PermissionID usage";
        private static readonly string MessageFormat = "PermissionInfo.PermissionID is deprecated. Use IPermissionDefinitionInfo.PermissionId instead.";
        private static readonly string Description = "PermissionInfo.PermissionID property is deprecated and should not be used.";
        private const string Category = "Usage";
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.SimpleAssignmentExpression);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var assignmentExpression = (AssignmentExpressionSyntax)context.Node;

            // Check if the left side of the assignment is a member access expression
            if (assignmentExpression.Left is MemberAccessExpressionSyntax memberAccess)
            {
                // Check for the name "PermissionID"
                if (memberAccess.Name.Identifier.Text.Equals("PermissionID"))
                {
                    // Resolve the type of the left-hand-side expression
                    var leftSymbol = context.SemanticModel.GetSymbolInfo(memberAccess.Expression).Symbol;

                    // If the left-hand-side symbol is of type PermissionInfo, report the diagnostic
                    if (leftSymbol != null && leftSymbol.ToString() == "DotNetNuke.Security.Permissions.PermissionInfo")
                    {
                        var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
