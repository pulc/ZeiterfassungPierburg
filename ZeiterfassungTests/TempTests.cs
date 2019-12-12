using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungPierburg.Models;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungTests
{
    [TestClass]
    public class TempTests
    {
        [TestMethod]
        public void FindingAttributesByInterfaceIsAsExpected()
        {
            SchichtInfo si = new SchichtInfo()
            {
                Datum = new DateTime(2019, 12, 6),
                Art = SchichtInfo.Schichtart.Früh,
                Identifier = 14
            };
            object[] atts = si.GetType().GetProperty("Datum")
                           .GetCustomAttributes(typeof(IPropertyStringFunctionAttribute), false);
            Console.WriteLine(String.Join(", ", atts));
            Assert.IsTrue(atts.Length > 0);
        }
    }
}
