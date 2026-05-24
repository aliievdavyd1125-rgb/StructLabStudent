using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Лабораторна_робота4.FileWork;
using static Лабораторна_робота4.Methods_of_Tasks;

namespace Лабораторна_робота4
{    

    public static class Methods_of_Tasks
    {
        public struct Zodiac
        {
            public string Name;

            public int start_day;
            public int start_month;

            public int end_day;
            public int end_month;

            public DateTime Start;
            public DateTime End;


            public Zodiac(string parts)
            {
                string[] formated_mas = parts.Split(';');

                Name = formated_mas[0];
                string[] start = formated_mas[1].Split('-');

                start_day = int.Parse(start[0]);
                start_month = int.Parse(start[1]);

                string[] end = formated_mas[2].Split('-');

                end_day = int.Parse(end[0]);
                end_month = int.Parse(end[1]);

                Start = new DateTime(2000, start_month, start_day);
                End = new DateTime(2000, end_month, end_day);
            }
        }
        public static void Show_Groups (Dictionary<string, List<Student>> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine($"Група: {group.Key}");

                foreach (var student in group.Value)
                {
                    Console.WriteLine($"{student.surName} {student.firstName} {student.patronymic} {student.sex} " +
                        $"{student.dateOfBirth} {student.mathematicsMark} {student.physicsMark} {student.informaticsMark} {student.scholarship}");
                }
            }
        }

        public static void Create_File()
        {
            string path = @"D:\students_info\zodiacs";

            string[] zodiacs =
            {
            "Capricorn;22-12;19-01",
            "Aquarius;20-01;18-02",
            "Pisces;19-02;20-03",
            "Aries;21-03;19-04",
            "Taurus;20-04;20-05",
            "Gemini;21-05;20-06",
            "Cancer;21-06;22-07",
            "Leo;23-07;22-08",
            "Virgo;23-08;22-09",
            "Libra;23-09;22-10",
            "Scorpio;23-10;21-11",
            "Sagittarius;22-11;21-12"
            };

            Directory.CreateDirectory(path);

            string zodiacPath = Path.Combine(path, "zodiac.txt");

            if(!File.Exists(zodiacPath))
            {
              File.WriteAllLines(zodiacPath, zodiacs);
            }
        }

        public static List<Zodiac> Read_file()
        {
            List<Zodiac> zodiacs = new List<Zodiac>();

            using (StreamReader sr = new StreamReader(
                "D:\\students_info\\zodiacs\\zodiac.txt",
                Encoding.UTF8))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    zodiacs.Add(new Zodiac(line));
                }
            }

            return zodiacs;
        }
    }
    internal class Addition_Tasks
    {
       
        public static void Show_Female(List<Student> students)
        {
            double M_avarage_grade = 0;
            int counted_Male = 0;

            List<(string student_name,string student_lastname,string Middle_name,double avg_grade)> Female_list = new();
            foreach (var student in students)
            {
                if (char.ToUpper(student.sex) == 'M')
                {
                    double sub_avarage = (student.mathematicsMark - '0' + student.physicsMark - '0' + student.informaticsMark - '0')/3;
                    M_avarage_grade += sub_avarage;
                    counted_Male += 1;
                }

                else if(char.ToUpper(student.sex) == 'F')
                {
                    double sub_avarage = (student.mathematicsMark - '0' + student.physicsMark - '0' + student.informaticsMark - '0') / 3;
                    Female_list.Add((student.firstName,student.surName,student.patronymic,sub_avarage));
                }
            }

            if(counted_Male==0)
            {
                Console.WriteLine("Обрахувати неможливо,записано 0 студенітв");
                return;
            }

            M_avarage_grade /= counted_Male; // Визначаємо середнє арифметичне всіх чоловіків

            foreach (var female in Female_list)
            {
                if (female.avg_grade > M_avarage_grade)
                {
                    Console.WriteLine($"Ім'я:{female.student_name},Прізвище:{female.student_lastname},По батькові:{female.Middle_name}");
                }
            }
        }

        public static void Group(List<Student> students)
        {
            Dictionary<string, List<Student>> groups1 = new Dictionary<string, List<Student>>();
            Dictionary<string, List<Student>> groups2 = new Dictionary<string, List<Student>>();

            foreach (var student in students)
            {
                string formated_date = $"{student.dateOfBirth.Year}_{student.dateOfBirth.Day}";
                string formated_day = $"{student.dateOfBirth.Day}";

                if (!groups1.ContainsKey(formated_date))
                {
                    groups1[formated_date] = new List<Student>();
                }

                if (!groups2.ContainsKey(formated_date))
                {
                    groups2[formated_day] = new List<Student>();
                }

                groups1[formated_date].Add(student);
                groups2[formated_day].Add(student);
            }

            Methods_of_Tasks.Show_Groups(groups1); // Показує групу 1
            Methods_of_Tasks.Show_Groups(groups2); // Показує групу 2
        }

        public static void Group_Zodiac(List<Zodiac> zodiacs, List<Student> students)
        {
            string path = @"D:\students_info\zodiacs";

            foreach (var student in students)
            {
                DateTime original = student.dateOfBirth;
                DateTime fixed_date = new DateTime(2000, original.Month, original.Day);

                foreach (var zodiac in zodiacs)
                {
                    bool isInRange;

                    if (zodiac.Start <= zodiac.End)
                    {
                        isInRange =
                            zodiac.Start <= fixed_date &&
                            fixed_date <= zodiac.End;
                    }
                    else
                    {

                        isInRange =
                            fixed_date >= zodiac.Start ||
                            fixed_date <= zodiac.End;
                    }

                    if (isInRange)
                    {
                        string zodiacPath = Path.Combine(path, $"data_{zodiac.Name}.txt");

                        string line =
                            $"{student.firstName} {student.surName} {student.patronymic} " +
                            $"{student.dateOfBirth:dd-MM-yyyy}";

                        File.AppendAllText(zodiacPath, line + Environment.NewLine);

                        break;
                    }
                }
            }
        }
        public static void Init()
        {
            List<FileWork.Student> All_students;
            List<Zodiac> zodiacs;

            using (StreamReader sr = new StreamReader("D:\\students_info\\input.txt", Encoding.UTF8))
            {
                All_students = FileWork.Collect_Students(sr);
            }

            Console.WriteLine("Виберіть номер додаткового завдання,(1-Вивести всіх студенток з вищим балом,2-Продивитися групи студентів з одним днем народження),3-Записати студентів по знаку зодіака");


            int choice  = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Show_Female(All_students);
                    break;
                case 2:
                    Group(All_students);
                    break;
                case 3:
                    Methods_of_Tasks.Create_File();
                    zodiacs = Methods_of_Tasks.Read_file();
                    Group_Zodiac(zodiacs, All_students);
                    break;
            }
        }
    }
}
