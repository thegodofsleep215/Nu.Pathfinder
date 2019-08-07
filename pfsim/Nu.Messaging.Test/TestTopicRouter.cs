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

        [TestMethod]
        public void TestPubSubByClass()
        {
            var router = new TopicRouter();
            var tc = new TestClass();

            router.SubscribeClass(tc);

            router.Publish(true, "DoesNotMatter");

            tc.Value.ShouldBeTrue();
        }
    }

    class TestClass
    {
        public bool Value { get; set; }

        [Subscription(typeof(bool), "#")]
        public void HandleBool(bool b)
        {
            Value = b;
        }
    }
}
