using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Nu.OfficerMiniGame.Dal.Dto;
using Should;

namespace Nu.OfficerMiniGame.Test
{
    [TestClass]
    public class TestPfDateTimeJsonSerializer
    {
        [TestMethod]
        public void Deserialize()
        {
            var seralized = "{DateTime: \"2020/12/1 1:10:30\"}";
            var actual = JsonConvert.DeserializeObject<StubWithTime>(seralized);
            actual.DateTime.Year.ShouldEqual(2020);
            actual.DateTime.Month.ShouldEqual(12);
            actual.DateTime.Day.ShouldEqual(1);
            actual.DateTime.Hour.ShouldEqual(1);
            actual.DateTime.Minute.ShouldEqual(10);
            actual.DateTime.Second.ShouldEqual(30);
        }

        [TestMethod]
        public void Serialize()
        {
            var deserialized = new StubWithTime
            {
                DateTime = new PfDateTime(2020, 1, 24, 5, 30, 23)
            };
            var actual = JsonConvert.SerializeObject(deserialized);
            actual.ShouldEqual("{\"DateTime\":\"2020/1/24 5:30:23\"}");
        }

        [TestMethod]
        public void BackAndForth()
        {
            var deserialized = new StubWithTime
            {
                DateTime = new PfDateTime(2020, 1, 24, 5, 30, 23)
            };

            var actual = JsonConvert.DeserializeObject<StubWithTime>(JsonConvert.SerializeObject(deserialized));
            actual.DateTime.Year.ShouldEqual(2020);
            actual.DateTime.Month.ShouldEqual(1);
            actual.DateTime.Day.ShouldEqual(24);
            actual.DateTime.Hour.ShouldEqual(5);
            actual.DateTime.Minute.ShouldEqual(30);
            actual.DateTime.Second.ShouldEqual(23);

        }
    }

    public class StubWithTime
    {
        public PfDateTime DateTime { get; set; }
    }
}
