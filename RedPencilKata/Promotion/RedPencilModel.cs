using System;

namespace Promotion
{
    public class RedPencilItem 
    {
        private decimal _fullPrice;

        public RedPencilItem(decimal fullPrice)
        {
            FullPrice = fullPrice;
            FullPriceUpdateDate = DateTime.Now;
        }

        public RedPencilItem(decimal fullPrice, DateTime fullPriceUpdateDate)
        {
            FullPrice = fullPrice;
            FullPriceUpdateDate = fullPriceUpdateDate;
        }

        public decimal FullPrice
        {
            get { return _fullPrice; }
            set { _fullPrice = value; FullPriceUpdateDate = DateTime.Now; }
        }

        public decimal PromotionPrice { get; set; }
        public DateTime FullPriceUpdateDate { get; set; }
        public DateTime? PromotionStartDate { get; set; }
        public DateTime? PromotionEndDate { get; set; }
        public bool IsRedPencilPromo { get; set; }
    }
}
