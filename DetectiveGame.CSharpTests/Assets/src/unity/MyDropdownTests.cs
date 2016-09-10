using Microsoft.VisualStudio.TestTools.UnitTesting;
using unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unity.Tests
{
    [TestClass()]
    public class MyDropdownTests
    {
        [TestMethod()]
        public void clearTest()
        {
            MyDropdown dropdown = new MyDropdownDebug();
            dropdown.add("a", 10);
            dropdown.clear();

            Assert.AreEqual(dropdown.getSelect(), 0);
        }

        [TestMethod()]
        public void addTest()
        {
            MyDropdown dropdown = new MyDropdownDebug();
            dropdown.add("a", 10);
            Assert.AreEqual(dropdown.getSelect(), 10);
        }

        [TestMethod()]
        public void getSelectTest()
        {
            MyDropdown dropdown = new MyDropdownDebug();
            dropdown.add("a", 10);
            Assert.AreEqual(dropdown.getSelect(), 10);
        }

        [TestMethod()]
        public void selectTest()
        {
            MyDropdown dropdown = new MyDropdownDebug();
            dropdown.add("a", 10);
            dropdown.add("b", 12);
            dropdown.select(12);
            Assert.AreEqual(dropdown.getSelect(), 12);

        }
    }
}