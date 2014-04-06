// Copyright (c) 2014, Bruce Mellows
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. This file must retain the above copyright notice, this list of conditions
//    and the following disclaimer.
// 2. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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
    public class AutoEventConnectorBehaviour
    {
        public AutoEventConnectorBehaviour()
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
        public void AutoEventConnector_TestConnectDisconnect()
        {
            var testEventPublisher = new TestEventPublisher();
            var testEventSubscriber = new TestEventSubscriber();

            testEventSubscriber.ConnectTo(testEventPublisher);

            testEventPublisher.RaiseTestEvent1();
            testEventPublisher.RaiseTestEvent2();
            Assert.AreEqual(0, testEventSubscriber.HandleTestEvent1Count);
            Assert.AreEqual(1, testEventSubscriber.HandleTestEventPublisherTestEvent1Count);

            testEventSubscriber.DisconnectFrom(testEventPublisher);

            testEventPublisher.RaiseTestEvent1();
            testEventPublisher.RaiseTestEvent2();
            Assert.AreEqual(0, testEventSubscriber.HandleTestEvent1Count);
            Assert.AreEqual(1, testEventSubscriber.HandleTestEventPublisherTestEvent1Count);
        }

        [TestMethod]
        public void AutoEventConnector_TestCache()
        {
            new TestEventSubscriber().ConnectTo(new TestEventPublisher());
            Assert.AreEqual(1, System.AutoEventConnector.CacheCount);

            new TestEventSubscriber().ConnectTo(new TestEventPublisher());
            Assert.AreEqual(1, System.AutoEventConnector.CacheCount);
        }

        private sealed class TestEventPublisher
        {
            public event EventHandler TestEvent1;

            public event EventHandler TestEvent2;

            public void RaiseTestEvent1()
            {
                var handler = this.TestEvent1;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }

            public void RaiseTestEvent2()
            {
                var handler = this.TestEvent2;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        private sealed class TestEventSubscriber
        {
            public int HandleTestEvent1Count { get; private set; }

            public int HandleTestEventPublisherTestEvent1Count { get; private set; }

            private void HandleTestEvent1(object sender, EventArgs args)
            {
                ++this.HandleTestEvent1Count;
            }

            private void HandleTestEventPublisherTestEvent1(object sender, EventArgs args)
            {
                ++this.HandleTestEventPublisherTestEvent1Count;
            }
        }
    }
}