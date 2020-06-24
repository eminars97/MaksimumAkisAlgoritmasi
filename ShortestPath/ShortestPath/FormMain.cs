using ShortestPath.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ShortestPath
{
    public partial class FormMain : Form
    {
        #region properties

        bool draw;
        Graphics graphic;
        Pen pen;
        int graphLongestCapacity = int.MinValue; //++ maksimum kapasite
        int graphCurrentCapacity = 0; //++ Graf yedek kapasite
        int graphTotalCost = 0; //++ Graf toplam kapasite
        int graphEndConnection; //Hedef dugumu tutar.
        int selectedPrevNodeId;
        int prevX;
        int prevY;
        int currentPrevX;
        int currentPrevY;
        int totalMaxCost = 0;
        int graphCurrentTotalCost = 0; //++ Graf toplam yedek kapasite
        int graphMinimumCapacity; //++ Her dugumun minimum kapasitesini tutar.
        int graphEndConnectionId;
        bool isMouseOnTheRectangle;
        public List<GraphPoint> graphPoints; //Graf dugum listesi
        EdgeConnection[] graphEdges; //Graf kenar listesi
        int[] graphVisits; //++ Graf uzerinde ziyaret edilen dugumlerin listesi
        int[] graphCurrentVisits; //++ Graf uzerinde ziyaret edilen dugumlerin yedek listesi
        int[] graphLongestPath; //++ Graf uzerinde en uzun yol listesi
        NodeConnection[] nodeConnections; //Dugum baglanti listesi
        int[,] mainGraph;


        //Kaynaktan hedefe yol bilgisini doner. Yol var ise true, yok ise false
        bool BreadthFirstSearch(int[,] rGraph, int[] currentPath)
        {
            bool[] visitList = new bool[graphPoints.Count]; //gezilen dugumler
            for (int i = 0; i < graphPoints.Count; ++i)
            {
                visitList[i] = false;
            }

            List<int> pointList = new List<int>(); //Dugumler listesi
            pointList.Add(comboBoxBegin.SelectedIndex);
            visitList[comboBoxBegin.SelectedIndex] = true;
            currentPath[comboBoxBegin.SelectedIndex] = -1;

            while (pointList.Count != 0)
            {
                int currentEdge = pointList[0];
                pointList.RemoveAt(0);

                for (int vertex = 0; vertex < graphPoints.Count; vertex++)
                {
                    if (visitList[vertex] == false && rGraph[currentEdge, vertex] > 0)
                    {
                        pointList.Add(vertex);
                        currentPath[vertex] = currentEdge;
                        visitList[vertex] = true;
                    }
                }
            }
            bool result = (visitList[comboBoxEnd.SelectedIndex] == true); //Kaynaktan hedefe ulasildiginda true doner. 
            return (result);
        }

        //FordFulkerson algoritmasi ile max akis bulunur.
        void GetMaxFlowValue()
        {
            int currentMax = int.MaxValue;
            int edge;
            totalMaxCost = 0;
            int vertex;
            int[] currentGraph = new int[graphPoints.Count]; //BFS dugum-kapasite yol bilgisi
            int k = 0;
            foreach (var item in graphPoints)
            {
                for (int i = 0; i < graphPoints.Count; i++)
                {
                    if (item.Connections.Where(x => x.NextGraphPointId == i).FirstOrDefault() != null)
                        mainGraph[k, i] = item.Connections.Where(x => x.NextGraphPointId == i).FirstOrDefault().Cost;
                    else
                        mainGraph[k, i] = 0;
                }
                k++;
            }
            //Dugumden diger dugumlere yol var ise devam edilir.
            while (BreadthFirstSearch(mainGraph, currentGraph))
            {
                //BFS algoritması ile minimum kenar-dugum kapasitesi bulunur.
                for (vertex = comboBoxEnd.SelectedIndex; vertex != comboBoxBegin.SelectedIndex; vertex = currentGraph[vertex])
                {
                    edge = currentGraph[vertex];
                    currentMax = Math.Min(currentMax, mainGraph[edge, vertex]);
                }

                //Kenar dugumlerin kapasiteleri guncellenir.
                for (vertex = comboBoxEnd.SelectedIndex; vertex != comboBoxBegin.SelectedIndex; vertex = currentGraph[vertex])
                {
                    edge = currentGraph[vertex];
                    mainGraph[edge, vertex] -= currentMax;
                    mainGraph[vertex, edge] += currentMax;
                }

                //Maksimum akis toplanir.
                totalMaxCost += currentMax;
            }
        }

        #endregion

        #region Constructor

        public FormMain()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Form yuklenirken default degerler atanir.
            draw = false; //resim cizilecek mi
            graphic = CreateGraphics(); //cizim alani
            pen = new Pen(Color.Black, 2); //kalem
            graphPoints = new List<GraphPoint>(); //Graf nokta listesi
            isMouseOnTheRectangle = true; //Mouse cizim karesinde mi?
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            //ilk iki dugum olusturulurken
            if (graphPoints.Count == 0)
            {
                //Mouse koordinati resim cizim alaninda ise
                //Cizim alani = 500*500
                if (e.X <= 500
                    && e.X >= 0
                    && e.Y <= 500
                    && e.Y >= 0)
                {
                    draw = true; //Resim cizilebilir mi?
                    prevX = e.X; //Mouse x noktasi
                    prevY = e.Y; //Mouse y noktasi
                    currentPrevX = e.X; //Mouse x noktasi
                    currentPrevY = e.Y; //Mouse y noktasi
                }
            }
            //Dugum var iken
            else
            {
                if (graphPoints.Count > 0)
                {
                    foreach (var item in graphPoints)
                    {
                        //Mouse koordinati herhangi bir dugumun uzerinde mi?
                        if (e.X <= item.PointX + 6
                            && e.X >= item.PointX - 6
                            && e.Y <= item.PointY + 6
                            && e.Y >= item.PointY - 6)
                        {
                            isMouseOnTheRectangle = true; //Mouse koordinati herhangi bir dugumun ustunde mi?
                            selectedPrevNodeId = item.Id; //Kaynak dugum
                            draw = true; //Resim cizilebilir mi?
                            prevX = e.X; //Mouse x noktasi
                            prevY = e.Y; //Mouse y noktasi
                            currentPrevX = e.X; //Mouse x noktasi
                            currentPrevY = e.Y; //Mouse y noktasi
                        }
                    }
                }
            }
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            //Resim cizilebilir mi?
            if (draw)
            {
                //Ekranda hicbir noktanin eklenmedigi
                if (graphPoints.Count == 0)
                {
                    //Graf noktalari olustur
                    GraphPoint graphPointBegin = new GraphPoint();
                    GraphPoint graphPointEnd = new GraphPoint();

                    //Graf baslangic noktasi ozellikleri. Ilk dugum
                    graphPointBegin.Id = graphPoints.Count; //Birinci dugum id
                    graphPointBegin.Name = "A" + graphPoints.Count.ToString(); //Birinci dugum adi
                    graphPointBegin.PointX = currentPrevX; //Birinci dugum x koordinati
                    graphPointBegin.PointY = currentPrevY; //Birinci dugum y koordinati
                    graphPointBegin.Connections = new List<GraphConnection>(); //Birinci dugum baglanti listesi
                    graphPoints.Add(graphPointBegin);

                    //Graf bitis noktasi ozellikleri. Ikinci dugum
                    graphPointEnd.Id = graphPoints.Count; //Ikinci dugum id
                    graphPointEnd.Name = "A" + graphPoints.Count.ToString(); //Ikinci dugum adi
                    graphPointEnd.PointX = prevX; //Ikinci dugum x koordinati
                    graphPointEnd.PointY = prevY; //Ikinci dugum x koordinati
                    graphPointEnd.Connections = new List<GraphConnection>(); //Ikinci dugum baglanti listesi
                    graphPoints.Add(graphPointEnd);


                    comboBoxBegin.Items.Add(graphPointBegin.Name); //baslangic combobox listesine birinci dugum eklenir
                    comboBoxEnd.Items.Add(graphPointBegin.Name); //baslangic combobox listesine birinci dugum eklenir
                    comboBoxBegin.Items.Add(graphPointEnd.Name); //bitis combobox listesine ikinci dugum eklenir
                    comboBoxEnd.Items.Add(graphPointEnd.Name); //bitis combobox listesine ikinci dugum eklenir

                    //Dugumun basi ve sonuna kareler eklenir. Sonraki eklenecek dugumlerde secimin kolay olmasi icin.
                    Brush brush = new SolidBrush(Color.Red);
                    graphic.FillRectangle(brush, graphPointBegin.PointX - 6, graphPointBegin.PointY - 6, 12, 12); //Baslangic dugum kirmizi karesi cizilir.
                    graphic.FillRectangle(brush, graphPointEnd.PointX - 6, graphPointEnd.PointY - 6, 12, 12); //bitis dugum kirmizi karesi cizilir.
                    brush.Dispose(); //brush bellekten silinir.

                    bool statePromt = false; //Kapasite sayisi sayi validasyonuna uygun mu?
                    string promptValue = ""; //Kapasite alinir.
                    while (!statePromt)
                    {
                        //Kapasite alinir.
                        promptValue = ShowDialog("Test", "123");
                        if (string.IsNullOrEmpty(promptValue))
                            MessageBox.Show("Lütfen kapasiteyi giriniz!");
                        //Sayi validasyonu
                        else if (!promptValue.Replace("-","").All(char.IsDigit))
                            MessageBox.Show("Lütfen sadece sayi giriniz!");
                        //6 haneden fazla girilemez.
                        else if (promptValue.Length > 6)
                            MessageBox.Show("Lütfen en fazla alti haneli sayi giriniz!");
                        else
                            statePromt = true;
                    }

                    //Iki graf dugumu arasindaki kapasite alinir.
                    GraphConnection graphConnectionBegin = new GraphConnection();
                    graphConnectionBegin.NextGraphPointId = graphPointEnd.Id;
                    graphConnectionBegin.Cost = Convert.ToInt32(promptValue);
                    graphPointBegin.Connections.Add(graphConnectionBegin);

                    //Kapasite listesi
                    listBoxCost.Items.Add(GetCostDescription(graphPointBegin, graphPointEnd, graphConnectionBegin.Cost));
                }
                //Ekranda en az bir dugumun oldugu durum
                else
                {
                    //Graf noktalari olustur
                    GraphPoint graphPoint = new GraphPoint();
                    graphPoint.Id = -1;

                    int nextPointId = -1;
                    foreach (var item in graphPoints)
                    {
                        //Mouse koordinatlari herhangi bir hedef dugumun uzerinde mi?
                        if (prevX >= item.PointX - 6
                            && prevX <= item.PointX + 6
                            && prevY >= item.PointY - 6
                            && prevY <= item.PointY + 6)
                        {
                            graphPoint = item; //Hedef dugum
                            break;
                        }
                    }
                    //Hedef dugum baslangic dugumunun kendisi olamaz.
                    if(graphPoint.Id != selectedPrevNodeId)
                    {
                        if (graphPoint.Id == -1)
                        {
                            //Graf baslangic noktasi ozellikleri.
                            graphPoint.Id = graphPoints.Count; //Dugum id
                            graphPoint.Name = "A" + graphPoints.Count.ToString(); //Dugum adi
                            graphPoint.PointX = prevX; //Dugum x noktasi
                            graphPoint.PointY = prevY; //Dugum y noktasi
                            graphPoint.Connections = new List<GraphConnection>(); //Dugum baglantilari
                            graphPoints.Add(graphPoint);
                            comboBoxBegin.Items.Add(graphPoint.Name); //Baslangic combobox listesine yeni dugum eklenir.
                            comboBoxEnd.Items.Add(graphPoint.Name); //Bitis combobox listesine yeni dugum eklenir.
                        }

                        //Dugumun basi ve sonuna kirmizi kare eklenir. Sonraki eklenecek dugumlerde secimin kolay olmasi icin.
                        Brush brush = new SolidBrush(Color.Red);
                        graphic.FillRectangle(brush, graphPoints[selectedPrevNodeId].PointX - 6, graphPoints[selectedPrevNodeId].PointY - 6, 12, 12); //Baslangic dugumune kirmizi kare eklenir.
                        graphic.FillRectangle(brush, graphPoint.PointX - 6, graphPoint.PointY - 6, 12, 12); //Bitis dugumune kirmizi kare eklenir.
                        brush.Dispose(); //brush nesnesi bellekten silinir.

                        bool statePromt = false; //Kapasite sayi validasyonuna uyuyor mu
                        string promptValue = ""; //kapasite
                        while (!statePromt)
                        {
                            //Kapasite alinir.
                            promptValue = ShowDialog("Test", "123");
                            if (string.IsNullOrEmpty(promptValue))
                                MessageBox.Show("Lütfen maliyeti giriniz!");
                            else if (!promptValue.Replace("-","").All(char.IsDigit))
                                MessageBox.Show("Lütfen sadece sayi giriniz!");
                            else if (promptValue.Length > 6)
                                MessageBox.Show("Lütfen en fazla alti haaneli sayi giriniz!");
                            else
                                statePromt = true;
                        }

                        //Graf dugumu kapasitesi alinir.
                        GraphConnection graphConnection = new GraphConnection(); //
                        graphConnection.NextGraphPointId = graphPoint.Id; //Dugum bitis noktasi
                        graphConnection.Cost = Convert.ToInt32(promptValue); //Baglanti kapasitesi
                        graphPoints[selectedPrevNodeId].Connections.Add(graphConnection);

                        //kapasite listesine dugum eklenir.
                        listBoxCost.Items.Add(GetCostDescription(graphPoints[selectedPrevNodeId], graphPoint, graphConnection.Cost));
                    }
                    else
                    {
                        MessageBox.Show("Dugum kendisine gelemez!");
                    }
                }
                isMouseOnTheRectangle = false; //Mouse koordinatlari cizim alaninda mi?
            }
            draw = false; //Cizim yapilamaz. Mouse cizim alani disinda.
            WriteNodesName(); //Dugumlerin isimleri yazilir. A0, A1 ...
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            //Sinirlanan alan kontrolu
            if (e.X < 500 && e.Y < 500)
            {
                //Cizilebilir mi
                if (draw && isMouseOnTheRectangle)
                {
                    //Cizgi ciz.
                    graphic.DrawLine(pen, prevX, prevY, e.X, e.Y);
                    prevX = e.X; //Onceki dugum x noktasi
                    prevY = e.Y; //Onceki dugum y noktasi
                }
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Form kapanirken grafik ve kalem degiskeni bellekten temizlenir.
            graphic.Dispose();
            pen.Dispose();
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            //Baslangic karesi grafigi.
            Graphics graphicRectangle = CreateGraphics();
            //Resim alani karesi ozellikleri. 500*500
            Rectangle rect = new Rectangle();
            rect.X = 0;
            rect.Y = 0;
            rect.Width = 100;
            rect.Height = 100;
            Pen penRectangle = new Pen(Color.Black, 3);
            //Baslangic karesi cizilir.
            graphicRectangle.DrawRectangle(penRectangle, 0, 0, 500, 500);
            penRectangle.Dispose(); //penRectangle nesnesi bellekten silinir.
            graphicRectangle.Dispose(); //graphicRectangle nesnesi bellekten silinir.
        }

        //Havuzu en cabuk dolduracak yol belirlenir.
        private void btnShortestPath_Click(object sender, EventArgs e)
        {
            mainGraph = new int[graphPoints.Count, graphPoints.Count];
            GetMaxFlowValue();
            MessageBox.Show("Maksimum Akis : " + totalMaxCost);
            label4.Text = "Maksimum Akis : " + totalMaxCost;
        }

        //Havuzun dolmamasi icin minimum sayida kapatilacak vanalar belirlenir.
        private void btnMinCut_Click(object sender, EventArgs e)
        {
            //Baslangic ve bitis noktalari alinir.
            if (comboBoxBegin.SelectedIndex < 0
                || comboBoxEnd.SelectedIndex < 0)
            {
                MessageBox.Show("Başlangıç ve bitiş noktalarını seçiniz!");
                return;
            }
            else if (comboBoxBegin.SelectedIndex == comboBoxEnd.SelectedIndex)
            {
                MessageBox.Show("Başlangıç ve bitiş noktası aynı olamaz!");
                return;
            }
            
            string popResult = "Silinecek düğümler;" + Environment.NewLine;
            //Minimum sayida kapatilacak vanalar belirlenir.
            string result = FindMinimumCutFlow();
            MessageBox.Show(popResult + result);
        }

        #endregion

        #region Methods

        private string GetCostDescription(GraphPoint itemBegin, GraphPoint itemEnd, int cost)
        {
            string content = "";
            content += itemBegin.Name + " - ";
            content += itemEnd.Name + " arasi maliyet : ";
            content += cost;
            return content;
        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 240,
                Height = 120,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Maliyeti giriniz...",
                StartPosition = FormStartPosition.CenterScreen
            };
            TextBox textBox = new TextBox() { Left = 10, Top = 10, Width = 200 };
            Button confirmation = new Button() { Text = "Ok", Left = 110, Width = 100, Top = 40, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void WriteNodesName()
        {
            foreach(var item in graphPoints)
            {
                Label lblItem = new Label();
                lblItem.Width = 20;
                lblItem.Height = 20;
                lblItem.Text = item.Name;
                lblItem.Left = item.PointX - 30;
                lblItem.Top = item.PointY;
                this.Controls.Add(lblItem);          }
        }

        #endregion

        
        //Baslangic dugumunden hedef dugume yol var mi
        private bool HasPathFromSourceToDestination(int[,] currentArrayPoints, int[] parentArrayPoints)
        {
            bool result = false;
            //Dugumun daha once ziyaret edilip edilmedigini tutar.
            bool[] visitList = new bool[currentArrayPoints.Length];
            visitList[comboBoxBegin.SelectedIndex] = true;

            //Baslangic dugumunden ziyaret edilen noktalar kuyrugu 
            parentArrayPoints[comboBoxBegin.SelectedIndex] = -1;
            Queue<int> visitQueue = new Queue<int>();
            visitQueue.Enqueue(comboBoxBegin.SelectedIndex);
            
            // Standard BFS Loop      
            while (visitQueue.Count != 0)
            {
                int pointVertex = visitQueue.Dequeue(); //Kuyruktan alinan elemanin indeksi.
                for (int i = 0; i < currentArrayPoints.GetLength(0); i++)
                {
                    if (currentArrayPoints[pointVertex, i] > 0 && !visitList[i])
                    {
                        visitQueue.Enqueue(i); //Kuyruga atilir
                        visitList[i] = true;
                        parentArrayPoints[i] = pointVertex;
                    }
                }
            }
            //Baslangictan bitis dugumune erisildiginde true donulur.
            result = (visitList[comboBoxEnd.SelectedIndex] == true);
            return result;
        }

        //Baslangic dugumunden itibaren gidilebilen tum dugum listesini dolasir.
        private static void FindAllNodesFromBeginNode(bool[] isVisitList, int[,] currentArrayPoints, int indexBegin)
        {
            isVisitList[indexBegin] = true;
            for (int i = 0; i < currentArrayPoints.GetLength(0); i++)
            {
                if (currentArrayPoints[indexBegin, i] > 0 && !isVisitList[i])
                {
                    //Rekursif olarak cagrilir.
                    FindAllNodesFromBeginNode(isVisitList, currentArrayPoints, i);
                }
            }
        }

        // Prints the minimum s-t cut 
        private string FindMinimumCutFlow()
        {
            int[,] arrayGraphPoints = new int[graphPoints.Count, graphPoints.Count];
            for (int i = 0; i < graphPoints.Count; i++)
            {
                for (int j = 0; j < graphPoints.Count; j++)
                {
                    int res = 0;
                    foreach (var conn in graphPoints[i].Connections)
                    {
                        if (conn.NextGraphPointId == j)
                            res = conn.Cost;
                    }
                    arrayGraphPoints[i, j] = res;
                }
            }

            string result = ""; //sonuc
            int capacity; //Kapasite
            int vertex; //Kenar
 
            //Yedek graf noktalari atanir.
            int[,] currentGraphPoints = new int[arrayGraphPoints.Length, arrayGraphPoints.Length];
            for (int i = 0; i < arrayGraphPoints.GetLength(0); i++)
            {
                for (int j = 0; j < arrayGraphPoints.GetLength(1); j++)
                {
                    currentGraphPoints[i, j] = arrayGraphPoints[i, j];
                }
            }
            
            //Ana noktalar.
            int[] parentNodes = new int[arrayGraphPoints.Length];

            //Dugumler arasinda baglanti var ise - boru hatti var ise - devam et.
            while (HasPathFromSourceToDestination(currentGraphPoints, parentNodes))
            {
                //Minumum yol ve maksimum su akisi saglayan boru hatti bulunur.
                int counterMax = int.MaxValue; //Dugumun min kapasitesi.
                for (vertex = comboBoxEnd.SelectedIndex; vertex != comboBoxBegin.SelectedIndex; vertex = parentNodes[vertex])
                {
                    capacity = parentNodes[vertex];
                    counterMax = Math.Min(counterMax, currentGraphPoints[capacity, vertex]);
                }

                //Dugumlerin - borularin - kapasiteleri yerlestirilir
                for (vertex = comboBoxEnd.SelectedIndex; vertex != comboBoxBegin.SelectedIndex; vertex = parentNodes[vertex])
                {
                    capacity = parentNodes[vertex];
                    currentGraphPoints[capacity, vertex] = currentGraphPoints[capacity, vertex] - counterMax;
                    currentGraphPoints[vertex, capacity] = currentGraphPoints[vertex, capacity] + counterMax;
                }
            }

            // Secili dugumun maksimum kapasiteli olan baglantisi bulunur.  
            bool[] visitedPoints = new bool[arrayGraphPoints.Length];
            FindAllNodesFromBeginNode(visitedPoints, currentGraphPoints, comboBoxBegin.SelectedIndex);

            //Dugumlerin baglantili olan tum kenarlari dolasilir.
            for (int i = 0; i < arrayGraphPoints.GetLength(0); i++)
            {
                for (int j = 0; j < arrayGraphPoints.GetLength(1); j++)
                {
                    if (arrayGraphPoints[i, j] > 0 && visitedPoints[i] && !visitedPoints[j])
                    {
                        result += "A" + i + " - A" + j + Environment.NewLine;
                    }
                }
            }
            return result;
        }


        private void FindLongestPath()
        {
            //Dugum baglanti listesi alinir.
            List<GraphConnection> graphConnections = new List<GraphConnection>();
            foreach (var item in graphPoints)
            {
                foreach (var itemConnection in item.Connections)
                {
                    itemConnection.PrevGraphPointId = item.Id;
                    graphConnections.Add(itemConnection);
                }
            }

            nodeConnections = new NodeConnection[graphPoints.Count];
            graphEdges = new EdgeConnection[graphConnections.Count]; //Graf kenarlari listesi

            for (int j = 0; j < graphPoints.Count; j++)
                nodeConnections[j] = new NodeConnection(j);
            int i = 0;
            foreach (var item in graphConnections)
            {
                EdgeConnection edgeConnection = new EdgeConnection(i);
                edgeConnection.BeginConnectionId = item.PrevGraphPointId;
                edgeConnection.EndConnectionId = item.NextGraphPointId;
                edgeConnection.Cost = item.Cost;
                edgeConnection.PrevFlow = 0;
                graphEdges[i] = edgeConnection;

                nodeConnections[edgeConnection.BeginConnectionId].GraphNextNodes.Add(i);
                nodeConnections[edgeConnection.EndConnectionId].GraphPrevNodes.Add(i);
                i++;
            }

            graphVisits = new int[nodeConnections.Length];
            graphCurrentVisits = new int[nodeConnections.Length];
            graphLongestPath = new int[nodeConnections.Length];

            List<int> pathLongest = new List<int>();

            bool hasLongestPath = false;
            graphEndConnection = comboBoxEnd.SelectedIndex;
            graphLongestCapacity = int.MinValue;
            graphMinimumCapacity = 1;
            GetLongestPathWithBacktrack(comboBoxBegin.SelectedIndex);
            if (graphLongestCapacity == int.MinValue)
                hasLongestPath = false;
            else
                hasLongestPath = true;

            if (hasLongestPath)
            {
                String output = "A" + graphLongestPath[0];
                pathLongest.Add(graphLongestPath[0]);
                for (int j = 1; j < graphCurrentTotalCost; j++)
                {
                    pathLongest.Add(graphLongestPath[j]);
                    output = output + " -> A" + graphLongestPath[j];
                }
                output += "                  Max Flow : " + graphLongestCapacity;
                label4.Text = "Yol : " + output;
            }
            else
            {
                MessageBox.Show("A0 noktasindan A" + (graphPoints.Count - 1) + " noktasina yol bulunamadi.");
            }
            //En uzun yol bulundu ise gidilen yollar cizim alani bolgesinde gosterilir.
            if (pathLongest.Count > 0)
            {
                for (int k = 0; k < pathLongest.Count; k++)
                {
                    if (k + 1 < pathLongest.Count)
                    {
                        Pen pen2 = new Pen(Color.Green, 3);
                        Brush brush2 = new SolidBrush(Color.Green);
                        graphic.FillRectangle(brush2, graphPoints[pathLongest[k]].PointX - 6, graphPoints[pathLongest[k]].PointY - 6, 12, 12); //Yol baslangic dugumune kare eklenir.
                        graphic.FillRectangle(brush2, graphPoints[pathLongest[k + 1]].PointX - 6, graphPoints[pathLongest[k + 1]].PointY - 6, 12, 12); //Yol bitis dugumune kare eklenir.
                        graphic.DrawLine(pen2, graphPoints[pathLongest[k]].PointX, graphPoints[pathLongest[k]].PointY, graphPoints[pathLongest[k + 1]].PointX, graphPoints[pathLongest[k + 1]].PointY); //Iki dugum arasinda yol cizilir
                        brush2.Dispose(); //brush2 degiskeni bellekten silinir.
                        pen2.Dispose(); //pen2 degiskeni bellekten silinir.
                    }
                }
            }
        }

        private void GetLongestPathWithBacktrack(int nodeId)
        {
            graphVisits[nodeId] = 1;
            graphCurrentVisits[graphCurrentCapacity++] = nodeId;
            if (nodeId == graphEndConnection && graphCurrentCapacity >= graphMinimumCapacity)
            {
                if (graphTotalCost > graphLongestCapacity)
                {
                    for (int i = 0; i < graphCurrentCapacity; i++)
                        graphLongestPath[i] = graphCurrentVisits[i];
                    graphCurrentTotalCost = graphCurrentCapacity;
                    graphLongestCapacity = graphTotalCost;
                }
            }
            else
            {
                List<int> fors = nodeConnections[nodeId].GraphNextNodes;
                for (int i = 0; i < fors.Count(); i++)
                {
                    int edge_obj = (int)fors.ElementAt(i);
                    int edge = edge_obj;
                    if (graphVisits[graphEdges[edge].EndConnectionId] == 0)
                    {
                        graphTotalCost += graphEdges[edge].Cost;
                        GetLongestPathWithBacktrack(graphEdges[edge].EndConnectionId);
                        graphTotalCost -= graphEdges[edge].Cost;
                    }
                }
            }
            graphVisits[nodeId] = 0;
            graphCurrentCapacity--;
        }
    }
}
