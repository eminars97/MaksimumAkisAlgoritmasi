using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath.Model
{
    public class EdgeConnection
    {
        #region Constructors

        public EdgeConnection(int id)
        {
            this.Id = id;
            this.BeginConnectionId = -1; //Baslangic degeri atanir.
            this.EndConnectionId = -1; //Baslangic degeri atanir.
            this.Path = 0; // Baslangic yolu 0 olarak belirlenir.
            this.Cost = 0; //Baslangic maliyeti 0 olarak belirlenir.
            this.PrevFlow = 0; //Onceki akis no
        }

        #endregion

        public int Id { get; set; } //Dugum id
        public int BeginConnectionId { get; set; } //Baslangic dugum id
        public int EndConnectionId { get; set; } //Bitis dugum id
        public int Path { get; set; } //Yol
        public int Cost { get; set; } //Maliyetini belirler. Kapasite.
        public int PrevFlow { get; set; } //Var olan akis
    }
}
