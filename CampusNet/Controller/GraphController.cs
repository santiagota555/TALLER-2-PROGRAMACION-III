using System;
using System.Collections.Generic;
using System.Linq;
using CampusNet.Model;
using CampusNet.View;

namespace CampusNet.Controller
{
    public class GraphController
    {
        private readonly Graph _graph;

        public GraphController()
        {
            _graph = new Graph();
        }

        public void Run()
        {
            UseCase1_BuildGraph();
            UseCase2_Traversals();
            UseCase3_SocialQueries();
            UseCase4_CrudOperations();
            GraphView.PrintSectionTitle("FIN DE LA SIMULACIÓN – CampusNet");
            Console.WriteLine();
        }

        private void UseCase1_BuildGraph()
        {
            GraphView.PrintSectionTitle("CASO DE USO 1 – Construcción del grafo");

            var users = new List<(string id, string name, UserRole role)>
            {
                ("U01", "Ana Torres",    UserRole.Estudiante),
                ("U02", "Carlos Ruiz",   UserRole.Profesor),
                ("U03", "Diana Lopez",   UserRole.Egresado),
                ("U04", "Esteban Mora",  UserRole.Estudiante),
                ("U05", "Fernanda Gil",  UserRole.Profesor),
                ("U06", "Gabriel Soto",  UserRole.Egresado),
                ("U07", "Helena Vargas", UserRole.Estudiante),
                ("U08", "Ivan Castro",   UserRole.Profesor),
                ("U09", "Julia Rios",    UserRole.Egresado),
                ("U10", "Kevin Pardo",   UserRole.Estudiante),
                ("U11", "Laura Mendez",  UserRole.Estudiante),
                ("U12", "Miguel Nunez",  UserRole.Profesor),
            };

            foreach (var (id, name, role) in users)
                _graph.AddVertex(new Vertex(id, name, role));

            // 18 edges:
            // U01 out-degree=5, U02 out-degree=4
            // Directed cycle: U04 -> U07 -> U10 -> U04
            // U11 and U12 start with in-degree 0
            var edges = new List<(string from, string to)>
            {
                ("U01","U02"),("U01","U03"),("U01","U04"),("U01","U05"),("U01","U06"),
                ("U02","U03"),("U02","U07"),("U02","U08"),("U02","U09"),
                ("U04","U07"),("U07","U10"),("U10","U04"),
                ("U03","U08"),
                ("U05","U09"),
                ("U06","U02"),
                ("U08","U03"),
                ("U09","U01"),
                ("U10","U06"),
            };

            foreach (var (from, to) in edges)
                _graph.AddEdge(from, to);

            GraphView.PrintAdjacencyList(_graph.GetAdjacencyList(), _graph.GetAllVertices());
            GraphView.PrintDegreeTable(_graph.GetInDegrees(), _graph.GetOutDegrees(), _graph.GetAllVertices());
        }

        private void UseCase2_Traversals()
        {
            GraphView.PrintSectionTitle("CASO DE USO 2 – Recorridos BFS y DFS");

            var vertexMap = _graph.GetAllVertices().ToDictionary(v => v.Id);

            string[] bfsStarts = { "U01", "U04", "U08" };
            foreach (var startId in bfsStarts)
            {
                var order = _graph.BFS(startId);
                var sv = _graph.GetVertex(startId);
                GraphView.PrintBFS(startId, sv?.Name ?? startId, order, vertexMap);
            }

            var dfsResult = _graph.DFSFull();
            GraphView.PrintDFS(dfsResult, vertexMap);
        }

        private void UseCase3_SocialQueries()
        {
            GraphView.PrintSectionTitle("CASO DE USO 3 – Consultas Sociales");

            var isolated = _graph.GetUsersWithNoFollowers();
            GraphView.PrintUsersWithNoFollowers(isolated);

            var influential = _graph.GetMostInfluential();
            var inDeg = _graph.GetInDegrees();
            int maxIn = influential.Count > 0 ? inDeg[influential[0].Id] : 0;
            GraphView.PrintMostInfluential(influential, maxIn);

            var active = _graph.GetMostActive();
            var outDeg = _graph.GetOutDegrees();
            int maxOut = active.Count > 0 ? outDeg[active[0].Id] : 0;
            GraphView.PrintMostActive(active, maxOut);

            GraphView.PrintReachability(
                "U01", _graph.GetVertex("U01")?.Name ?? "U01",
                "U09", _graph.GetVertex("U09")?.Name ?? "U09",
                _graph.CanReach("U01", "U09"));

            GraphView.PrintReachability(
                "U11", _graph.GetVertex("U11")?.Name ?? "U11",
                "U02", _graph.GetVertex("U02")?.Name ?? "U02",
                _graph.CanReach("U11", "U02"));
        }

        private void UseCase4_CrudOperations()
        {
            GraphView.PrintSectionTitle("CASO DE USO 4 – Operaciones CRUD");

            
            var newUser = new Vertex("U13", "Natalia Reyes", UserRole.Estudiante);
            bool addedV = _graph.AddVertex(newUser);
            GraphView.PrintCrudResult("Agregar usuario U13 (Natalia Reyes)", addedV, newUser.ToString());

            
            bool dupV = _graph.AddVertex(new Vertex("U13", "Duplicado", UserRole.Profesor));
            GraphView.PrintCrudResult("Agregar duplicado U13", !dupV,
                dupV ? "ERROR: insertado duplicado" : "Rechazado correctamente (ya existe)");

            
            bool ae1 = _graph.AddEdge("U13", "U01");
            GraphView.PrintCrudResult("Agregar arista U13->U01", ae1, ae1 ? "Insertada" : "Ya existia");

            bool ae2 = _graph.AddEdge("U11", "U12");
            GraphView.PrintCrudResult("Agregar arista U11->U12", ae2, ae2 ? "Insertada" : "Ya existia");

            
            bool dupE = _graph.AddEdge("U13", "U01");
            GraphView.PrintCrudResult("Agregar arista duplicada U13->U01", !dupE,
                dupE ? "ERROR: insertado duplicado" : "Rechazada correctamente (duplicado)");

            Console.WriteLine("\n  [Lista de adyacencia tras agregar U13 y aristas]");
            GraphView.PrintAdjacencyList(_graph.GetAdjacencyList(), _graph.GetAllVertices());

            
            bool updated = _graph.UpdateVertex("U10", newName: "Kevin Pardo Jr.", newRole: UserRole.Egresado);
            GraphView.PrintCrudResult("Actualizar U10 (nombre y rol)", updated,
                _graph.GetVertex("U10")?.ToString() ?? "no encontrado");

            
            bool re = _graph.RemoveEdge("U01", "U06");
            GraphView.PrintCrudResult("Eliminar arista U01->U06", re, re ? "Eliminada" : "No encontrada");

            
            bool rv = _graph.RemoveVertex("U13");
            GraphView.PrintCrudResult("Eliminar usuario U13", rv,
                rv ? "Eliminado (con sus aristas)" : "No encontrado");

            GraphView.PrintSectionTitle("ESTADO FINAL DEL GRAFO");
            GraphView.PrintAdjacencyList(_graph.GetAdjacencyList(), _graph.GetAllVertices());
            GraphView.PrintDegreeTable(_graph.GetInDegrees(), _graph.GetOutDegrees(), _graph.GetAllVertices());
        }
    }
}
