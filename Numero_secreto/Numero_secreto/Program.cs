using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NumeroSecreto
{
    const int Minimo = 1;
    const int Maximo = 100;

    static readonly Random Aleatorio = new Random();

    static void Main()
    {
        Console.Write("¿Jugar tú (j) o ver la estrategia automática (a)? ");
        string modo = (Console.ReadLine() ?? "").Trim().ToLower();

        if (modo == "a")
            Demo();
        else
            Jugar();
    }

    // Modo jugador: el usuario intenta adivinar el número.
    static void Jugar()
    {
        int secreto = Aleatorio.Next(Minimo, Maximo + 1); // el límite superior es exclusivo
        int intentos = 0;
        bool acertado = false;

        Console.WriteLine($"Elegí un número entre {Minimo} y {Maximo}. ¡Adivínalo!");

        while (!acertado)
        {
            Console.Write("Tu intento: ");

            // TryParse evita que el programa falle si el usuario escribe letras
            if (!int.TryParse(Console.ReadLine(), out int intento))
            {
                Console.WriteLine("Por favor escribe un número entero.");
                continue;
            }

            if (intento < Minimo || intento > Maximo)
            {
                Console.WriteLine($"El número debe estar entre {Minimo} y {Maximo}.");
                continue;
            }

            intentos++;

            if (intento < secreto)
                Console.WriteLine("El número secreto es MAYOR.");
            else if (intento > secreto)
                Console.WriteLine("El número secreto es MENOR.");
            else
            {
                Console.WriteLine($"¡Correcto! Lo lograste en {intentos} intentos.");
                acertado = true;
            }
        }
    }

    // Modo automático: adivina siempre probando la mitad del rango.
    static int BusquedaBinaria(int secreto)
    {
        int bajo = Minimo;
        int alto = Maximo;
        int intentos = 0;

        while (bajo <= alto)
        {
            int intento = (bajo + alto) / 2;
            intentos++;
            Console.WriteLine($"Intento {intentos}: {intento} (rango {bajo}-{alto})");

            if (intento == secreto)
                return intentos;
            else if (intento < secreto)
                bajo = intento + 1;  // el secreto es mayor
            else
                alto = intento - 1;  // el secreto es menor
        }

        return intentos;
    }

    static void Demo()
    {
        int secreto = Aleatorio.Next(Minimo, Maximo + 1);
        Console.WriteLine($"Número secreto: {secreto}");
        int total = BusquedaBinaria(secreto);
        Console.WriteLine($"La búsqueda binaria lo encontró en {total} intentos (máximo posible: 7).");
    }
}