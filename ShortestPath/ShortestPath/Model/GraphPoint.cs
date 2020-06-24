using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath.Model
{
    public class GraphPoint
    {
        /// <summary>
        /// Graf noktasi Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Graf noktasi X pozisyonu
        /// </summary>
        public int PointX { get; set; }

        /// <summary>
        /// Graf noktasi Y pozisyonu
        /// </summary>
        public int PointY { get; set; }

        /// <summary>
        /// Graf noktasi adi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Graf noktasi baglantilari
        /// </summary>
        public List<GraphConnection> Connections { get; set; }
    }
}
