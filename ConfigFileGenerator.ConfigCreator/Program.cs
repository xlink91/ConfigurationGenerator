using ConfigFileGenerator.ConfigCreator.Implementation;
using System;
using System.Configuration;
using System.IO;

namespace ConfigFileGenerator.ConfigCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(" ", args));
//#if DEBUG
//            string pathBase = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"));
//            args = new[] { Path.Combine(pathBase, "ConfigFiles"), "Info"};
//#endif
            string key = "aspnet:RoslynCompilerLocation";
            string value = Path.Combine(AppContext.BaseDirectory, "roslyn");
            if (ConfigurationManager.AppSettings[key] != value)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            JsonCreator jsonCreator = new JsonCreator();
            jsonCreator.GenerateJson(args[0],args[1]);
        }
    }
}
