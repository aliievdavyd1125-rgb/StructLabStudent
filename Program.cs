using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace struct_lab_student
{
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
            int total_grade =
                students[i].mathematicsMark +students[i].physicsMark +
                students[i].informaticsMark;

            if (students[i].mathematicsMark == '5' &&
                students[i].physicsMark == '5' &&
                students[i].informaticsMark == '5')
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

  }
}
