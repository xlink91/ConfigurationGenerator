﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigFileGenerator.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ConfigFileGeneratorTests.CustomTypes;
using ConfigFileGenerator.Contract;

namespace ConfigFileGenerator.Implementation.Tests
{
    [TestClass()]
    public class ConfigResolverTests
    {
        [TestInitialize]
        public void PutConfigFile()
        {
            if(!System.IO.File.Exists(@"ConfigFiles\Schema\Information.cs"))
                System.IO.File.Copy(@"..\..\CustomTypes\Information.cs", @"ConfigFiles\Schema\Information.cs", false);
            if (System.IO.File.Exists(@"ConfigFiles\Data\Information.json"))
                return;

            ProcessStartInfo pInfo = new ProcessStartInfo
            {
                FileName = "ConfigFileGenerator.ConfigCreator.exe",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = $"ConfigFiles {nameof(Information)}"
            };
            Process process = Process.Start(pInfo);
            process.WaitForExit();
        }

        [TestMethod()]
        public void ResolveTest()
        {
            Guid guid = ConfigResolver.Instance.Resolve<Information, Guid>(x => x.Guid);
            Assert.AreEqual(guid.ToString().ToUpper(), "087D3AD4-7F5D-457B-A16C-9D402D22B2E6");
        }
    }
}