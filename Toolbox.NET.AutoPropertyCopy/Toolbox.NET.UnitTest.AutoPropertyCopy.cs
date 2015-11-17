// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AutoPropertyCopyBehaviour
    {
        public AutoPropertyCopyBehaviour()
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
        public void AutoPropertyCopy_TestCopy()
        {
            var testPropertySource = new TestPropertySource();
            var testPropertyTarget = new TestPropertyTarget();

            if (testPropertySource.Property1 == testPropertyTarget.Property1)
            {
                Assert.Inconclusive("the properties are already equal, so cannot be sure that it worked");
            }

            testPropertySource.CopyProperties(testPropertyTarget);

            Assert.AreEqual(testPropertySource.Property1, testPropertyTarget.Property1);
            Assert.AreEqual(testPropertySource.Property2, testPropertyTarget.Property2);
            Assert.AreEqual(testPropertySource.Property3, testPropertyTarget.Property3);
            Assert.AreNotEqual(testPropertySource.Property4, testPropertyTarget.Property4);
            Assert.AreNotEqual(testPropertySource.Property5, testPropertyTarget.Property5);
            Assert.AreEqual(testPropertySource.Property6, testPropertyTarget.Property6);
            Assert.AreEqual(testPropertySource.Property7, testPropertyTarget.Property7);
        }

        [TestMethod]
        public void AutoPropertyCopy_TestTwoCopies()
        {
            var testPropertySource = new TestPropertySource();
            var testPropertyTarget1 = new TestPropertyTarget();
            var testPropertyTarget2 = new TestPropertyTarget();

            if (testPropertySource.Property1 == testPropertyTarget1.Property1 || testPropertySource.Property1 == testPropertyTarget2.Property1)
            {
                Assert.Inconclusive("the properties are already equal, so cannot be sure that it worked");
            }

            testPropertySource.CopyProperties(testPropertyTarget1);

            Assert.AreEqual(testPropertySource.Property1, testPropertyTarget1.Property1);
            Assert.AreEqual(testPropertySource.Property2, testPropertyTarget1.Property2);
            Assert.AreEqual(testPropertySource.Property3, testPropertyTarget1.Property3);
            Assert.AreNotEqual(testPropertySource.Property4, testPropertyTarget1.Property4);
            Assert.AreNotEqual(testPropertySource.Property5, testPropertyTarget1.Property5);
            Assert.AreEqual(testPropertySource.Property6, testPropertyTarget1.Property6);
            Assert.AreEqual(testPropertySource.Property7, testPropertyTarget1.Property7);

            testPropertySource.CopyProperties(testPropertyTarget2);

            Assert.AreEqual(testPropertySource.Property1, testPropertyTarget2.Property1);
            Assert.AreEqual(testPropertySource.Property2, testPropertyTarget2.Property2);
            Assert.AreEqual(testPropertySource.Property3, testPropertyTarget2.Property3);
            Assert.AreNotEqual(testPropertySource.Property4, testPropertyTarget2.Property4);
            Assert.AreNotEqual(testPropertySource.Property5, testPropertyTarget2.Property5);
            Assert.AreEqual(testPropertySource.Property6, testPropertyTarget2.Property6);
            Assert.AreEqual(testPropertySource.Property7, testPropertyTarget2.Property7);
        }

        [TestMethod]
        public void AutoPropertyCopy_TestCache()
        {
            var testPropertySource = new TestPropertySource();
            var testPropertyTarget1 = new TestPropertyTarget();
            var testPropertyTarget2 = new TestPropertyTarget();

            testPropertySource.CopyProperties(testPropertyTarget1);
            Assert.AreEqual(1, System.AutoPropertyCopy.CacheCount);

            testPropertySource.CopyProperties(testPropertyTarget2);
            Assert.AreEqual(1, System.AutoPropertyCopy.CacheCount);
        }

        private sealed class TestPropertySource
        {
            public TestPropertySource()
            {
                this.Property1 = Guid.NewGuid().ToString();
                this.Property2 = Guid.NewGuid().ToString();
                this.Property4 = Guid.NewGuid().ToString();
                this.Property5 = 12345;
                this.Property6 = 6;
                this.Property7 = 7;
            }

            public string Property1 { get; set; }

            public string Property2 { get; private set; }

            public string Property3 { get { return this.Property1; } }

            internal string Property4 { get; set; }

            public int Property5 { get; set; }

            public int? Property6 { get; set; }

            public int Property7 { get; set; }
        }

        private sealed class TestPropertyTarget
        {
            public TestPropertyTarget()
            {
                this.Property1 = Guid.NewGuid().ToString();
                this.Property2 = Guid.NewGuid().ToString();
                this.Property4 = Guid.NewGuid().ToString();
                this.Property5 = Guid.NewGuid().ToString();
                this.Property6 = -6;
                this.Property7 = -7;
            }

            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string Property3 { get; set; }
            public string Property4 { get; set; }
            public string Property5 { get; set; }
            public int Property6 { get; set; }
            public int? Property7 { get; set; }
        }
    }
}