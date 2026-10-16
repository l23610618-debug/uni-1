using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        //  g = distancia real: km que ya recorrimos desde Arad hasta esa ciudad
        //  h = distancia estimada: distancia en linea recta de esa ciudad a Bucarest
        //  f = g + h = distancia total estimado de llegar a Bucarest pasando por esa ciudad

        var h = new Dictionary<string, int> {
            {"Arad",366}, {"Bucharest",0}, {"Craiova",160}, {"Drobeta",242}, {"Eforie",161},
            {"Fagaras",178}, {"Giurgiu",77}, {"Hirsova",151}, {"Iasi",226}, {"Lugoj",244},
            {"Mehadia",241}, {"Neamt",234}, {"Oradea",380}, {"Pitesti",98}, {"Rimnicu Vilcea",193},
            {"Sibiu",253}, {"Timisoara",329}, {"Urziceni",80}, {"Vaslui",199}, {"Zerind",374} };

        // Carreteras del mapa: ciudadA, ciudadB, km
        string[] caminos = {
            "Oradea,Zerind,71", "Zerind,Arad,75", "Oradea,Sibiu,151", "Arad,Sibiu,140",
            "Arad,Timisoara,118", "Sibiu,Fagaras,99", "Sibiu,Rimnicu Vilcea,80", "Timisoara,Lugoj,111",
            "Lugoj,Mehadia,70", "Mehadia,Drobeta,75", "Drobeta,Craiova,120", "Rimnicu Vilcea,Craiova,146",
            "Rimnicu Vilcea,Pitesti,97", "Craiova,Pitesti,138", "Pitesti,Bucharest,101",
            "Fagaras,Bucharest,211", "Bucharest,Urziceni,85", "Bucharest,Giurgiu,90",
            "Urziceni,Hirsova,98", "Hirsova,Eforie,86", "Urziceni,Vaslui,142", "Vaslui,Iasi,92", "Iasi,Neamt,87" };

        // creacion del grafo
        var mapa = new Dictionary<string, Dictionary<string, int>>();
        foreach (string c in caminos)
        {
            string[] p = c.Split(',');
            int km = int.Parse(p[2]);
            if (!mapa.ContainsKey(p[0])) mapa[p[0]] = new Dictionary<string, int>();
            if (!mapa.ContainsKey(p[1])) mapa[p[1]] = new Dictionary<string, int>();
            mapa[p[0]][p[1]] = km;
            mapa[p[1]][p[0]] = km;
        }

        string inicio = "Arad", meta = "Bucharest";

        var g = new Dictionary<string, int> { { inicio, 0 } };        // g: mejor distancia real conocida desde Arad hasta cada ciudad (Arad vale 0)

        var padre = new Dictionary<string, string>();        // padre: desde que ciudad llegamos a cada ciudad (sirve para armar la ruta al final)

        var abiertas = new List<string> { inicio };        // abiertas: ciudades que todavia no hemos explorado

        var cerradas = new HashSet<string>();      // cerradas: ciudades que ya exploramos

        while (abiertas.Count > 0)
        {
            // Elegir la ciudad abierta con menor f = g + h
            string actual = abiertas.OrderBy(c => g[c] + h[c]).First();

            // Si la ciudad elegida es la meta, ya encontramos la mejor ruta
            if (actual == meta) break;

            abiertas.Remove(actual);
            cerradas.Add(actual);

            // Revisar cada vecino de la ciudad actual
            foreach (var vecino in mapa[actual])
            {
                if (cerradas.Contains(vecino.Key)) continue;

                // nuevoG: costo real si llegamos al vecino pasando por la ciudad actual
                //         = lo que ya llevamos (g de actual) + km de la carretera
                int nuevoG = g[actual] + vecino.Value;

                // Guardar solo si es la primera vez que vemos al vecino
                // o si este camino es mas corto que el que ya conociamos
                if (!g.ContainsKey(vecino.Key) || nuevoG < g[vecino.Key])
                {
                    g[vecino.Key] = nuevoG;
                    padre[vecino.Key] = actual;
                    if (!abiertas.Contains(vecino.Key)) abiertas.Add(vecino.Key);
                }
            }
        }

        // Reconstruir la ruta: desde la meta, ir al padre de cada ciudad hasta llegar al inicio
        var ruta = new List<string>();
        for (string n = meta; ; n = padre[n])
        {
            ruta.Insert(0, n);
            if (n == inicio) break;
        }   

        Console.WriteLine("Ruta: " + string.Join(" -> ", ruta.ToArray()));
        Console.WriteLine("Costo total: " + g[meta] + " km");
        Console.ReadKey();
    }
}