﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulacionExtracciones
{
    class Program
    {
        private const int hv = 100000000;


        static void Main(string[] args)
        {
            Programa:
            int t, tpll, ns, tf, nt, ss, sll, staF, staI, tpsF, tpsI,stoI,stoF,itoI,itoF;
            Random rnd = new Random();


            
            t = tpll=stoI=stoF=itoI=itoF = 0; tf = 20000; ns = nt = 0; ss = sll = staF=staI = 0; tpsI = tpsF = hv;

            while (t <= tf)
            {
                simulacion(ref t, ref tpll, ref ns, ref nt, ref ss, ref sll, ref staI, ref staF, tpsI, tpsF, itoI, itoF, stoI, stoF, rnd);
            }
            while (ns > 0)
            {
                tpll = hv;
                simulacion(ref t, ref tpll, ref ns, ref nt, ref ss, ref sll, ref staI, ref staF, tpsI, tpsF, itoI, itoF, stoI, stoF, rnd);
            }

            Console.WriteLine("Personas totales: " + nt);
            Console.WriteLine("PEC: " + (ss - sll - staF - staI) / nt);
            Console.WriteLine("STA fijo: "+staF+ " STA intermitente: " + staI);

            Console.WriteLine("Atendidos: " + nt);

            Console.WriteLine("PTOF: " + ( stoF/t));

            Console.WriteLine("Duración simulación: " + t);

            Console.WriteLine("Escriba 'y' si desea correr la simulación de nuevo");
            if (Console.ReadLine().ToLower() == "y")
            {
                goto Programa;
            }




        }


        private static void simulacion(ref int t, ref int tpll, ref int ns, ref int nt, ref int ss, ref int sll, ref int staI, ref int staF, int tpsI, int tpsF, int itoI, int itoF, int stoI, int stoF, Random rnd)
        {

            if (tpsF <= tpsI)
            {
                if (tpsF <= tpll)//salida fijo
                {
                    t = tpsF;
                    ss = ss + t;
                    ns--;
                    nt++;
                    if (ns >= 1)
                    {
                        int ta = generarTA(rnd);
                        tpsF = t + ta;
                        staF = staF + ta;
                    }
                    else
                    {
                        itoF = t;
                        tpsF = hv;
                    }
                }
                else //llegada
                {
                    t = tpll;
                    sll = sll + t;
                    int ia = generarIA(rnd);
                    tpll = t + ia;
                    ns++;
                    if (ns == 1)
                    {
                        int ta = generarTA(rnd);
                        staF = staF + ta;
                        tpsF = t + ta;
                        stoF = stoF + t - itoF;
                    }
                    else
                    {
                        if (ns == 7)
                        {
                            int ta = generarTA(rnd);
                            tpsI = t + ta;
                            staI = staI + ta;
                        }
                    }
                }
            }
            else
            {
                if (tpsI <= tpll) //salida intermitente
                {
                    t = tpsI;
                    ss = ss + t;
                    ns--;
                    nt++;
                    if (ns > 6)
                    {
                        int ta = generarTA(rnd);
                        staI = staI + ta;
                        tpsI = t + ta;

                    }
                    else
                    {
                        tpsI = hv;
                    }
                }
                else //llegada
                {
                    t = tpll;
                    sll = sll + t;
                    int ia = generarIA(rnd);
                    tpll = t + ia;
                    ns++;
                    if (ns == 1)
                    {
                        int ta = generarTA(rnd);
                        staF = staF + ta;
                        tpsF = t + ta;
                        stoF = stoF + t - itoF;
                    }
                    else
                    {
                        if (ns == 7)
                        {
                            int ta = generarTA(rnd);
                            tpsI = t + ta;
                            staI = staI + ta;
                        }
                    }
                }
            }
        }

        private static int generarIA(Random rnd)
        {

            double x = rnd.NextDouble() * (0.975 - 0.01) + 0.01;
            int r = Convert.ToInt32(0.163 / Math.Pow((-1 + Math.Pow(x, (-0.0269811))), 0.8375209380234506));
            Console.WriteLine("IA generado: " + r);
            return r;
        }

        private static int generarTA(Random rnd)
        {

            double x = rnd.NextDouble() * (0.998 - 0.001) + 0.001;
            int r = Convert.ToInt32(Math.Pow(Math.Pow(1 - x, -1.1886) - 1, 0.1961 * 2.1947));
            Console.WriteLine("TA generado: " + r);
            return r;
        }

        private static int generarRET(Random rnd)
        {

            double x = rnd.NextDouble() * (0.975 - 0.01) + 0.01;
            double xx = Math.Pow(1 / x - 1, 0.567620);
            int r = Convert.ToInt32(2.115 / xx);
            Console.WriteLine("RET generado: " + r);
            return r;
        }
        private static int buscarPuesto(int[] tps, int[] sto)
        {
            if (tps.Length == 0)
            {
                return 0;
            }
            int[] filtrados = tps.Select((s, i) => new { i, s })
                            .Where(e => e.s == hv)
                            .Select(t => t.i)
                            .ToArray();
            int min = hv;
            int final = hv;
            foreach (var item in filtrados)
            {
                if (min > sto[item])
                {
                    min = sto[item];
                    final = item;
                }
            }

            return final;


        }

        private static int encontrarMenorTps(int[] tps)
        {
            if (tps.Length == 0)
            {
                return 0;
            }
            return Array.IndexOf(tps, tps.Min());
        }
    }
}