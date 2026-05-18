using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusNet.Model
{
    public class Graph
    {
        
        private Dictionary<string, Vertex> _vertices;

        
        private Dictionary<string, List<string>> _adjacencyList;

        public Graph()
        {
            _vertices = new Dictionary<string, Vertex>();
            _adjacencyList = new Dictionary<string, List<string>>();
        }

        

        public IEnumerable<Vertex> GetAllVertices() => _vertices.Values;

        public IEnumerable<string> GetNeighbours(string id)
        {
            if (_adjacencyList.TryGetValue(id, out var list))
                return list;
            return Enumerable.Empty<string>();
        }

        public Vertex? GetVertex(string id)
        {
            _vertices.TryGetValue(id, out var v);
            return v;
        }

        public bool VertexExists(string id) => _vertices.ContainsKey(id);

        public bool EdgeExists(string fromId, string toId)
        {
            return _adjacencyList.TryGetValue(fromId, out var list) && list.Contains(toId);
        }

        // Returns a snapshot of the full adjacency list for the View
        public Dictionary<string, List<string>> GetAdjacencyList()
        {
            // Return a shallow copy so callers cannot mutate internal state
            var copy = new Dictionary<string, List<string>>();
            foreach (var kv in _adjacencyList)
                copy[kv.Key] = new List<string>(kv.Value);
            return copy;
        }


        public bool AddVertex(Vertex v)
        {
            if (_vertices.ContainsKey(v.Id)) return false;
            _vertices[v.Id] = v;
            _adjacencyList[v.Id] = new List<string>();
            return true;
        }

        public bool RemoveVertex(string id)
        {
            if (!_vertices.ContainsKey(id)) return false;

            // Remove outgoing edges
            _adjacencyList.Remove(id);
            _vertices.Remove(id);

            // Remove incoming edges from all other vertices
            foreach (var list in _adjacencyList.Values)
                list.Remove(id);

            return true;
        }

        public bool UpdateVertex(string id, string? newName = null, UserRole? newRole = null)
        {
            if (!_vertices.TryGetValue(id, out var v)) return false;
            if (newName != null) v.Name = newName;
            if (newRole.HasValue) v.Role = newRole.Value;
            return true;
        }

        // ─────────────────────────────────────────────
        //  CRUD – Edges
        // ─────────────────────────────────────────────

        public bool AddEdge(string fromId, string toId)
        {
            if (!_vertices.ContainsKey(fromId) || !_vertices.ContainsKey(toId)) return false;
            if (_adjacencyList[fromId].Contains(toId)) return false;  // no duplicates
            _adjacencyList[fromId].Add(toId);
            return true;
        }

        public bool RemoveEdge(string fromId, string toId)
        {
            if (!_adjacencyList.TryGetValue(fromId, out var list)) return false;
            return list.Remove(toId);
        }

      
        public List<string> BFS(string startId)
        {
            var visited = new List<string>();
            if (!_vertices.ContainsKey(startId)) return visited;

            var queue = new Queue<string>();
            var seen = new HashSet<string> { startId };
            queue.Enqueue(startId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                visited.Add(current);
                foreach (var neighbour in _adjacencyList[current])
                {
                    if (!seen.Contains(neighbour))
                    {
                        seen.Add(neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
            }
            return visited;
        }

        public bool CanReach(string sourceId, string targetId)
        {
            if (!_vertices.ContainsKey(sourceId) || !_vertices.ContainsKey(targetId))
                return false;
            return BFS(sourceId).Contains(targetId);
        }

        public class DfsResult
        {
            public List<string> DiscoveryOrder { get; } = new List<string>();
            public List<(string, string)> BackEdges { get; } = new List<(string, string)>();
            public bool HasCycle => BackEdges.Count > 0;
        }

        public DfsResult DFSFull()
        {
            var result = new DfsResult();
            var color = new Dictionary<string, int>(); // 0=white,1=gray,2=black
            foreach (var id in _vertices.Keys) color[id] = 0;

            foreach (var id in _vertices.Keys)
                if (color[id] == 0)
                    DFSVisit(id, color, result);

            return result;
        }

        private void DFSVisit(string id, Dictionary<string, int> color, DfsResult result)
        {
            color[id] = 1; // gray (in progress)
            result.DiscoveryOrder.Add(id);

            foreach (var neighbour in _adjacencyList[id])
            {
                if (color[neighbour] == 0)
                {
                    DFSVisit(neighbour, color, result);
                }
                else if (color[neighbour] == 1)
                {
                    // Back edge → cycle
                    result.BackEdges.Add((id, neighbour));
                }
            }
            color[id] = 2; // black (finished)
        }

        // ─────────────────────────────────────────────
        //  Metrics
        // ─────────────────────────────────────────────

        // In-degree: how many vertices point TO this vertex
        public Dictionary<string, int> GetInDegrees()
        {
            var inDeg = new Dictionary<string, int>();
            foreach (var id in _vertices.Keys) inDeg[id] = 0;
            foreach (var list in _adjacencyList.Values)
                foreach (var to in list)
                    if (inDeg.ContainsKey(to)) inDeg[to]++;
            return inDeg;
        }

       
        public Dictionary<string, int> GetOutDegrees()
        {
            var outDeg = new Dictionary<string, int>();
            foreach (var kv in _adjacencyList)
                outDeg[kv.Key] = kv.Value.Count;
            return outDeg;
        }

        public List<Vertex> GetUsersWithNoFollowers()
        {
            var inDeg = GetInDegrees();
            return _vertices.Values.Where(v => inDeg[v.Id] == 0).ToList();
        }

        public List<Vertex> GetMostInfluential()
        {
            var inDeg = GetInDegrees();
            if (!inDeg.Any()) return new List<Vertex>();
            int max = inDeg.Values.Max();
            return _vertices.Values.Where(v => inDeg[v.Id] == max).ToList();
        }

        public List<Vertex> GetMostActive()
        {
            var outDeg = GetOutDegrees();
            if (!outDeg.Any()) return new List<Vertex>();
            int max = outDeg.Values.Max();
            return _vertices.Values.Where(v => outDeg[v.Id] == max).ToList();
        }
    }
}
