using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigFileGenerator.ConfigCreator.Implementation
{
    public class JsonCreator
    {
        public void GenerateJson(string _path, string _class)
        {
            string path = _path;
            string @class = _class + ".cs";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeDomProvider objCodeCompiler = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };
            parameters.ReferencedAssemblies.AddRange(new[] { "System.Core.dll" });
            string rs = Path.Combine(Path.Combine(path, "Schema"), @class);
            var code = Regex.Replace(Regex.Unescape(File.ReadAllText(rs)), @"\t|\n|\r", "");
            CompilerResults cresult = provider.CompileAssemblyFromSource(parameters, code);
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
