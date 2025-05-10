using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public string Name { get; set; }
    public List<Course> Courses { get; } = new();

    public void Enroll(Course course)
    {
        if (!Courses.Contains(course))
        {
            Courses.Add(course);
            course.AddStudent(this);
        }
    }

    public void ShowAttendanceReport()
    {
        Console.WriteLine($"\n[Frekwencja] {Name}");
        foreach (var course in Courses)
        {
            var attendance = course.Attendances.Where(a => a.Student == this).ToList();
            int present = attendance.Count(a => a.IsPresent);
            int total = attendance.Count;
            Console.WriteLine($"- {course.Title}: {present}/{total} obecności");
        }
    }

    public void ShowGrades()
    {
        Console.WriteLine($"\n[Oceny] {Name}");
        foreach (var course in Courses)
        {
            var grade = course.Grades.FirstOrDefault(g => g.Student == this);
            string gradeText = grade != null ? grade.Value.ToString("0.0") : "brak";
            Console.WriteLine($"- {course.Title}: {gradeText}");
        }
    }
}

class Course
{
    public string Title { get; set; }
    public List<Student> Students { get; } = new();
    public List<Grade> Grades { get; } = new();
    public List<Attendance> Attendances { get; } = new();

    public Course(string title) => Title = title;

    public void AddStudent(Student student)
    {
        if (!Students.Contains(student))
            Students.Add(student);
    }

    public void AssignGrade(Student student, double value)
    {
        Grades.Add(new Grade { Student = student, Course = this, Value = value });
    }

    public void MarkAttendance(Student student, DateTime date, bool isPresent)
    {
        Attendances.Add(new Attendance { Student = student, Course = this, Date = date, IsPresent = isPresent });
    }
}

class Grade
{
    public Student Student { get; set; }
    public Course Course { get; set; }
    public double Value { get; set; }
}

class Attendance
{
    public Student Student { get; set; }
    public Course Course { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
}

class Program
{
    static List<Student> students = new();
    static List<Course> courses = new();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1. Dodaj studenta");
            Console.WriteLine("2. Dodaj kurs");
            Console.WriteLine("3. Zapisz studenta na kurs");
            Console.WriteLine("4. Wystaw ocenę");
            Console.WriteLine("5. Dodaj frekwencję");
            Console.WriteLine("6. Pokaż raport frekwencji");
            Console.WriteLine("7. Pokaż oceny studenta");
            Console.WriteLine("0. Wyjście");
            Console.Write("Wybierz: ");

            string input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1": AddStudent(); break;
                case "2": AddCourse(); break;
                case "3": EnrollStudent(); break;
                case "4": AssignGrade(); break;
                case "5": AddAttendance(); break;
                case "6": ShowAttendanceReport(); break;
                case "7": ShowGrades(); break;
                case "0": return;
                default: Console.WriteLine("Nieznana opcja."); break;
            }
        }
    }

    static void AddStudent()
    {
        Console.Write("Podaj imię studenta: ");
        string name = Console.ReadLine();
        students.Add(new Student { Name = name });
        Console.WriteLine("Dodano studenta.");
    }

    static void AddCourse()
    {
        Console.Write("Podaj nazwę kursu: ");
        string title = Console.ReadLine();
        courses.Add(new Course(title));
        Console.WriteLine("Dodano kurs.");
    }

    static void EnrollStudent()
    {
        var student = SelectStudent();
        var course = SelectCourse();
        student?.Enroll(course);
        Console.WriteLine("Zapisano na kurs.");
    }

    static void AssignGrade()
    {
        var student = SelectStudent();
        var course = SelectCourse();
        Console.Write("Podaj ocenę: ");
        double grade = double.Parse(Console.ReadLine());
        course.AssignGrade(student, grade);
        Console.WriteLine("Wystawiono ocenę.");
    }

    static void AddAttendance()
    {
        var student = SelectStudent();
        var course = SelectCourse();
        Console.Write("Data (YYYY-MM-DD): ");
        DateTime date = DateTime.Parse(Console.ReadLine());
        Console.Write("Obecny (t/n): ");
        bool isPresent = Console.ReadLine().ToLower() == "t";
        course.MarkAttendance(student, date, isPresent);
        Console.WriteLine("Dodano frekwencję.");
    }

    static void ShowAttendanceReport()
    {
        var student = SelectStudent();
        student?.ShowAttendanceReport();
    }

    static void ShowGrades()
    {
        var student = SelectStudent();
        student?.ShowGrades();
    }

    static Student SelectStudent()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("Brak studentów.");
            return null;
        }

        Console.WriteLine("Studenci:");
        for (int i = 0; i < students.Count; i++)
            Console.WriteLine($"{i + 1}. {students[i].Name}");

        Console.Write("Wybierz numer: ");
        int index = int.Parse(Console.ReadLine()) - 1;
        return students.ElementAtOrDefault(index);
    }

    static Course SelectCourse()
    {
        if (courses.Count == 0)
        {
            Console.WriteLine("Brak kursów.");
            return null;
        }

        Console.WriteLine("Kursy:");
        for (int i = 0; i < courses.Count; i++)
            Console.WriteLine($"{i + 1}. {courses[i].Title}");

        Console.Write("Wybierz numer: ");
        int index = int.Parse(Console.ReadLine()) - 1;
        return courses.ElementAtOrDefault(index);
    }
}
