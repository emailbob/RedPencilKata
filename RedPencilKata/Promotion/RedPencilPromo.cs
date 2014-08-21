using System;

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
                if (IsDiscountInRange(item) && IsFullPriceStable(item))
                {
                    item = StartPromotion(item);
                }
                else
                {
                    item = EndPromotion(item);
                }
            }
            else
            {
                // allow further reduction in prices during promotion if rules pass
                if (newPrice <= item.PromotionPrice)
                {
                    item.PromotionPrice = newPrice;

                    if (!IsDiscountInRange(item) || !IsFullPriceStable(item))
                        item = EndPromotion(item);
                }
                else
                {
                    item = EndPromotion(item);
                }
            }

            return item;
        }

        public double PriceReductionPercent(RedPencilItem item)
        {
            return (double)(((item.FullPrice - item.PromotionPrice) / item.FullPrice) * 100);
        }

        public bool PassLowerPercent(RedPencilItem item)
        {
            return (PriceReductionPercent(item) >= 5);
        }

        public bool PassUpperPercent(RedPencilItem item)
        {
            return (PriceReductionPercent(item) <= 30);
        }

        public bool IsDiscountInRange(RedPencilItem item)
        {
            return (PassLowerPercent(item) && PassUpperPercent(item));
        }

        public bool IsFullPriceStable(RedPencilItem item)
        {
            return (DateTime.Now - item.FullPriceUpdateDate).TotalDays >= 30;
        }

        private static RedPencilItem StartPromotion(RedPencilItem item)
        {
            item.IsRedPencilPromo = true;
            item.PromotionStartDate = DateTime.Now;
            item.PromotionEndDate = item.PromotionStartDate.Value.AddDays(30);

            return item;
        }

        private static RedPencilItem EndPromotion(RedPencilItem item)
        {
            item.IsRedPencilPromo = false;
            item.PromotionEndDate = DateTime.Now;

            return item;
        }
    }
    
}