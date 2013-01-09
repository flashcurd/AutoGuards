﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoGuards.Engine;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;
using Roslyn.Services;
using Roslyn.Services.CSharp;

namespace AutoGuards.CompileConsole
{
    internal class Program
    {
        /*private static void Main3(string[] args)
        {
            IWorkspace workspace = Workspace.LoadStandAloneProject(@"C:\Dev\MySolutions\AutoGuards\AutoGuards.Target\AutoGuards.Target.csproj");

            foreach (var project in workspace.CurrentSolution.Projects)
            {
                var originalCompilation = project.GetCompilation();

                Compilation newCompilation = Compilation.Create("temp").AddReferences(project.MetadataReferences);

                foreach (var document in project.Documents)
                {
                    SyntaxTree tree = SyntaxTree.ParseText(document.GetText());

                    newCompilation = newCompilation.AddSyntaxTrees(tree);

                    SemanticModel semanticModel = newCompilation.GetSemanticModel(tree);

                    AutoGuardSyntaxRewriter rewriter = new AutoGuardSyntaxRewriter(newCompilation, semanticModel);

                    var rewritten = rewriter.Visit(tree.GetRoot());

                    newCompilation = newCompilation.ReplaceSyntaxTree(tree, rewritten.SyntaxTree );

                }

                using (var file = new FileStream("CompiledTarget.exe", FileMode.Create))
                {
                    var result = newCompilation.Emit(file);
                }
            }
        }*/

        private static void Main(string[] args)
        {
            IWorkspace workspace = Workspace.LoadStandAloneProject(@"C:\Dev\MySolutions\AutoGuards\AutoGuards.Target\AutoGuards.Target.csproj");

            foreach (var project in workspace.CurrentSolution.Projects)
            {
                var compilation = project.GetCompilation();

                foreach (var document in project.Documents)
                {
                    SemanticModel model = (SemanticModel) document.GetSemanticModel();

                    var rewriter = new AutoGuardSyntaxRewriter(compilation, model);

                    //Dont know of a nice way of getting the root node.
                    var rewritten = rewriter.Visit(model.SyntaxTree.GetRoot().AncestorsAndSelf().First());

                    compilation = compilation.ReplaceSyntaxTree(document.GetSyntaxTree(), rewritten.SyntaxTree);
                }

                using (var file = new FileStream("CompiledTarget.exe", FileMode.Create))
                {
                    var result = compilation.Emit(file);
                }
            }
        }
    }
}
