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
                Identifier = 65
            };
            dmSi = new DataMapper<SchichtInfo>("SchichtInfo");
        }

        [TestMethod]
        public void CustomToStringFunctionsAreAllFound()
        {
            Assert.AreEqual(dmSi.propertyToStringFunctions.Count, 1);
        }

        [TestMethod]
        public void DatumCustomStringFunctionYieldsValidDateFormat()
        {
            Assert.AreEqual(
            dmSi.propertyToStringFunctions["Datum"].Invoke(
                new DateTime(2019, 12, 6)), "2019-12-06");
        }

        [TestMethod]
        public void GetItemsSQLStringIsOkay()
        {
            Console.WriteLine(dmSi.GetItemsSQLString());
            Console.WriteLine(String.Join(", ",dmSi.propertyColumnMappings.Values));
            Assert.IsTrue(true);
        }
    }
}