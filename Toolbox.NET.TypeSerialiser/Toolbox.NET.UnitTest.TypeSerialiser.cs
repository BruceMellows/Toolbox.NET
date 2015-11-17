// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET.UnitTest
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TypeSerialiserBehaviour
    {
        public TypeSerialiserBehaviour()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }

            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes

        [TestMethod]
        public void SimpleTypeToString()
        {
            Assert.AreEqual("System.String", new TypeSerialiser(typeof(string)).ToString());
            Assert.AreEqual("System.String", string.Format("{0}", new TypeSerialiser(typeof(string))));
            Assert.AreEqual("System.String", string.Format("{0:XFER}", new TypeSerialiser(typeof(string))));
        }

        [TestMethod]
        public void SimpleStringToType()
        {
            Assert.AreEqual(typeof(string), new TypeSerialiser("System.String").Type);
        }

        [TestMethod]
        public void SimpleTypeToCSharpString()
        {
            Assert.AreEqual("System.String", new TypeSerialiser(typeof(string)).ToString(TypeSerialiser.CSharpFormat));
            Assert.AreEqual("System.String", string.Format("{0:CS}", new TypeSerialiser(typeof(string))));
        }

        [TestMethod]
        public void ComplexTypeToString()
        {
            Assert.AreEqual("System.Collections.Generic.List[System.String]", new TypeSerialiser(typeof(List<string>)).ToString());
            Assert.AreEqual("System.Collections.Generic.List[System.String]", string.Format("{0}", new TypeSerialiser(typeof(List<string>))));
            Assert.AreEqual("System.Collections.Generic.List[System.String]", string.Format("{0:XFER}", new TypeSerialiser(typeof(List<string>))));
        }

        [TestMethod]
        public void ComplexTypeToCSharpString()
        {
            Assert.AreEqual("System.Collections.Generic.List<System.String>", new TypeSerialiser(typeof(List<string>)).ToString(TypeSerialiser.CSharpFormat));
            Assert.AreEqual("System.Collections.Generic.List<System.String>", string.Format("{0:CS}", new TypeSerialiser(typeof(List<string>))));
        }

        [TestMethod]
        public void ComplexStringToType()
        {
            Assert.AreEqual(typeof(List<string>), new TypeSerialiser("System.Collections.Generic.List[System.String]").Type);
        }

        [TestMethod]
        public void VeryComplexTypeToString()
        {
            Assert.AreEqual("System.Collections.Generic.List[System.Tuple[System.Int32,System.String]]", new TypeSerialiser(typeof(List<Tuple<int, string>>)).ToString());
            Assert.AreEqual("System.Collections.Generic.List[System.Tuple[System.Int32,System.String]]", string.Format("{0}", new TypeSerialiser(typeof(List<Tuple<int, string>>))));
            Assert.AreEqual("System.Collections.Generic.List[System.Tuple[System.Int32,System.String]]", string.Format("{0:XFER}", new TypeSerialiser(typeof(List<Tuple<int, string>>))));
        }

        [TestMethod]
        public void VeryComplexTypeToCSharpString()
        {
            Assert.AreEqual("System.Collections.Generic.List<System.Tuple<System.Int32,System.String>>", new TypeSerialiser(typeof(List<Tuple<int, string>>)).ToString(TypeSerialiser.CSharpFormat));
            Assert.AreEqual("System.Collections.Generic.List<System.Tuple<System.Int32,System.String>>", string.Format("{0:CS}", new TypeSerialiser(typeof(List<Tuple<int, string>>))));
        }

        [TestMethod]
        public void VeryComplexStringToType()
        {
            Assert.AreEqual(typeof(List<Tuple<int, string>>), new TypeSerialiser("System.Collections.Generic.List[System.Tuple[System.Int32,System.String]]").Type);
        }
    }
}