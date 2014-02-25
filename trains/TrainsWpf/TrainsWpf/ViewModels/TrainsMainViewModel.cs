using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Input;
using GraphX;
using GraphX.GraphSharp.Algorithms.Layout.Simple.FDP;
using GraphX.GraphSharp.Algorithms.OverlapRemoval;
using QuickGraph;
using QuickGraph.Algorithms;
using TrainsWpf.Annotations;
using TrainsWpf.Models;
using TrainsWpf.Commands;


namespace TrainsWpf.ViewModels
{
    public class TrainsMainViewModel : INotifyPropertyChanged
    {
        private TrainGraph _trainGraph;
        private GxLogicCoreTrains _logicCoreTrains;

        #region Properties

        public ICommand LoadGraphFromFileCommand { get; private set; }
        public ICommand GetDistanceForRouteCommand { get; private set; }
        public ICommand GetShortestRouteCommand { get; private set; }
        public GraphAreaTrains GraphArea { get; set; }
        
        private string _distanceForRoute;
        public string DistanceForRoute {
            get { return _distanceForRoute; }
            set 
            { 
                _distanceForRoute = value;
                OnPropertyChanged("DistanceForRoute");
            }
        }

        private string _shortestRoute;
        public string ShortestRoute 
        {
            get { return _shortestRoute; }
            set 
            { 
                _shortestRoute = value;
                OnPropertyChanged("ShortestRoute");
            }
        }

        private double _shortestRouteDistance;
        public double ShortestRouteDistance
        {
            get { return _shortestRouteDistance; }
            set
            {
                _shortestRouteDistance = value;
                OnPropertyChanged("ShortestRouteDistance");
            }
        }

        private string _routeStartName;
        public string RouteStartName
        {
            get { return _routeStartName; }
            set
            {
                _routeStartName = value;
                OnPropertyChanged("ShortestRoute");
            }
        }

        private string _routeEndName;
        public string RouteEndName
        {
            get { return _routeEndName; }
            set
            {
                _routeEndName = value;
                OnPropertyChanged("ShortestRoute");
            }
        }
        #endregion

        public TrainsMainViewModel(GraphAreaTrains graphArea)
        {
            _trainGraph = new TrainGraph();
            LoadGraphFromFileCommand = new LoadGraphFromFileCommand(this);
            GetDistanceForRouteCommand = new GetDistanceForRouteCommand(this);
            GetShortestRouteCommand = new GetShortestRouteCommand(this);
            SetLogicCoreProperties();
            GraphArea = graphArea;
            GraphArea.LogicCore = _logicCoreTrains;
        }

        public TrainGraph GetTrainGraph()
        {
            return _trainGraph;
        }

        public void LoadGraphFromFile()
        {
            
            var openDialog = new OpenFileDialog();
            var okClicked = openDialog.ShowDialog();
            if (okClicked != true) return;
            
            var fileStream = openDialog.OpenFile();
            _trainGraph = new TrainGraph();

            using (var reader = new System.IO.StreamReader(fileStream))
            {
                // Read the first line from the file and write it the textbox.
                //tbResults.Text = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var routes = line.Split(',');
                    foreach (var route in routes)
                    {
                        AddRoute(route.Trim(' '));
                    }
                }
            }
            fileStream.Close();
            DrawUpdatedGraph();
        }

        private void DrawUpdatedGraph()
        {
            _logicCoreTrains.Graph = _trainGraph;
            GraphArea.ShowAllEdgesLabels(true);
            GraphArea.GenerateGraph(true);
        }

        private void AddRoute(string route)
        {
            var sourceName = route.Substring(0, 1);
            var destinationName = route.Substring(1, 1);
            var distance = Convert.ToInt32(route.Substring(2, 1));

            DataVertex sourceVertex;
            if (_trainGraph.Vertices.Count(x => x.Text == sourceName) == 0)
            {
                sourceVertex = new DataVertex(sourceName) {ID = _trainGraph.Vertices.Count() + 1};
                _trainGraph.AddVertex(sourceVertex);
            }
            else
            {
                sourceVertex = _trainGraph.Vertices.First(x => x.Text == sourceName);
            }

            DataVertex destinationVertex;
            if (_trainGraph.Vertices.Count(x => x.Text == destinationName) == 0)
            {
                destinationVertex = new DataVertex(destinationName) {ID = _trainGraph.Vertices.Count() + 1};
                _trainGraph.AddVertex(destinationVertex);
            }
            else
            {
                destinationVertex = _trainGraph.Vertices.First(x => x.Text == destinationName);
            }

            if (_trainGraph.Edges.Count(x => x.Source.Text == sourceName && x.Target.Text == destinationName) == 0)
            {
                _trainGraph.AddEdge(new DataEdge(sourceVertex, destinationVertex, distance) 
                {ID = _trainGraph.Edges.Count() + 1, Text = distance.ToString(CultureInfo.InvariantCulture)});
            }

        }

        public void GetTotalDistanceForRoute(string route)
        {
            var distance = CalculateDistanceForRoute(route);
            DistanceForRoute = Math.Abs(distance - -1) < 0.00000001 ? "NO SUCH ROUTE" : distance.ToString(CultureInfo.InvariantCulture);
        }

        //returns -1 if route is not valid
        private double CalculateDistanceForRoute(string route)
        {
            var totalDistance = 0.0;
            if (route.Length < 2)
                return -1;
            var sourceName = route.Substring(0, 1);
            var destinationName = route.Substring(1, 1);
            if (_trainGraph.Edges.Count(x => x.Source.Text == sourceName && x.Target.Text == destinationName) == 0)
                return -1;
            var edge = _trainGraph.Edges.First(x => x.Source.Text == sourceName && x.Target.Text == destinationName);
            totalDistance = edge.Weight;
            if (route.Length > 2)
            {
                for (var i = 2; i < route.Length; i++)
                {
                    sourceName = destinationName;
                    destinationName = route.Substring(i, 1);
                    if (_trainGraph.Edges.Count(x => x.Source.Text == sourceName && x.Target.Text == destinationName) ==
                        0)
                        return -1;
                    edge = _trainGraph.Edges.First(x => x.Source.Text == sourceName && x.Target.Text == destinationName);
                    totalDistance += edge.Weight;
                }
            }
            return totalDistance;
        }

        public void FindShortestRoute()
        {
            Func<DataEdge, double> edgeCost = e => e.Weight;
            var startVertex = _trainGraph.Vertices.First(x => x.Text == _routeStartName);
            var tryGetPaths = _trainGraph.ShortestPathsDijkstra(edgeCost, startVertex);
            //var tryGetPaths = _trainGraph.ShortestPathsDag(edgeCost, startVertex);
            var destinationVertex = _trainGraph.Vertices.First(x => x.Text == _routeEndName);
            IEnumerable<DataEdge> route;

            if (tryGetPaths(destinationVertex, out route))
            {
                var dataEdges = route as IList<DataEdge> ?? route.ToList();
                var routeDesc = dataEdges.ElementAt(0).Source.Text;;
                double routeDistance = 0;
                
                foreach (var dataEdge in dataEdges)
                {
                    routeDesc += string.Concat("-", dataEdge.Target.Text);
                    routeDistance += dataEdge.Weight;
                }
                ShortestRoute = routeDesc;
                ShortestRouteDistance = routeDistance;
            }
            else
            {
                ShortestRoute = "NO ROUTE EXISTS";
                ShortestRouteDistance = 0;
            }
        }

        public IGXLogicCore<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>> LogicCoreTrains
        {
            get { return _logicCoreTrains; }
        }

        private void SetLogicCoreProperties()
        {
            _logicCoreTrains = new GxLogicCoreTrains();
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            _logicCoreTrains.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set optional parameters using AlgorithmFactory
            _logicCoreTrains.DefaultLayoutAlgorithmParams =
                               _logicCoreTrains.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)_logicCoreTrains.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            _logicCoreTrains.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Setup optional params
            _logicCoreTrains.DefaultOverlapRemovalAlgorithmParams =
                              _logicCoreTrains.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            ((OverlapRemovalParameters)_logicCoreTrains.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 50;
            ((OverlapRemovalParameters)_logicCoreTrains.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 50;

            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            _logicCoreTrains.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            _logicCoreTrains.AsyncAlgorithmCompute = false;

            _logicCoreTrains.EnableParallelEdges = true;

            _logicCoreTrains.Graph = _trainGraph;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
