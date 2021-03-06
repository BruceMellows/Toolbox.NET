﻿// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET.UnitTest
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Toolbox.NET;

    [TestClass]
    public class FactoryBehavior
    {
        #region Additional test attributes

        public FactoryBehavior()
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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
        }

        //
        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void TestInitialize()
        {
        }

        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
        }

        #endregion Additional test attributes

        [TestMethod]
        public void ConstructFactory()
        {
            FactoryCreateService.IFactory();
        }

        [TestMethod]
        public void ConstructWidgetFactory()
        {
            FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                ;
        }

        [TestMethod]
        public void MissingWidget()
        {
            var widgetMaker = FactoryCreateService.IFactory()
                .WidgetMaker<Tuple<string, int>>()
                .Setup(() => Tuple.Create(string.Empty, 0))
                ;

            try
            {
                widgetMaker
                    .Create(Tuple.Create(string.Empty, 0), Tuple.Create(string.Empty, 0))
                    ;

                Assert.IsTrue(false);
            }
            catch (InvalidProgramException)
            {
            }
        }

        [TestMethod]
        public void MakeWidget0()
        {
            FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup(() => new List<int>())
                .Create()
                ;
        }

        [TestMethod]
        public void MakeWidget1()
        {
            FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup((string s) => new List<int>())
                .Create(string.Empty)
                ;
        }

        [TestMethod]
        public void MakeWidget2()
        {
            FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup((string s, int i) => new List<int>())
                .Create(string.Empty, 0)
                ;
        }

        [TestMethod]
        public void MakeWidget3()
        {
            FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup((string s, int i, Type t) => new List<int>())
                .Create(string.Empty, 0, typeof(int))
                ;
        }

        [TestMethod]
        public void MergeAddUpdateWithoutDuplicates()
        {
            var maker1 = FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup(() => new List<int> { 1 })
                ;
            var maker2 = FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup((string s) => new List<int> { 1, 2 })
                ;

            // Throw
            var mergedFactory = FactoryCreateService.IFactory();
            mergedFactory
                .Pull(maker1, ItemSinkDuplicate.Throw)
                .Pull(maker2, ItemSinkDuplicate.Throw)
                ;
            Assert.AreEqual(1, mergedFactory.WidgetMaker<List<int>>().Create().Count);
            Assert.AreEqual(2, mergedFactory.WidgetMaker<List<int>>().Create(string.Empty).Count);

            // Ignore
            var addedFactory = FactoryCreateService.IFactory();
            addedFactory
                .Pull(maker1, ItemSinkDuplicate.Ignore)
                .Pull(maker2, ItemSinkDuplicate.Ignore)
                ;
            Assert.AreEqual(1, addedFactory.WidgetMaker<List<int>>().Create().Count);
            Assert.AreEqual(2, addedFactory.WidgetMaker<List<int>>().Create(string.Empty).Count);

            // Override
            var updatedFactory = FactoryCreateService.IFactory();
            updatedFactory
                .Pull(maker1, ItemSinkDuplicate.Override)
                .Pull(maker2, ItemSinkDuplicate.Override)
                ;
            Assert.AreEqual(1, updatedFactory.WidgetMaker<List<int>>().Create().Count);
            Assert.AreEqual(2, updatedFactory.WidgetMaker<List<int>>().Create(string.Empty).Count);
        }

        [TestMethod]
        public void MergeAddUpdateWithDuplicates()
        {
            var maker1 = FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup(() => new List<int> { 1 })
                ;

            var maker2 = FactoryCreateService.IFactory()
                .WidgetMaker<List<int>>()
                .Setup(() => new List<int> { 1, 2 })
                ;

            // Throw
            try
            {
                FactoryCreateService.IFactory()
                    .Pull(maker1, ItemSinkDuplicate.Throw)
                    .Pull(maker2, ItemSinkDuplicate.Throw)
                    ;

                Assert.IsTrue(false);
            }
            catch (InvalidProgramException)
            {
            }

            // Ignore
            IFactory addedFactory = FactoryCreateService.IFactory();
            addedFactory
                .Pull(maker1, ItemSinkDuplicate.Ignore)
                .Pull(maker2, ItemSinkDuplicate.Ignore)
                ;

            Assert.AreEqual(1, addedFactory.WidgetMaker<List<int>>().Create().Count);

            // Override
            IFactory updatedFactory = FactoryCreateService.IFactory();
            updatedFactory
                .Pull(maker1, ItemSinkDuplicate.Override)
                .Pull(maker2, ItemSinkDuplicate.Override)
                ;

            Assert.AreEqual(2, updatedFactory.WidgetMaker<List<int>>().Create().Count);
        }

        [TestMethod]
        public void FactoryCanCreateFromSuccess()
        {
            var canCreate = FactoryCreateService.IFactory()
                .WidgetMaker<string>()
                .Setup((string s) => string.Empty)
                .CanCreateFrom<string>()
                ;
            Assert.IsTrue(canCreate);
        }

        [TestMethod]
        public void FactoryCanCreateFromFailure()
        {
            var canCreate = FactoryCreateService.IFactory()
                .WidgetMaker<string>()
                .CanCreateFrom<string>()
                ;
            Assert.IsFalse(canCreate);
        }
    }
}