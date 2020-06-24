using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath.Model
{
    public class GraphConnection
    {
        /// <summary>
        /// Dugum baglanti maliyeti
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Dugum baslangic noktasi id
        /// </summary>
        public int PrevGraphPointId { get; set; }

        /// <summary>
        /// Dugum bitis noktasi id
        /// </summary>
        public int NextGraphPointId { get; set; }
    }
}
