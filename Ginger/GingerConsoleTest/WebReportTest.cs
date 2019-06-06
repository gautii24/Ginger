﻿using amdocs.ginger.GingerCoreNET;
using Amdocs.Ginger.CoreNET.Logger;
using Amdocs.Ginger.CoreNET.Repository;
using Amdocs.Ginger.GingerConsole;
using Amdocs.Ginger.GingerConsole.ReporterLib;
using Amdocs.Ginger.Repository;
using Ginger.SolutionGeneral;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GingerConsoleTest
{
    [TestClass]
    public class WebReportTest
    {
        private  SolutionRepository sr;

        public WebReportTest()
        {
            InitWorkSpace();
        }

        private void InitWorkSpace()
        {
            GingerConsoleWorkSpace ws = new GingerConsoleWorkSpace();
            WorkSpace.Init(ws);
            OpenSolution(@"C:\Ginger\test");
            WorkSpace.Instance.Solution = (Solution)(ISolution)sr.RepositorySerializer.DeserializeFromFile(Path.Combine(sr.SolutionFolder, "Ginger.Solution.xml"));

        }
        [TestMethod]
        [Timeout(60000)]
        public void TestNewWebReport()
        {
            //a selected guid can be send 
            string guidStr = "";
            // a selected browser from unix can be run ,with his path
            string browserPath = "C:\\Program Files\\Internet Explorer\\iexplore.exe";
            WebReportGenerator webReporterRunner = new WebReportGenerator();
            Assert.IsTrue(webReporterRunner.RunNewHtmlReport());
        }

        private void OpenSolution(string sFolder)
        {
            if (Directory.Exists(sFolder))
            {
                Console.WriteLine("Opening Solution at folder: " + sFolder);
                sr = GingerSolutionRepository.CreateGingerSolutionRepository();
                WorkSpace.Instance.SolutionRepository = sr;
                sr.Open(sFolder);
            }
            else
            {
                Console.WriteLine("Directory not found: " + sFolder);
            }
        }

    }
}
