using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace _Prototype.Navigation
{
    public class PathfindingStats
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public long ElapsedTime
        {
            get { return _stopwatch.ElapsedMilliseconds; }
        }

        public int Cycles { get; set; }
        public int SkippedNodes { get; set; }
        public int NodesWithHighScores { get; set; }
        public int TotalNodesExamined { get; set; }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop(bool success)
        {
            _stopwatch.Stop();
            var message = string.Format("Path found : {0}, Time Elapsed : {1}ms, Cycles : {2}, TotalNodes : {3}, Skipped Nodes : {4} NodesWithHighScores: {5}",
                success,
                _stopwatch.ElapsedMilliseconds,
                Cycles,
                TotalNodesExamined, 
                SkippedNodes,
                NodesWithHighScores);
            Debug.Log(message);
        }
    }
}