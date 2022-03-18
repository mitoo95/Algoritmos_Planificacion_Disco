using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanificadorDisco
{
    class Disco
    {
        public int pistas { get; set; }
        public int posicionCabeza { get; set; }
        public IList<int> listaEntrada { get; set; }
        public IList<int> listaPlanificacion { get; set; }
        public Disco()
        {
            this.pistas = 0;
            this.posicionCabeza = 0;
            this.listaEntrada = new List<int>(pistas);
            this.listaPlanificacion = new List<int>(pistas);
        }
        public IList<int> llenadoColaEntrada(IList<int> lista, int p, int c)
        {
            this.pistas = p;
            this.posicionCabeza = c;
            var rand = new Random();
            Console.WriteLine("Rellenado aleatorio de lista:");
            for (int i = 0; i < pistas; i++)
            {
                lista.Add(rand.Next(0, pistas));
                Console.Write(lista[i] + ", ");
                Task.Delay(1000).Wait();
            }

            return lista;
        }
    }
    public class node
    {
        public int distancia = 0;
        public Boolean accessed = false;
    }
    class Algoritmos
    {
        public int contadorFIFO = 0;
        public int contadorSSTF = 0;
        public int contadorSCAN = 0;
        public int contadorCSCAN = 0;
        

        public IList<int> FIFO(IList<int> list, int head)
        {

            foreach(var cur_track in list)
            {
                var distancia = Math.Abs(cur_track - head);
                contadorFIFO += distancia;
                head = cur_track;
            }

            return list;
        }
        public IList<int> SSTF(IList<int> list, int head)
        {
            if (list.Count == 0)
                return list;

            node[] dif = new node[list.Count];

            for (int i = 0; i < dif.Length; i++)
                dif[i] = new node();
            
            int[] secuenciaSeek = new int[list.Count + 1];

            for (int i = 0; i < list.Count; i++)
            {
                secuenciaSeek[i] = head;
                calcularDiferencia(list, head, dif);
                int index = findMin(dif);
                dif[index].accessed = true;
                contadorSSTF += dif[index].distancia;
                head = list[index];
            }

            secuenciaSeek[secuenciaSeek.Length - 1] = head;

            return secuenciaSeek;
        }
        public static void calcularDiferencia(IList<int> cola, int cabeza, node[] dif)
        {
            for (int i = 0; i < dif.Length; i++)
                dif[i].distancia = Math.Abs(cola[i] - cabeza);
        }
        public static int findMin(node[] dif)
        {
            int indice = -1, minimo = int.MaxValue;

            for (int i = 0; i < dif.Length; i++)
            {
                if (!dif[i].accessed && minimo > dif[i].distancia)
                {

                    minimo = dif[i].distancia;
                    indice = i;
                }
            }
            return indice;
        }
        public IList<int> SCAN(IList<int> list, int head, String direccion)
        {
            int distancia, cur_track;
            List<int> left = new List<int>(),
                            right = new List<int>();
            List<int> secuenciaSeek = new List<int>();
            var disk = new Disco();

            if (direccion == "left")
                left.Add(0);
            else if (direccion == "right")
                right.Add(disk.pistas - 1);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < head)
                    left.Add(list[i]);
                if (list[i] > head)
                    right.Add(list[i]);
            }

            left.Sort();
            right.Sort();

            int run = 2;
            while (run-- > 0)
            {
                if (direccion == "left")
                {
                    for (int i = left.Count - 1; i >= 0; i--)
                    {
                        cur_track = left[i];
                        secuenciaSeek.Add(cur_track);
                        distancia = Math.Abs(cur_track - head);
                        contadorSCAN += distancia;
                        head = cur_track;
                    }
                    direccion = "right";
                }
                else if (direccion == "right")
                {
                    for (int i = 0; i < right.Count; i++)
                    {
                        cur_track = right[i];
                        secuenciaSeek.Add(cur_track);
                        distancia = Math.Abs(cur_track - head);
                        contadorSCAN += distancia;
                        head = cur_track;
                    }
                    direccion = "left";
                }
            }
            return secuenciaSeek;
        }
        public IList<int> CSCAN(IList<int> list, int head)
        {
            int distancia, cur_track;
            var disk = new Disco();

            List<int> left = new List<int>();
            List<int> right = new List<int>();
            List<int> secuenciaSeek = new List<int>();

            left.Add(0);
            right.Add(disk.pistas - 1);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < head)
                    left.Add(list[i]);
                if (list[i] > head)
                    right.Add(list[i]);
            }

            left.Sort();
            right.Sort();

            for (int i = 0; i < right.Count; i++)
            {
                cur_track = right[i];
                secuenciaSeek.Add(cur_track);
                distancia = Math.Abs(cur_track - head);
                contadorCSCAN += distancia;
                head = cur_track;
            }

            head = 0;

            contadorCSCAN += (disk.pistas - 1);

            for (int i = 0; i < left.Count; i++)
            {
                cur_track = left[i];
                secuenciaSeek.Add(cur_track);
                distancia = Math.Abs(cur_track - head);
                contadorCSCAN += distancia;
                head = cur_track;
            }
            return secuenciaSeek;
        }
    }
}
