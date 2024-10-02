using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDemo
{
    public class AuctionModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public string LotCount { get; set; }
        public string StartDate { get; set; }
        public string StartMonth { get; set; }
        public string StartYear { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndMonth { get; set; }
        public string EndYear { get; set; }
        public string EndTime { get; set; }
        public string Location { get; set; }
    }
}
