using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace GDataStructures
{
    public class ListTests
    {
        private GList<int> intList;
        private GList<string> sortList;
        private GList<string> searchList;
            
        [SetUp]
        public void InitialiseTests()
        {
            intList = new GList<int>(new IntComparer());
            intList.Add(1);
            intList.Add(20);
            intList.Add(5);

            sortList = new GList<string>(new StringComparer());
        }

        [Test]
        public void TestIntComparer()
        {
            var intComp = new IntComparer();
            Assert.IsTrue(intComp.Compare(5, 6) < 0);
            Assert.IsTrue(intComp.Compare(9, 6) > 0);
            Assert.IsTrue(intComp.Compare(5, 5) == 0);
        }

        [Test]
        public void TestAddIntListFunctionality()
        {
            Assert.AreEqual(intList.Count(),3);
            intList.Add(56);
            Assert.AreEqual(intList.Count(), 4);
        }

        [Test]
        public void TestIntSearch()
        {
            Assert.AreEqual(0, intList.IndexOf(1));
        }

        [Test]
        public void TestSort()
        {
            sortList.Add("aaa");
            sortList.Add("zzz");
            sortList.Add("bbb");
            sortList.Add("ttt");
            Assert.AreEqual("aaa", sortList.ItemAt(0));
            Assert.AreEqual("zzz", sortList.ItemAt(1));
            Assert.AreEqual("bbb", sortList.ItemAt(2));
            Assert.AreEqual("ttt", sortList.ItemAt(3));
            Assert.IsFalse(sortList.IsSorted());
            sortList.Sort();
            Assert.AreEqual(sortList.ItemAt(0), "aaa");
            Assert.AreEqual(sortList.ItemAt(1), "bbb");
            Assert.AreEqual(sortList.ItemAt(2), "ttt");
            Assert.AreEqual(sortList.ItemAt(3), "zzz");
        }

        [Test]
        public void TestSearch()
        {
            searchList.Add("hovercraft");
            searchList.Add("of");
            searchList.Add("eels");
            searchList.Add("my");
            searchList.Add("is");
            searchList.Add("full");
            Assert.IsTrue(searchList.Contains("my"));
            Assert.IsTrue(searchList.Contains("hovercraft"));
            Assert.IsTrue(searchList.Contains("is"));
            Assert.IsTrue(searchList.Contains("full"));
            Assert.IsTrue(searchList.Contains("of"));
            Assert.IsTrue(searchList.Contains("eels"));
            Assert.IsFalse(searchList.Contains("spam"));
            Assert.AreEqual(0, searchList.IndexOf("eels"));
        }
    }
}
