using NUnit.Framework;
using Models.Common.Encode;
using Models.EntityFramework;

namespace ShopNetMVC.Tests
{
    [TestFixture]
    public class UnitTest
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void testGenIdFormat_ddmmyy()
        {
            var expected_results = "0211180002";
            var dummyData = new
            {
                db = new ShopDbContext(),
                table = Converter.ItemTypes.Bill
            };
            Assert.AreEqual(expected_results, Converter.genIdFormat_ddmmyy(dummyData.db, dummyData.table));
        }

        [Test]
        public void testFormatPrice()
        {
            var expected_results = "15,000,000";
            var dummyData = new
            {
                cost = 150000000
            };
            Assert.AreEqual(expected_results, Converter.formatPrice(dummyData.cost));
        }
    }
}
