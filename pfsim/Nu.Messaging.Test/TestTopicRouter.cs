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

        [TestMethod]
        public void TestRemoteCall()
        {
            var router = new TopicRouter();

            router.RegisterRemoteCall((int x) => x + 1);
            router.RemoteCall<int, int>(1).ShouldEqual(2);
        }

        [TestMethod]
        public void TestRemoteCallByClass()
        {
            var router = new TopicRouter();
            var tc = new TestClass();

            router.SubscribeClass(tc);

            router.RemoteCall<int, int>(1).ShouldEqual(2);
        }

    }

    class TestClass
    {
        public bool Value { get; set; }

        [Subscription("#")]
        public void HandleBool(bool b)
        {
            Value = b;
        }

        [RemoteCall]
        public int AddOne(int i)
        {
            return i + 1;
        }
    }
}
