using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class HorizontalMetricsTable : Table
    {
        public IList<LongHorizontalMetric> HorizontalMetrics { get; set; }
        public IList<Int16> LeftSideBearing { get; set; }

        public HorizontalMetricsTable()
        {
            HorizontalMetrics = new List<LongHorizontalMetric>();
            LeftSideBearing = new List<Int16>();
        }
    }
}
