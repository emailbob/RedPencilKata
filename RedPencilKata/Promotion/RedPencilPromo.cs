using System;
using System.Text;

namespace Promotion
{
    public class RedPencilPromotion
    {
        public RedPencilItem ChangePromotionPrice(RedPencilItem item, decimal newPrice)
        {
            // end expired promotions
            if (item.PromotionEndDate <= DateTime.Now)
                item.IsRedPencilPromo = false;

            if (!item.IsRedPencilPromo)
            {
                item.PromotionPrice = newPrice;

                // start promotion if rules pass
                if (isDiscountInRange(item) && isFullPriceStable(item))
                {
                    item = startPromotion(item);
                }
                else
                {
                    item = endPromotion(item);
                }
            }
            else
            {
                // allow further reduction in prices during promotion if rules pass
                if (newPrice <= item.PromotionPrice)
                {
                    item.PromotionPrice = newPrice;

                    if (!isDiscountInRange(item) || !isFullPriceStable(item))
                        item = endPromotion(item);
                }
                else
                {
                    item = endPromotion(item);
                }
            }

            return item;
        }

        public double priceReductionPercent(RedPencilItem item)
        {
            return (double)(((item.FullPrice - item.PromotionPrice) / item.FullPrice) * 100);
        }

        public bool passLowerPercent(RedPencilItem item)
        {
            return (priceReductionPercent(item) >= 5);
        }

        public bool passUpperPercent(RedPencilItem item)
        {
            return (priceReductionPercent(item) <= 30);
        }

        public bool isDiscountInRange(RedPencilItem item)
        {
            return (passLowerPercent(item) && passUpperPercent(item));
        }

        public bool isFullPriceStable(RedPencilItem item)
        {
            return (DateTime.Now - item.FullPriceUpdateDate).TotalDays >= 30;
        }

        private RedPencilItem startPromotion(RedPencilItem item)
        {
            item.IsRedPencilPromo = true;
            item.PromotionStartDate = DateTime.Now;
            item.PromotionEndDate = item.PromotionStartDate.Value.AddDays(30);

            return item;
        }

        private RedPencilItem endPromotion(RedPencilItem item)
        {
            item.IsRedPencilPromo = false;
            item.PromotionEndDate = DateTime.Now;

            return item;
        }
    }
    
}