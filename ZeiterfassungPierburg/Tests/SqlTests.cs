using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Tests
{
    [TestClass]
    public class SqlTests
    {
        [TestMethod]
        public void DictionariesWorkFine()
        {

            var d = SQLServer.Instance.GetDictionary("Fertigungsteil", "Bezeichnung", null);
            foreach (var x in d.Keys)
            {
                Console.WriteLine($"{x}: {d[x]}");
            }
            Assert.IsTrue(true);
        }
    }
}