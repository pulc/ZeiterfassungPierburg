﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Models.NeueZeiterfassung;

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
        /*
        [TestMethod]
        public void ReadRecordsFromSQLServerWorks()
        {
            IEnumerable<Mitarbeiter> data = SQLServer.Instance.GetItems<Mitarbeiter>();
            Console.WriteLine("Test: reading Mitarbeiter objects from SQL server");
            foreach (var item in data)
                Console.WriteLine(item.ToString());
            Assert.AreEqual(2, data.Count());
        }
        */
        /*
        [TestMethod]
        public void FilteringMitarbeiterWorks()
        {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("vorname", "'Frodo'");
            IEnumerable<Mitarbeiter> data = SQLServer.Instance.GetItems<Mitarbeiter>(filter);
            Assert.AreEqual(1, data.Count());
        }
        */
        [TestMethod]
        public void InsertCommandIsValid()
        {
            Mitarbeiter m = new Mitarbeiter()
            {
                Nachname = "Schmidt",
                Vorname = "Günther",
                Kostenstelle = 23,
                Mitarbeiterkreis = 565,
                Abrechnungskreis = 2,
                Beschäftigungsart = "intern",
                Personalnummer = 273
            };
            Console.WriteLine(new DataMapper<Mitarbeiter>("Mitarbeiter").GetInsertSqlString(m));
            int newId = -1;
            try
            {
                newId = SQLServer.Instance.InsertItem<Mitarbeiter>(m);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error inserting item: " + e.Message);
                Assert.IsTrue(false);
            }
            Assert.AreNotEqual(-1, newId);
        }

        [TestMethod]
        public void FormToModelValid()
        {
            Mitarbeiter n = new Mitarbeiter();
            n.GetType();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InsertSchichtIsValid()
        {
            var produktionsanlagen = ControllerHelper.SelectColumn("Produktionsanlage", "Bezeichner", "Bezeichner");

            NeueZeiterfassung n = new NeueZeiterfassung()
            {
                Datum = new DateTime(2019, 11, 24),
                Name = "Baggins Bilbo",
                Schicht = 1,
                Fertigungsteil = "Volvo",
                Stückzahl = 10,
                Zeit = 6.5m,
                ProduktionsanlageList = ControllerHelper.GetSelectListItems(produktionsanlagen),
                Produktionsanlage = "Band 103"
            };
            NeueZeiterfassungDBHandler h = new NeueZeiterfassungDBHandler();
            Assert.IsTrue(h.AddZeiterfassung(n));
        }


    }
}