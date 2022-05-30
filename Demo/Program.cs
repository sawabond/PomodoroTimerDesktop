﻿using Domain;
using System;
using System.Threading;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var pt = new PomodoroTimer(new TimerConfiguration { Minutes = 0, Seconds = 3 });

            Console.WriteLine(pt.ToString());

            pt.Start();
            Thread.Sleep(5050);

            Console.WriteLine(pt.ToString());

            Console.ReadKey();
        }
    }
}
