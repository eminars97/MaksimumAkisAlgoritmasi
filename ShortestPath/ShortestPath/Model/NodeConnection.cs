using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath.Model
{
    public class NodeConnection
    {
        #region Constructors

        public NodeConnection(int id)
        {
            this.Id = id;
            this.GraphPrevNodes = new List<int>();
            this.GraphNextNodes = new List<int>();
        }

        #endregion

        public int Id { get; set; }
        public List<int> GraphNodes { get; set; } // predecessors (String)
        public List<int> GraphNextNodes { get; set; } // forward edges -node is start vertex (Integer)
        public List<int> GraphPrevNodes { get; set; } // backward edges -node is end vertex (Integer)
        public List<int> GraphConnections { get; set; } //Graf dugumlerinin baglanti listesi
    }
}
