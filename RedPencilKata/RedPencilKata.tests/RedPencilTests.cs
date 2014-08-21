using System;
using NUnit.Framework;
using Promotion;

namespace RedPencilKata.tests
{
    [TestFixture]
    public class RedPencilTests
    {
        private RedPencilPromotion _test;
        private DateTime _makeStableDate;

        [SetUp]
        public void Init()
        {
            _test = new RedPencilPromotion();
            _makeStableDate = DateTime.Now.AddDays(-30);
        }

        [Test]
        public void Set_NewItemFullPrice()
        {            
            var item = new RedPencilItem(100.00m);
            TextOutput(item);

            Assert.AreEqual(100.00m, item.FullPrice);
        }

        [Test]
        public void Change_ItemFullPrice()
        {
            var item = new RedPencilItem(100.00m);
            var initalChangeDate = item.FullPriceUpdateDate;
            TextOutput(item);

            //System.Threading.Thread.Sleep(2000); //added to verify that FullPriceUpdateDate is getting updated
            item.FullPrice = 200.00m;
            TextOutput(item);

            Assert.AreEqual(200.00m, item.FullPrice);
            Assert.AreNotEqual(initalChangeDate, item.FullPriceUpdateDate);
        }
        
        [Test]
        public void Check_NewItemIsRedPencilItemtion_False()
        {
            var item = new RedPencilItem(100.00m);
            TextOutput(item);

            Assert.IsFalse(item.IsRedPencilPromo);
        }


        [Test]
        public void Set_ItemPromotionPrice()
        {            
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.AreEqual(100.00m, item.FullPrice);
            Assert.AreEqual(80.00m, item.PromotionPrice);
        }

        [Test]
        public void Check_PromotionPriceIsFivePercentOrHigher_True()
        {
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 95.00m);
            TextOutput(item);

            Assert.IsTrue(_test.PassLowerPercent(item));

            var item2 = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item2, 94.99m);
            TextOutput(item2);

            Assert.IsTrue(_test.PassLowerPercent(item2));
        }

        [Test]
        public void Check_PromotionPriceIsFivePercentOrHigher_False()
        {
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 95.01m);
            TextOutput(item);

            Assert.IsFalse(_test.PassLowerPercent(item));
        }

        [Test]
        public void Check_PromotionPriceIsThrityPercentOrLower_True()
        {
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 70.00m);
            TextOutput(item);

            Assert.IsTrue(_test.PassUpperPercent(item));

            var item2 = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item2, 70.01m);
            TextOutput(item2);

            Assert.IsTrue(_test.PassUpperPercent(item2));
        }

        [Test]
        public void Check_PromotionPriceIsThrityPercentOrLower_False()
        {
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 69.99m);
            TextOutput(item);

            Assert.IsFalse(_test.PassUpperPercent(item));
        }

        [Test]
        public void Check_FullPriceStableAtLeastThrityDays_True()
        {
            var item = new RedPencilItem(100.00m) {FullPriceUpdateDate = DateTime.Now.AddDays(-30)};
            TextOutput(item);
           
            Assert.IsTrue(_test.IsFullPriceStable(item));
        }

        [Test]
        public void Check_FullPriceStableAtLeastThrityDays_False()
        {
            var item = new RedPencilItem(100.00m) {FullPriceUpdateDate = DateTime.Now.AddDays(-10)};
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.IsFalse(_test.IsFullPriceStable(item));
        }

        [Test]
        public void Check_ValidPromotionPriceChangeIsRedPencilItemtion_True()
        {
            var item = new RedPencilItem(100.00m, _makeStableDate);
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.IsTrue(item.IsRedPencilPromo);
        }
        
        [Test]
        public void Check_InValidPromotionPriceChangeIsRedPencilItemtion_False()
        {
            var item = new RedPencilItem(100.00m, _makeStableDate);
            _test.ChangePromotionPrice(item, 10.00m);
            TextOutput(item);

            Assert.IsFalse(item.IsRedPencilPromo);
        }

        [Test]
        public void Check_PromotionLastsThrityDays_True()
        {
            var item = new RedPencilItem(100.00m, _makeStableDate);
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.AreEqual(30, (item.PromotionEndDate.Value - item.PromotionStartDate.Value).TotalDays);
        }

        [Test]
        public void Check_ValidPromotionPriceReductionChangeDoesNotExtendPromotion_True()
        {
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 90.00m);
            var initalEndDate = item.PromotionEndDate.Value;
            TextOutput(item);

            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.AreEqual(initalEndDate, item.PromotionEndDate);
        }

        [Test]
        public void Check_ValidPromotionPriceIncreaseEndsPromotion_True()
        {
            var item = new RedPencilItem(100.00m);
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            _test.ChangePromotionPrice(item, 90.00m);
            TextOutput(item);

            Assert.IsFalse(item.IsRedPencilPromo);
        }

        [Test]
        public void Check_InValidPromotionPriceReductionEndsPromotion_True()
        {
            var item = new RedPencilItem(100.00m, _makeStableDate);
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.IsTrue(item.IsRedPencilPromo);

            _test.ChangePromotionPrice(item, 69.99m);
            TextOutput(item);

            Assert.IsFalse(item.IsRedPencilPromo);
        }

        [Test]
        public void Check_StartAnotherRedPencilItemtionAfterCurrentEnds_True()
        {
            var item = new RedPencilItem(100.00m, _makeStableDate);
            _test.ChangePromotionPrice(item, 80.00m);
            TextOutput(item);

            Assert.IsTrue(item.IsRedPencilPromo);

            item.PromotionEndDate = DateTime.Now.AddDays(-10);
            _test.ChangePromotionPrice(item, 79.00m);
            TextOutput(item);

            Assert.IsTrue(item.IsRedPencilPromo);
        }

        // Display Text Output in NUnit GUI
        public void TextOutput(RedPencilItem item)
        {
            Console.WriteLine("\nFullPrice:" + item.FullPrice);
            Console.WriteLine("PromotionPrice:" + item.PromotionPrice);
            Console.WriteLine("IsRedPencilPromo:" + item.IsRedPencilPromo);
            Console.WriteLine("priceReductionPercent:" + _test.PriceReductionPercent(item));
            Console.WriteLine("isFullPriceStable:" + _test.IsFullPriceStable(item));
            Console.WriteLine("FullPriceUpdateDate:" + item.FullPriceUpdateDate);
            Console.WriteLine("PromotionStartDate:" + item.PromotionStartDate);
            Console.WriteLine("PromotionEndDate:" + item.PromotionEndDate);
        }
    }
}
