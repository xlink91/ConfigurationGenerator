using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConfigFileGenerator.ConfigCreator.Implementation
{
    public class JsonCreator
    {
        public void GenerateJson(string _path, string _class)
        {
            string path = _path;
            string @class = _class + ".cs";

            string rs = Path.Combine(Path.Combine(path, "Schema"), @class);
            var code = Regex.Replace(Regex.Unescape(File.ReadAllText(rs)), @"\t|\n|\r", "");

            CSharpCodeProvider provider =
                new CSharpCodeProvider();

            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.AddRange(new[] { "System.Core.dll" });

            CompilerResults cresult =
               compiler.CompileAssemblyFromSource(compilerparams, code);
            if (cresult.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in cresult.Errors)
                {
                    errors.AppendFormat("Line {0},{1}\t: {2}\n",
                           error.Line, error.Column, error.ErrorText);
                }
                throw new Exception(errors.ToString());
            }

            //            Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider()
            //;            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            //            CodeDomProvider objCodeCompiler = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();

            //            CompilerParameters parameters = new CompilerParameters
            //            {
            //                GenerateInMemory = true,
            //                GenerateExecutable = false
            //            };
            //            parameters.ReferencedAssemblies.AddRange(new[] { "System.Core.dll" });
            //            string rs = Path.Combine(Path.Combine(path, "Schema"), @class);
            //            var code = Regex.Replace(Regex.Unescape(File.ReadAllText(rs)), @"\t|\n|\r", "");
            //            CompilerResults cresult = provider.CompileAssemblyFromSource(parameters, code);
            /*
            string errorMessage = string.Empty;
            if (cresult.Errors.HasErrors)
            {
                foreach (CompilerError compileError in cresult.Errors)
                {   
                    // Display compilation errors.
                    errorMessage += Environment.NewLine + compileError.ErrorText;
                }
            }
            */
            bool gac = cresult.CompiledAssembly.GlobalAssemblyCache;
            Type type = cresult.CompiledAssembly.ExportedTypes.Where(x => x.Name == _class).First();
            var obj = Activator.CreateInstance(type);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Path.Combine(Path.Combine(path, "Data"), _class + ".json"), json);
        }
    }
}
