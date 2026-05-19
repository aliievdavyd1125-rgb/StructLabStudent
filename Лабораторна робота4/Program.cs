using System;
using System.Collections.Generic;
using System.IO;

namespace Лабораторна_робота4
{
    using System;
    using System.Text;
    using MyFrac = (long nom, long denom);
    using MyTime = (int hour, int min, int sec);



    public static class TimeWork
    {
        public static string MytimeToString(MyTime t)
        {
            return $"{t.hour}:{t.min:D2}:{t.sec:D2}";
        }
        public static MyTime Normalize(MyTime t)
        {
            int total_hours = ToSecSinceMidnight(t);
            total_hours = (total_hours % 86400 + 86400) % 86400; // для межі (відємних чисел)

            MyTime t2 = FromSecSinceMidnight(total_hours);

            return t2;
        }

        public static int ToSecSinceMidnight(MyTime t)
        {
            return t.hour * 3600 + t.min * 60 + t.sec;
        }

        public static MyTime FromSecSinceMidnight(int t)
        {
            t = (t % 86400 + 86400) % 86400;

            int hours = t / 3600;
            int minutes = (t % 3600) / 60;
            int seconds = t % 60;

            return (hours, minutes, seconds);
        }

        public static MyTime AddOneSecond(MyTime t)
        {
            t.sec += 1;
            t = Normalize(t);

            return t;

        }

        public static MyTime AddOneMinute(MyTime t)
        {
            t.min += 1;
            t = Normalize(t);

            return t;
        }

        public static MyTime AddOneHour(MyTime t)
        {
            t.hour += 1;

            t = Normalize(t);
            return t;
        }

        public static MyTime AddSeconds(MyTime t, int s)
        {
            int seconds = ToSecSinceMidnight(t);
            int total_seconds = seconds + s;

            t = FromSecSinceMidnight(total_seconds);
            return t;
        }

        public static int Difference(MyTime t1, MyTime t2)
        {
            int first_time = ToSecSinceMidnight(t1);
            int second_time = ToSecSinceMidnight(t2);

            return first_time - second_time;
        }

        public static string WhatLesson(MyTime t)
        {
            var lessons = new (int start, int end)[]
            {
            (8*3600, 9*3600 + 20*60),
            (9*3600 + 40*60, 11*3600),
            (11*3600 + 20*60, 12*3600 + 40*60),
            (13*3600, 14*3600 + 20*60),
            (14*3600 + 40*60, 16*3600),
            (16*3600 + 10*60, 17*3600 + 30*60),
            };

            int current_time = t.hour * 3600 + t.min * 60 + t.sec;

            if (lessons[0].start > current_time)
                return "пари ще не почалися";

            else if (lessons[5].end < current_time)
                return "пари вже скінчилися";

            for (int i = 0; i < lessons.Length; i++)
            {
                if (lessons[i].start <= current_time && current_time <= lessons[i].end)
                {
                    return $"{i + 1}-a пара";
                }

                if (i < lessons.Length - 1)
                {
                    if (current_time > lessons[i].end && current_time < lessons[i + 1].start)
                    {
                        return $"перерва між {i + 1}-ю та {i + 2}-ю парами";
                    }
                }
            }
            return "невідомий стан";
        }

        public static MyTime Enter_time()
        {
            Console.Write("Введіть час у такому форматі (години:хвилини:секунди): ");
            string my_time = Console.ReadLine();

            int count = my_time?.Count(c => c == ':') ?? 0;
            bool ok = my_time != null &&
          my_time.All(c => char.IsDigit(c) || c == ':' || c == '-');

            while (count < 2 || !ok)
            {
                Console.WriteLine("Введіть коректний час");
                my_time = Console.ReadLine();

                count = my_time?.Count(c => c == ':') ?? 0;
                ok = my_time != null && my_time.All(c => char.IsDigit(c) || c == ':');
            }

            string[] parts = my_time.Split(":");

            int H = int.Parse(parts[0]);
            int M = int.Parse(parts[1]);
            int S = int.Parse(parts[2]);

            MyTime t = new MyTime(H, M, S);
            return t;
        }
    }

    public static class Work_with_fraction
    {
        public static string MyFracToString(MyFrac f)
        {
            return $"{f.nom}/{f.denom}";
        }

        static long Algoruthm_Evclid(long nom,long denom)
        {
            long max = Math.Max(nom,denom); 
            long min = Math.Min(nom, denom);

            while (min != 0)
            {
                long temp = min;
                min = max % min;
                max = temp;
            }

            long Lowest_common = Math.Abs(max);
            return Lowest_common;
        }

        static long Least_Common_Denominator(long a,long b)
        {
            long GCD = Algoruthm_Evclid(a, b); // Greatest Common Divisor
            long LCD = (a * b) / GCD;

            return LCD;
        }

        public static MyFrac Normalize(MyFrac t)
        {   

            long Lowest_common = Algoruthm_Evclid(t.nom,t.denom);

            long nom = t.nom / Lowest_common;
            long denom = t.denom / Lowest_common;

            if (t.denom < 0)
            {
                nom *= -1;
                denom *= -1;
            }

            return new MyFrac(nom,denom);         
        }

        public static MyFrac Enter_Frac()
        {
            while (true)
            {
                try
                {
                    Console.Write("Введіть чисельник: ");
                    long nom = long.Parse(Console.ReadLine());

                    Console.Write("Введіть знаменник: ");
                    long denom = long.Parse(Console.ReadLine());

                    long check_res = nom / denom;

                    return new MyFrac(nom, denom);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Введіть коректне число!");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("На нуль ділити не можна!");
                }
            }
        }

        public static string ToStringWithIntPart(MyFrac f)
        {

            MyFrac normalized = Normalize(f);
            long nom = normalized.nom;

            if (nom < 0)
                nom = Math.Abs(nom);

            long int_part = nom / normalized.denom;
            long left_part = nom % normalized.denom;

            string sign = "";


            if (normalized.nom < 0)
            {
                sign = "-";
            }

            if(int_part==0)
            {
                return $"{sign}({left_part}/{normalized.denom})";
            }

            return $"{sign}({int_part}+{left_part}/{normalized.denom})";
        }

        public static double DoubleValue(MyFrac f)
        {
            return (double)f.nom / f.denom;
        }

        public static MyFrac Plus(MyFrac f1, MyFrac f2)
        {  
           f1 = Normalize(f1); // Скорочуємо для подальших обчсилень
           f2 = Normalize(f2);

           long LCD = Least_Common_Denominator(f1.denom, f2.denom);

            long multiplef1 = LCD / f1.denom;
            long multiplef2 = LCD / f2.denom;

            long total_nom = multiplef1*f1.nom + multiplef2*f2.nom;
            MyFrac f = new MyFrac(total_nom,LCD);
            return Normalize(f);
        }

        public static MyFrac Minus(MyFrac f1, MyFrac f2)
        {
            f1 = Normalize(f1);
            f2 = Normalize(f2);

            long LCD = Least_Common_Denominator(f1.denom, f2.denom);

            long multiplef1 = LCD / f1.denom;
            long multiplef2 = LCD / f2.denom;

            long total_nom = multiplef1 * f1.nom - multiplef2 * f2.nom;
            MyFrac f = new MyFrac(total_nom, LCD);
            return Normalize(f);
        }

        public static MyFrac Multiply(MyFrac f1, MyFrac f2)
        {
            f1 = Normalize(f1);
            f2 = Normalize(f2);

            MyFrac f = new MyFrac(f1.nom * f2.nom, f1.denom * f2.denom);
            return Normalize(f);
        }

        public static MyFrac Divide(MyFrac f1, MyFrac f2)
        {    
            if(f2.nom == 0)
            {
                Console.WriteLine("На нуль ділити не можна");
                return f1;
            }

            f1 = Normalize(f1);
            f2 = Normalize(f2);

            MyFrac f = new MyFrac(f1.nom * f2.denom, f1.denom * f2.nom);
            return Normalize(f);
        }

        public static MyFrac CalcExpr1(int n)
        {

            MyFrac f = new MyFrac(1, 2);
            for (int i = 1; i < n; i++)
            {
                MyFrac f2 = new MyFrac(1,(i+1)*(i+2));     
                f = Plus(f, f2);             
            }
            return f;
        }

        public static MyFrac CalcExpr2(int n)
        {
            MyFrac f1 = new MyFrac(1, 1);
            MyFrac f2 = new MyFrac(1, 4);

            MyFrac f = Minus(f1, f2);

            for(int i = 2;i<n;i++)
            {
                long x = (i + 1) * (i + 1);
                f2 = new MyFrac(1,x);
                f = Multiply(f, Minus(f1, f2));
            }

            return f;
        }
    }

    public static class FileWork
    {   
        
        public static List<Student> Collect_Students(StreamReader sr)
        {
             
            List<Student> All_studnets = new List<Student>();
            string line;

            while ((line = sr.ReadLine()) != null) // Перебирає поки не скінчиться файл
            {

                if (string.IsNullOrWhiteSpace(line)) continue; // Пропускає лінію якщо вона пуста

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 9)
                {
                    continue;
                }

                Student s = new Student(parts); // Створення студента
                All_studnets.Add(s);
            }

            return All_studnets;        
        }

        public static void Print_Info(List<Student> students)
        {
            Console.WriteLine("Загальна інформація про студентів:");

            foreach (Student student in students)
            {
                Console.WriteLine(
                    $"Ім'я: {student.firstName}, " +
                    $"Прізвище: {student.surName}, " +
                    $"По батькові: {student.patronymic}, " +
                    $"Стать: {student.sex}, " +
                    $"Дата народження: {student.dateOfBirth}, " +
                    $"Оцінки: {student.mathematicsMark}, {student.physicsMark}, {student.informaticsMark}, " +
                    $"Стипендія: {student.scholarship}"
                );
            }
        }

        public static void Update_students(List<Student> students)
        {
            for (int i = 0; i < students.Count; i++)
            {    
                char math_grade = students[i].mathematicsMark == '-' ? '5':students[i].mathematicsMark;
                char physic_grade = students[i].physicsMark == '-' ? '5':students[i].physicsMark;
                char informatic_grade = students[i].informaticsMark == '-' ? '5':students[i].informaticsMark;
                    

                if (math_grade == '5' &&
                    physic_grade == '5' &&
                    informatic_grade == '5')
                {
                    Student s = students[i];
                    s.scholarship = 3000;
                    students[i] = s;
                }
            }
        }

        public static void SaveStudents(List<Student> students)
        {
            using (StreamWriter sw = new StreamWriter("D:\\students_info\\data_new.txt", false, Encoding.UTF8)) // false - стриає старий файл
            {
                foreach (var s in students)
                {   

                    sw.WriteLine(
                        $"{s.surName} {s.firstName} {s.patronymic} {s.sex} " +
                        $"{s.dateOfBirth} {s.mathematicsMark} {s.physicsMark} {s.informaticsMark} {s.scholarship}"
                    );
                }
            }
        }

        public struct Student
        {
            public string surName;
            public string firstName;
            public string patronymic;
            public char sex;
            public string dateOfBirth;
            public char mathematicsMark;
            public char physicsMark;
            public char informaticsMark;
            public int scholarship;

            public Student(string[] parts)
            {
                surName = parts[0];
                firstName = parts[1];
                patronymic = parts[2];

                sex = char.Parse(parts[3]);
                dateOfBirth = parts[4];

                mathematicsMark = char.Parse(parts[5]);
                physicsMark = char.Parse(parts[6]);
                informaticsMark = char.Parse(parts[7]);

                scholarship = int.Parse(parts[8]);
            }
        }
        

    }



    internal class Program
    {   

        static void First_task()
        {
            bool status = true;
            MyTime t = TimeWork.Enter_time();

            while (status)
            {
                Console.WriteLine("Виберіть цифру для виконання коду");
                Console.WriteLine("{1-Нормалізувати часи,2-Перевторити в секунди," +
                    "3-Перетворити в звичайний час,4-Додати одну секунду,5-Додати одну хвилину,6-Додати одну годину,7-Знайти різницю" +
                    "8-Додати секунди,9-Визначити яка зарах пара,10 - Ввести нове значення ,0-вийти з циклу}");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        t = TimeWork.Normalize(t);
                        Console.WriteLine(TimeWork.MytimeToString(t));
                        break;

                    case 2:
                        Console.WriteLine(TimeWork.ToSecSinceMidnight(t));
                        break;
                    case 3:
                        Console.WriteLine(TimeWork.MytimeToString(t));
                        break;

                    case 4:
                        t = TimeWork.AddOneSecond(t);
                        Console.WriteLine(TimeWork.MytimeToString(t));
                        break;
                    case 5:
                        t = TimeWork.AddOneMinute(t);
                        Console.WriteLine(TimeWork.MytimeToString(t));
                        break;

                    case 6:
                        t = TimeWork.AddOneHour(t);
                        Console.WriteLine(TimeWork.MytimeToString(t));
                        break;
                    case 7:
                        MyTime t2 = TimeWork.Enter_time();
                        Console.WriteLine(TimeWork.Difference(t, t2));
                        break;
                    case 8:
                        Console.Write("Введіть секунди: ");
                        int s = int.Parse(Console.ReadLine());

                        t = TimeWork.AddSeconds(t, s);
                        Console.WriteLine(TimeWork.MytimeToString(t)); ;
                        break;

                    case 9:
                        Console.WriteLine(TimeWork.WhatLesson(t));
                        break;

                    case 0:
                        status = false;
                        break;

                    case 10:
                        t = TimeWork.Enter_time();
                        break;

                    default:
                        Console.WriteLine("Такого вибору не існує");
                        break;

                }
            }

           
        }


        static void Second_task()
        {
           bool status = true;
           MyFrac f = Work_with_fraction.Enter_Frac();
           MyFrac f2 = (0, 0);
           int n;

            while (status)
            {
                Console.WriteLine("Виберіть цифру для виконання коду");
                Console.WriteLine("{1-Рядкове подання дробу,2-Нормалізувати дріб," +
                    "3-Сформувати рядкове подання,4-Обрахувати суму дробів,5 - Обрахувати різницю дробів,6-Обрахувати множення дробів,7 - Обрахувати ділення дробів ," +
                    "8-Обчислити вираз(1 версія),9-Обчислити вираз(2 версія),10-Перезаписати значення,0-Вийти з циклу");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine(Work_with_fraction.MyFracToString(f));
                        break;

                    case 2:
                        f = Work_with_fraction.Normalize(f);
                        Console.WriteLine(Work_with_fraction.MyFracToString(f));
                        break;

                    case 3:
                        Console.WriteLine(Work_with_fraction.ToStringWithIntPart(f));
                        break;
                    case 4:
                        Console.WriteLine("Введіть другий дріб");
                        f2 = Work_with_fraction.Enter_Frac();
                        Console.WriteLine(Work_with_fraction.Plus(f, f2));
                        break;

                    case 5:
                        Console.WriteLine("Введіть другий дріб");
                        f2 = Work_with_fraction.Enter_Frac();
                        Console.WriteLine(Work_with_fraction.Minus(f, f2));
                        break;

                    case 6:
                        Console.WriteLine("Введіть другий дріб");
                        f2 = Work_with_fraction.Enter_Frac();
                        Console.WriteLine(Work_with_fraction.Multiply(f, f2));
                        break;

                    case 7:
                        Console.WriteLine("Введіть другий дріб");
                        f2 = Work_with_fraction.Enter_Frac();
                        Console.WriteLine(Work_with_fraction.Divide(f, f2));
                        break;

                    case 8:
                        Console.WriteLine("Введіть число n");
                        n = int.Parse(Console.ReadLine());
                        Console.WriteLine(Work_with_fraction.CalcExpr1(n));
                        break;

                    case 9:
                        Console.WriteLine("Введіть число n");
                        n = int.Parse(Console.ReadLine());
                        Console.WriteLine(Work_with_fraction.CalcExpr2(n));
                        break;

                    case 10:
                        f = Work_with_fraction.Enter_Frac();
                        break;

                    case 0:
                        status = false;
                        break;

                    default:
                        Console.WriteLine("Такого вибору не існує");
                        break;
                }         
            }
        }
       
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Виберіть блок завдань (1-1_Блок,2-2_Блок)");
                int block_chocie = int.Parse(Console.ReadLine());
                int task_hcoie;


                if (block_chocie == 1)
                {
                    Console.WriteLine("Введіть номер завдання");
                    task_hcoie = int.Parse(Console.ReadLine());
                    switch (task_hcoie)
                    {
                        case 1:
                            First_task();
                            break;

                        case 2:
                            Second_task();
                            break;
                    }
                }

                else if (block_chocie == 2)
                {
                    string file_path = "some_file";
                    while (file_path != "exit")
                    {
                        try
                        {
                            Console.WriteLine("Введіть назву файлу");
                            file_path = Console.ReadLine();
                            List<FileWork.Student> All_students;

                            using (StreamReader sr = new StreamReader(file_path, Encoding.UTF8))
                            {
                                All_students = FileWork.Collect_Students(sr);

                                FileWork.Print_Info(All_students);
                            }

                            FileWork.Update_students(All_students);
                            FileWork.SaveStudents(All_students);

                        }
                        catch (FileNotFoundException)
                        {   
                            if(file_path != "exit")
                            Console.WriteLine("Введіть коректну назву файлу!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Введіть коректний номер завдання");
                }
            }         
        }
    }
}
