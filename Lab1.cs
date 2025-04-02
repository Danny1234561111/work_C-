﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread01
{
    class ProgrammLab01
    {
        static void Main()
        {
            Thread thread1 = new Thread(() => Print(5, 15));
            Thread thread2 = new Thread(() => Print(10, 20));
            thread1.Start();
            thread2.Start();
        }

        static void Print(int start, int end)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} начал работу");

            for (int i = start; i <= end; i++)
            {
                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
                Thread.Sleep(100); 
            }

            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} завершил работу");
        }
    }
}