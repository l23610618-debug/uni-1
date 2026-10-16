using System;
using System.Collections.Generic;

namespace Arriero
{
    class Program
    {
        static void Main(string[] args)
        {
            const char izq = '0';
            const char der = '1';

            const int arriero = 0;
            const int puma = 1;
            const int cabra = 2;
            const int lechuga = 3;
            const int solo = 0;

            // Estado = 4 caracteres [arriero][puma][cabra][lechuga]; '0' = izquierda, '1' = derecha
            string inicio = "" + izq + izq + izq + izq;
            string meta = "" + der + der + der + der;

            // La cola guarda caminos completos ("0000 1010 ...") para poder mostrar la solución
            Queue<string> cola = new Queue<string>();
            List<string> visitados = new List<string>();

            cola.Enqueue(inicio);
            visitados.Add(inicio);

            while (cola.Count > 0)
            {
                // BFS: se saca siempre el camino más antiguo, así la primera solución es la más corta
                string camino = cola.Dequeue();
                string estadoActual = camino.Substring(camino.Length - 4); // últimos 4 chars = estado actual

                if (estadoActual == meta)
                {
                    string[] estados = camino.Split(' ');

                    Console.WriteLine("Solucion en " + (estados.Length - 1) + " pasos:");
                    Console.WriteLine();

                    for (int i = 0; i < estados.Length; i++)
                    {
                        string estado = estados[i];
                        string accion = "Inicio";

                        if (i > 0)
                        {
                            string antes = estados[i - 1];
                            accion = "El arriero cruza solo";
                            // El pasajero es quien cambió de orilla respecto al estado anterior
                            if (antes[puma] != estado[puma]) accion = "El arriero cruza con el puma";
                            if (antes[cabra] != estado[cabra]) accion = "El arriero cruza con la cabra";
                            if (antes[lechuga] != estado[lechuga]) accion = "El arriero cruza con la lechuga";
                        }

                        string enIzq = "";
                        string enDer = "";
                        if (estado[arriero] == izq) enIzq += "Arriero "; else enDer += "Arriero ";
                        if (estado[puma] == izq) enIzq += "Puma "; else enDer += "Puma ";
                        if (estado[cabra] == izq) enIzq += "Cabra "; else enDer += "Cabra ";
                        if (estado[lechuga] == izq) enIzq += "Lechuga "; else enDer += "Lechuga ";

                        Console.WriteLine("Paso " + i + ": " + accion);
                        Console.WriteLine("   " + estado + " -> Izq: " + enIzq + "| Der: " + enDer);
                        Console.WriteLine();
                    }
                    break;
                }

                // El arriero siempre cruza a la orilla contraria
                char orillaArriero = estadoActual[arriero];
                char orillaNueva = (orillaArriero == izq) ? der : izq;

                // llevar: 0 = solo, 1 = puma, 2 = cabra, 3 = lechuga
                for (int llevar = solo; llevar <= lechuga; llevar++)
                {
                    // Solo se puede llevar a quien esté en la misma orilla que el arriero
                    if (llevar != solo && estadoActual[llevar] != orillaArriero)
                        continue;

                    char[] nuevo = estadoActual.ToCharArray();
                    nuevo[arriero] = orillaNueva;
                    if (llevar != solo)
                        nuevo[llevar] = orillaNueva;
                    string nuevoEstado = new string(nuevo);

                    // Reglas: si el arriero no está, el puma se come a la cabra y la cabra a la lechuga
                    bool pumaSeComeCabra = nuevoEstado[puma] == nuevoEstado[cabra] && nuevoEstado[arriero] != nuevoEstado[puma];
                    bool cabraSeComeLechuga = nuevoEstado[cabra] == nuevoEstado[lechuga] && nuevoEstado[arriero] != nuevoEstado[cabra];
                    bool yaVisitado = visitados.Contains(nuevoEstado);

                    // Solo se agregan estados seguros y no repetidos
                    if (!pumaSeComeCabra && !cabraSeComeLechuga && !yaVisitado)
                    {
                        visitados.Add(nuevoEstado);
                        cola.Enqueue(camino + " " + nuevoEstado);
                    }
                }
            }

            Console.ReadKey();
        }
    }
}