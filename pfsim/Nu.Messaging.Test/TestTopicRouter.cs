using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Nu.Messaging.Test
{

    [TestClass]
    public class TestTopicRouter
    {

        [TestMethod]
        public void TestPubSub()
        {
            var router = new TopicRouter();

            var testObj = false;
            router.Subscribe((bool b) => testObj = true, "Key");
            router.Publish(true, "Key");

            testObj.ShouldBeTrue();
        }
    }
}
