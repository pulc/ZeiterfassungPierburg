using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Tests
{
    [TestClass]
    public class SchichtInfoDataMappingTest
    {
        SchichtInfo si;
        DataMapper<SchichtInfo> dmSi;
        [TestInitialize]
        public void Initialize()
        {
            si = new SchichtInfo()
            {
                Art = SchichtInfo.Schichtart.Früh,
                Datum = new DateTime(2019, 12, 6),
                ID = 65
            };
            dmSi = new DataMapper<SchichtInfo>("SchichtInfo");
        }
    }

    [TestClass]
    public class MitarbeiterTests
    {
        [TestMethod]
        public void ReadRecordsFromSQLServerWorks()
        {
            IEnumerable<Mitarbeiter> data = SQLServer.Instance.GetItems<Mitarbeiter>();
            Console.WriteLine("Test: reading Mitarbeiter objects from SQL server");
            foreach (var item in data)
                Console.WriteLine(item.ToString());
            Assert.AreEqual(2, data.Count());
        }

        [TestMethod]
        public void FilteringMitarbeiterWorks()
        {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("vorname", "'Frodo'");
            IEnumerable<Mitarbeiter> data = SQLServer.Instance.GetItems<Mitarbeiter>(filter);
            Assert.AreEqual(1, data.Count());
        }
    }
}