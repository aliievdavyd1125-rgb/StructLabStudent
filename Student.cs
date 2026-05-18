using System;
using System.Text;
using System.Linq;

namespace struct_lab_student
{
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
