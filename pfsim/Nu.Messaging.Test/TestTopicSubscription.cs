using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Nu.Messaging.Test
{
    [TestClass]
    public class TestTopicSubscription
    {
        [TestClass]
        public class TesRoutingKey : TestTopicSubscription
        {

            [TestMethod]
            public void SingleWordMatches()
            {
                var sub = new TopicSubscription(null, "Test");
                sub.TestRoutingKey("Test").ShouldBeTrue();
            }

            [TestMethod]
            public void SingleWordDoesNotMatch()
            {
                var sub = new TopicSubscription(null, "Test");
                sub.TestRoutingKey("Best").ShouldBeFalse();
            }

            [TestMethod]
            public void TwoWordsMatches()
            {
                var sub = new TopicSubscription(null, "Test.Foo");
                sub.TestRoutingKey("Test.Foo").ShouldBeTrue();
            }

            [TestMethod]
            public void TwoWordsDoesNotMatch()
            {
                var sub = new TopicSubscription(null, "Test.Foo");
                sub.TestRoutingKey("Test.Fee").ShouldBeFalse();
            }

            [TestMethod]
            public void WildCardAtEnd()
            {
                var sub = new TopicSubscription(null, "Test.*");
                sub.TestRoutingKey("Test.Foo").ShouldBeTrue();
            }

            [TestMethod]
            public void WildCardInBegining()
            {
                var sub = new TopicSubscription(null, "*.Foo");
                sub.TestRoutingKey("Test.Foo").ShouldBeTrue();
            }

            [TestMethod]
            public void WildCardInMiddle()
            {
                var sub = new TopicSubscription(null, "Test.*.Foo");
                sub.TestRoutingKey("Test.Word.Foo").ShouldBeTrue();
            }

            [TestMethod]
            public void TwoWildCards()
            {
                var sub = new TopicSubscription(null, "Test.*.Foo.*");
                sub.TestRoutingKey("Test.Word.Foo.Thing").ShouldBeTrue();
            }

            [TestMethod]
            public void HashOnly()
            {
                var sub = new TopicSubscription(null, "#");
                sub.TestRoutingKey("Test.Word.Foo.Thing").ShouldBeTrue();
            }

            [TestMethod]
            public void WordThenHash()
            {
                var sub = new TopicSubscription(null, "Test.#");
                sub.TestRoutingKey("Test.Word.Foo.Thing").ShouldBeTrue();
            }

            [TestMethod]
            public void WordThenHashThenWord()
            {
                var sub = new TopicSubscription(null, "Test.#.Thing");
                sub.TestRoutingKey("Test.Word.Foo.Thing").ShouldBeTrue();
            }

            [TestMethod]
            public void KeysAreNotSameLengthPubIsLonger()
            {
                var sub = new TopicSubscription(null, "Test.Thing");
                sub.TestRoutingKey("Test.Word.Foo.Thing").ShouldBeFalse();
            }

            [TestMethod]
            public void KeysAreNotSameLengthSubIsLonger()
            {
                var sub = new TopicSubscription(null, "Test.Thing.Very.Long.Key.You.See");
                sub.TestRoutingKey("Test.Word.Foo.Thing").ShouldBeFalse();
            }


        }
    }
}
