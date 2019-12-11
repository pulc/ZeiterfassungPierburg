using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungTests
{
    [TestClass]
    public class SchichtInfoDataMappingTest
    {
        DataMapper<SchichtInfo> dmS;
        SchichtInfo s1;
        [TestInitialize]
        public void Initialize()
        {
            dmS = new DataMapper<SchichtInfo>("SchichtInfo");
            s1 = new SchichtInfo()
            {
                Art = SchichtInfo.Schichtart.Früh,
                Datum = new DateTime(2019, 12, 6),
                Identifier = 1
            };
        }
    }

    [TestClass]
    public class MitarbeiterDataMappingTest
    {
        DataMapper<Mitarbeiter> dmM;
        Mitarbeiter m1;
        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine("[TST] DataMapping: Mitarbeiter init");
            dmM = new DataMapper<Mitarbeiter>("Mitarbeiter");
            m1 = new Mitarbeiter()
            {
                Nachname = "Schmidt",
                Vorname = "Herbert",
                Identifier = 1,
                Beschäftigungsart = "intern",
                Kostenstelle = 41,
                Mitarbeiterkreis = 1,
                Abrechnungskreis = 1,
                Personalnummer = 1683
            };
        }
        [TestMethod]
        public void InsertSQLCommandIsValid()
        {
            string s = dmM.InsertCommand(m1);
            System.Console.WriteLine(m1.Nachname);
            System.Console.WriteLine(String.Format("[TEST] Insert Mitarbeiter Command: {0}",
                s));
            Assert.IsNotNull(s);
        }
    }
}
