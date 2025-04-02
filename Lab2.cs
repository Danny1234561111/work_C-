using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread01
{
    class ProgrammLab02
    {
        static Thread? thread1;
        static void Print(int Number)
        {
            Console.WriteLine($"Поток {Number} начал работу");
            for (int i = 1; i <= 100; i++)
            {
                Console.WriteLine($"Поток {Number}: {i}");
            }
            Console.WriteLine($"Поток {Number} завершил работу");
        }
        static void Main()
        {
            Console.WriteLine("Основной поток начал работу");
            Console.WriteLine("\nЭксперимент 1: Без задержки между запусками");
            thread1 = new Thread(PrintNumbersFirst);
            Thread thread2 = new Thread(PrintNumbersSecond);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            Console.WriteLine("\nЭксперимент 2: С задержкой 1с между запусками");
            thread1 = new Thread(PrintNumbersFirst);
            thread2 = new Thread(PrintNumbersSecond);

            thread1.Start();
            Thread.Sleep(1000); 
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("\nОсновной поток завершил работу");
        }
        
        static void PrintNumbersFirst()
        {
            Console.WriteLine("Поток 1 начал работу");
            Print(1);
        }
        static void PrintNumbersSecond()
        {
            thread1.Join(); 

            Print(2);

        }
    }
}