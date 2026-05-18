using System;
using System.Collections.Generic;
using CampusNet.Model;

namespace CampusNet.View
{
    public static class GraphView
    {
        
        //  Helpers
        

        private static void Header(string title)
        {
            string line = new string('═', 60);
            Console.WriteLine();
            Console.WriteLine(line);
            Console.WriteLine($"  {title}");
            Console.WriteLine(line);
        }

        private static void SubHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"  ── {title} ──");
        }

        
        //  Graph structure
        

        public static void PrintAdjacencyList(
            Dictionary<string, List<string>> adjList,
            IEnumerable<Vertex> vertices)
        {
            Header("LISTA DE ADYACENCIA – CampusNet");

            
            var vertexMap = new Dictionary<string, Vertex>();
            foreach (var v in vertices) vertexMap[v.Id] = v;

            foreach (var kv in adjList)
            {
                string fromLabel = vertexMap.TryGetValue(kv.Key, out var fromV)
                    ? fromV.ToString() : kv.Key;

                if (kv.Value.Count == 0)
                {
                    Console.WriteLine($"  {fromLabel}  →  (ninguno)");
                }
                else
                {
                    var targets = new List<string>();
                    foreach (var toId in kv.Value)
                    {
                        string toLabel = vertexMap.TryGetValue(toId, out var toV)
                            ? $"{toV.Name}({toId})" : toId;
                        targets.Add(toLabel);
                    }
                    Console.WriteLine($"  {fromLabel}  →  {string.Join(", ", targets)}");
                }
            }
        }

       
        //  BFS
       

        public static void PrintBFS(string startId, string startName, List<string> order,
                                    Dictionary<string, Vertex> vertexMap)
        {
            SubHeader($"BFS desde [{startId}] {startName}");
            Console.Write("  Orden de visita: ");
            var labels = new List<string>();
            foreach (var id in order)
            {
                string label = vertexMap.TryGetValue(id, out var v) ? $"{v.Name}({id})" : id;
                labels.Add(label);
            }
            Console.WriteLine(string.Join(" → ", labels));
            Console.WriteLine($"  Vértices alcanzados: {order.Count}");
        }

       
        //  DFS
        

        public static void PrintDFS(Graph.DfsResult result, Dictionary<string, Vertex> vertexMap)
        {
            Header("DFS COMPLETO");

            Console.Write("  Orden de descubrimiento: ");
            var labels = new List<string>();
            foreach (var id in result.DiscoveryOrder)
            {
                string label = vertexMap.TryGetValue(id, out var v) ? $"{v.Name}({id})" : id;
                labels.Add(label);
            }
            Console.WriteLine(string.Join(" → ", labels));

            Console.WriteLine();
            if (result.HasCycle)
            {
                Console.WriteLine($"  ⚠  CICLOS DETECTADOS (aristas de retroceso):");
                foreach (var (from, to) in result.BackEdges)
                {
                    string fl = vertexMap.TryGetValue(from, out var fv) ? $"{fv.Name}({from})" : from;
                    string tl = vertexMap.TryGetValue(to, out var tv) ? $"{tv.Name}({to})" : to;
                    Console.WriteLine($"       {fl}  →→  {tl}  (back edge)");
                }
            }
            else
            {
                Console.WriteLine("  No se detectaron ciclos.");
            }
        }

        //  Social queries
       

        public static void PrintUsersWithNoFollowers(List<Vertex> users)
        {
            SubHeader("Usuarios sin seguidores (grado de entrada = 0)");
            if (users.Count == 0) { Console.WriteLine("  Ninguno."); return; }
            foreach (var u in users) Console.WriteLine($"  • {u}");
        }

        public static void PrintMostInfluential(List<Vertex> users, int inDeg)
        {
            SubHeader($"Usuarios más influyentes (grado de entrada = {inDeg})");
            foreach (var u in users) Console.WriteLine($"  ★  {u}");
        }

        public static void PrintMostActive(List<Vertex> users, int outDeg)
        {
            SubHeader($"Usuarios más activos (grado de salida = {outDeg})");
            foreach (var u in users) Console.WriteLine($"  ✦  {u}");
        }

        public static void PrintReachability(string aId, string aName,
                                             string bId, string bName, bool canReach)
        {
            SubHeader($"Alcanzabilidad: ¿{aName}({aId}) puede llegar a {bName}({bId})?");
            Console.WriteLine(canReach
                ? $"  ✔  SÍ, {aName} puede alcanzar a {bName}."
                : $"  ✘  NO, {aName} NO puede alcanzar a {bName}.");
        }

     
        //  CRUD feedback
       

        public static void PrintCrudResult(string operation, bool success, string detail)
        {
            string mark = success ? "✔" : "✘";
            Console.WriteLine($"  [{mark}] {operation}: {detail}");
        }

        public static void PrintSectionTitle(string title)
        {
            Header(title);
        }

        public static void PrintDegreeTable(Dictionary<string, int> inDeg,
                                            Dictionary<string, int> outDeg,
                                            IEnumerable<Vertex> vertices)
        {
            SubHeader("Tabla de grados");
            Console.WriteLine($"  {"Id",-8} {"Nombre",-20} {"Rol",-12} {"In",-6} {"Out",-6}");
            Console.WriteLine(new string('-', 56));
            foreach (var v in vertices)
            {
                Console.WriteLine($"  {v.Id,-8} {v.Name,-20} {v.Role,-12} " +
                                  $"{inDeg[v.Id],-6} {outDeg[v.Id],-6}");
            }
        }
    }
}
