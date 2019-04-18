using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netflix_Api.Models
{
    public class RootObject
    {
        public string CodeName { get; set; }
        public List<ViewedItem> ViewedItems { get; set; }
        public int VhSize { get; set; }
        public int Trkid { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string Tz { get; set; }
    }
}