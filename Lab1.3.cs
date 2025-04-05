using System;
using System.Threading;

namespace MultiThreadExample
{
    class Program
    {
        private static double currentValue = 0.5;
        private static readonly object syncLock = new object();
        private static bool isCosineTurn = true;
        private static bool isActive = true;
        private const int MaxIterations = 10;
        private static int iterationCounter = 0;

        static void Main()
        {
            Console.WriteLine($"Начальное значение: {currentValue}");
            Console.WriteLine($"Максимальное количество итераций: {MaxIterations}");

            Thread cosineThread = new Thread(ComputeCosine);
            Thread arccosineThread = new Thread(ComputeArccosine);

            cosineThread.Start();
            arccosineThread.Start();

            cosineThread.Join();
            arccosineThread.Join();

            Console.WriteLine("Программа завершила выполнение.");
        }

        static void ComputeCosine()
        {
            currentValue = Math.Cos(currentValue * 2);
            try
            {
                while (isActive)
                {
                    lock (syncLock)
                    {
                        if (!isActive || iterationCounter >= MaxIterations)
                        {
                            isActive = false;
                            Monitor.Pulse(syncLock);
                            break;
                        }

                        while (!isCosineTurn)
                        {
                            Monitor.Wait(syncLock);
                            if (!isActive) return;
                        }

                        if (!isActive) return;

                        currentValue = Math.Cos(currentValue);
                        iterationCounter++;
                        Console.WriteLine($"Косинусный поток [{iterationCounter}/{MaxIterations}]: новое значение = {currentValue:F6}");

                        if (currentValue < -1.0 || currentValue > 1.0)
                        {
                            Console.WriteLine("Ошибка: значение вышло за пределы [-1, 1] для арккосинуса");
                            isActive = false;
                            Monitor.Pulse(syncLock);
                            break;
                        }

                        isCosineTurn = false;
                        Monitor.Pulse(syncLock);
                    }

                    Thread.Sleep(300);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Косинусный поток прерван");
            }
        }

        static void ComputeArccosine()
        {
            currentValue = Math.Acos(currentValue);
            try
            {
                while (isActive)
                {
                    lock (syncLock)
                    {
                        if (!isActive || iterationCounter >= MaxIterations)
                        {
                            isActive = false;
                            Monitor.Pulse(syncLock);
                            break;
                        }

                        while (isCosineTurn)
                        {
                            Monitor.Wait(syncLock);
                            if (!isActive) return;
                        }

                        if (!isActive) return;

                        if (currentValue < -1.0 || currentValue > 1.0)
                        {
                            Console.WriteLine("Ошибка: значение вне допустимого диапазона для арккосинуса");
                            isActive = false;
                            Monitor.Pulse(syncLock);
                            break;
                        }

                        currentValue = Math.Acos(currentValue);
                        iterationCounter++;
                        Console.WriteLine($"Арккосинусный поток [{iterationCounter}/{MaxIterations}]: новое значение = {currentValue:F6}");

                        isCosineTurn = true;
                        Monitor.Pulse(syncLock);
                    }

                    Thread.Sleep(300);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Арккосинусный поток прерван");
            }
        }
    }
}
