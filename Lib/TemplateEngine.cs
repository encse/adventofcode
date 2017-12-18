using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;


using System.Text;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.CodeDom.Compiler;

namespace AdventOfCode2017.Templates {

    public interface Generator {

        string GenerateSolutionTemplate(SolutionTemplateModel model);
        string GenerateProjectReadme(ProjectReadmeModel model);
        string GenerateSplashScreen(SplashScreenModel model);
        
    }

    class TemplateEngine {

        public Generator Load(string templateFolder) {

            string classNameFromFileName(string fileName) {
                return Path.GetFileNameWithoutExtension(fileName);
            }

            var engine = RazorEngine.Create(b => {
                InheritsDirective.Register(b); // make sure the engine understand the @inherits directive in the input templates
                b.SetNamespace("AdventOfCode2017.Templates"); // define a namespace for the Template class
                b.ConfigureClass((r, c) => {
                    c.ClassName = classNameFromFileName(r.Source.FilePath);
                });
                b.Build();
            });

            var project = RazorProject.Create(".");
            var te = new RazorTemplateEngine(engine, project);

            var nameAndContent = Directory.EnumerateFiles(templateFolder)
                .Where(file => file.EndsWith(".cshtml"))
                .Select(file => {
                    var cs = te.GenerateCode(project.GetItem(file));
                    return (name: classNameFromFileName(file), content: CSharpSyntaxTree.ParseText(cs.GeneratedCode));
                })
                .ToList();

            var generatorFunctions = string.Join("\n", 
                from x in nameAndContent
                select $@"public string Generate{x.name}({x.name}Model model) {{
                    var template = new {x.name}();
                    template.Model = model;
                    template.ExecuteAsync().Wait();
                    return template.GetOutput();
                }}"
            );

            var trees = nameAndContent.Select(x => x.content).ToList();
            trees.Add(CSharpSyntaxTree.ParseText($@"
                namespace AdventOfCode2017.Templates {{
                    public class GeneratorImpl : Generator {{
                        {generatorFunctions}
                    }}
                }}
            "));

            var compilation = CSharpCompilation.Create(GetType().Name, trees,
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // include corlib
                    MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location), // this file (that contains the MyTemplate base class)

                    // for some reason on .NET core, I need to add this... this is not needed with .NET framework
                    MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Runtime.dll")),
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)); // we want a dll


            var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success) {
                throw new Exception(string.Join(Environment.NewLine, result.Diagnostics));
            }

            var asm = Assembly.Load(ms.ToArray());
            return (Generator)Activator.CreateInstance(asm.GetType("AdventOfCode2017.Templates.GeneratorImpl"));
        }
    }

    public class SolutionTemplateModel {
        public string Title { get; set; }
        public int Day { get; set; }
    }

    public class ProjectReadmeModel {
        public string Calendar { get; set; }
    }

    public class SplashScreenModel {
        public IEnumerable<CalendarToken> Calendar { get; set; }
    }

    public abstract class BaseTemplate<TModel> {
        StringBuilder sb = new StringBuilder();
        public TModel Model;

        public void WriteLiteral(string literal) {
            sb.Append(literal);
        }

        public void Write(object obj) {
            sb.Append(obj);
        }

        public string GetOutput() {
            return sb.ToString();
        }

        public string ToLiteral(string input) {
            return input.Replace("\n", "\\n");
        }
        public async virtual Task ExecuteAsync() {
            await Task.Yield(); // whatever, we just need something that compiles...
        }
    }
}