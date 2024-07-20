using ConsoleAppSirRasul.Exceptions;
using ConsoleAppSirRasul.Extensions;
using ConsoleAppSirRasul.Helpers;
using ConsoleAppSirRasul.Models;
using Newtonsoft.Json;

internal class Program
{
    static void Main(string[] args)
    {
        //Student student1 = new Student("Iii", "Kkk", 1);
        //Student student2 = new Student("Iii", "Kkk", 1);
        //Clasroom clasroom1 = new Clasroom("PB333", "backend");
        //clasroom1.StudentAdd(student1);
        //clasroom1.StudentAdd(student2);

        //List<Clasroom> clasrooms = new List<Clasroom> { clasroom1 };
        //var JSONresult = JsonConvert.SerializeObject(clasrooms, Formatting.Indented);
        //string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Jsons\classroom.json");
        //using (StreamWriter sw = new StreamWriter(path))
        //{
        //    sw.WriteLine(JSONresult);
        //}
        Menu();
    }

    static void Menu()
    {
        while (true)
        {

            Console.WriteLine("1. Classroom yarat");
            Console.WriteLine("2. Student yarat");
            Console.WriteLine("3. Butun Telebeleri ekrana cixart");
            Console.WriteLine("4. Secilmis sinifdeki telebeleri ekrana cixart");
            Console.WriteLine("5. Telebeni ekrana cixart");
            Console.WriteLine("6. Telebe sil");
            Console.WriteLine("7. Proqramdan cix");
            Console.WriteLine("Make a choice:");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateClassroom();
                    break;
                case "2":
                    CreateStudent();
                    break;
                case "3":
                    DisplayAllStudents();
                    break;
                case "4":
                    DisplayStudentsByClass();
                    break; 
                case "5":
                    FindStudent();
                    break;
                case "6":
                    DeleteStudent();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("duzgun deyer daxil et");
                    break;
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
        }
    }

    static void CreateClassroom()
    {
        string classroomName = GetValidClassName();
        string classType = GetValidClassType();

        Clasroom nowUsingClasroom = new(classroomName, classType);
        Clasroom.clasrooms.Add(nowUsingClasroom);
        var JSONresult = JsonConvert.SerializeObject(Clasroom.clasrooms, Formatting.Indented);
        string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Jsons\classroom.json");
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(JSONresult);
        }
        Console.WriteLine($"{classroomName} created");
    }

    static string GetValidClassName()
    {
        while (true)
        {
            Console.WriteLine("classroom name:");
            string classroomName = Console.ReadLine();
            if (classroomName.CheckClassName())
                return classroomName;

            Console.WriteLine("enter name correctly (example: PB303)");
        }
    }

    static Student FindId(int id)
    {
        foreach (var cla in Clasroom.clasrooms)
        {
            foreach (var item in cla.students)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
        }
        throw new StudentNotFoundException("Student not found");
    }
    static void FindStudent()
    {
        try
        {
            Console.WriteLine("Write id of student which you want to see:");
            DisplayAllStudents();
            int id = GetValidId();
            Console.WriteLine(FindId(id));
        }
        catch (StudentNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }
    static string GetValidClassType()
    {
        while (true)
        {
            Console.WriteLine("class type: (backend or frontend)");
            string classType = Console.ReadLine();
            if (classType.ToLower() == "backend" || classType.ToLower() == "frontend")
                return classType;

            Console.WriteLine("write backend or frontend");
        }
    }

    static void CreateStudent()
    {
        if (Clasroom.clasrooms.Count == 0)
        {
            Console.WriteLine("Create class before student");
            return;
        }

        string studentName = GetValidName("Enter name of student:");
        string studentSurname = GetValidName("Enter surname of student:");

        foreach (var item in Clasroom.clasrooms)
        {
            Console.WriteLine(item);
        }

        AA:
        try
        {
            Console.WriteLine("Type the id of class which you want to add student to that:");
            int idAdd = GetValidId();
            Clasroom classToAdd = FindClassroomById(idAdd);//Clasroom.clasrooms.First(c => c.Id == idAdd);
            Student student = new(studentName, studentSurname, Clasroom.clasrooms.First(c => c.Id == idAdd).Id);
            classToAdd.StudentAdd(student);
            Console.WriteLine($"{studentName} {studentSurname} created");
            var JSONresult = JsonConvert.SerializeObject(Clasroom.clasrooms, Formatting.Indented);
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Jsons\classroom.json");
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(JSONresult);
            }
        }
        catch (ClassroomNotFoundException e)
        {
            Console.WriteLine(e.Message);
            goto AA;
        }
        catch(StudentLimitException e) { Console.WriteLine(e.Message); }
    }

    static string GetValidName(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string name = Console.ReadLine();
            if (name.CheckName())
                return name;

            Console.WriteLine("Enter name correctly (example: Ibrahim)");
        }
    }

    static int GetValidId()
    {
        while (true)
        {
            string strAdd = Console.ReadLine();
            if (int.TryParse(strAdd, out int idAdd) && idAdd > 0)
                return idAdd;

            Console.WriteLine("Enter valid id");
        }
    }

    static void DisplayAllStudents()
    {
        if (Clasroom.clasrooms.Count == 0)
        {
            Console.WriteLine("No classrooms created");
            return;
        }

        foreach (var cla in Clasroom.clasrooms)
        {
            foreach (var student in cla.GetStudents())
            {
                Console.WriteLine(student);
            }
        }
    }

    static void DisplayStudentsByClass()
    {
        AA:
        try
        {
            if (Clasroom.clasrooms.Count == 0)
            {
                Console.WriteLine("No classrooms created");
                return;
            }

            foreach (var item in Clasroom.clasrooms)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Type the id of the class whose students you want to see:");
            int id = GetValidId();

            foreach (var student in FindClassroomById(id).GetStudents())
            {
                Console.WriteLine(student);
            }
        }
        catch (ClassroomNotFoundException e)
        {

            Console.WriteLine(e.Message);
            goto AA;
        }
    }

    static void DeleteStudent()
    {
        AA:
        try
        {
            if (Clasroom.clasrooms.Count == 0)
            {
                Console.WriteLine("No classrooms created");
                return;
            }

            foreach (var cla in Clasroom.clasrooms)
            {
                foreach (var student in cla.GetStudents())
                {
                    Console.WriteLine(student);
                }
            }

            Console.WriteLine("Type the id of the student you want to remove:");
            int idRemove = GetValidId();

            foreach (var cla in Clasroom.clasrooms)
            {
                if (cla.Delete(idRemove))
                    break;
            }
            var JSONresult = JsonConvert.SerializeObject(Clasroom.clasrooms, Formatting.Indented);
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Jsons\classroom.json");
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(JSONresult);
            }
        }
        catch (StudentNotFoundException e)
        {
            Console.WriteLine(e.Message);
            goto AA;
        }
    }

   
    static Clasroom FindClassroomById(int id)
    {
        foreach (Clasroom classroom in Clasroom.clasrooms)
        {
            if (classroom.Id == id) return classroom;
        }
        throw new ClassroomNotFoundException("class not found");
    }

}
