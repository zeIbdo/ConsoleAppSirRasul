﻿using ConsoleAppSirRasul.Exceptions;
using ConsoleAppSirRasul.Extensions;
using ConsoleAppSirRasul.Helpers;
using ConsoleAppSirRasul.Models;
using Newtonsoft.Json;
using System.IO;

internal class Program
{

    static void Main(string[] args)
    {
        LoadData();
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
    static void LoadData()
    {
        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Jsons\classroom.json");

        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            var deserializedClassrooms = JsonConvert.DeserializeObject<List<Clasroom>>(jsonData) ?? new List<Clasroom>();
            Clasroom.clasrooms = deserializedClassrooms;
            Clasroom.ResetIdCounter(); 
            foreach (var classroom in Clasroom.clasrooms)
            {
                if (classroom.students == null) 
                {
                    classroom.students = new List<Student>();
                }
                foreach (var student in classroom.GetStudents())
                {
                    Student.UpdateIdCounter(student.Id); 
                }
            }
        }
    }
    static void CreateClassroom()
    {
        string classroomName = GetValidClassName();
        if (Clasroom.SameName(classroomName) == false) { Console.WriteLine("Eyni ad ile class yaradila bilmez"); return; }
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
            if (Clasroom.clasrooms.Count == 0) {
                Console.WriteLine("no classrooms created");
                return;
                    }
            else {
                int count=0;
                foreach(var c in Clasroom.clasrooms) { count += c.GetStudents().Count; };
                if (count == 0) { Console.WriteLine("no students created"); return; }
                else
                {
                    Console.WriteLine("Write id of student which you want to see:");
                    DisplayAllStudents();
                    int id = GetValidId();
                    Console.WriteLine(FindId(id));
                }
            }
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
        int count = 0;
        foreach (var c in Clasroom.clasrooms) { count += c.GetStudents().Count; };
        if (count == 0) { Console.WriteLine("no students created"); return; }
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
        try
        {
            if (Clasroom.clasrooms.Count == 0)
            {
                Console.WriteLine("No classrooms created");
                return;
            }
            else
            {
                int count = 0;
                foreach (var c in Clasroom.clasrooms) { count += c.GetStudents().Count; };
                if (count == 0) { Console.WriteLine("no students created"); return; }
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
        }
        catch (ClassroomNotFoundException e)
        {

            Console.WriteLine(e.Message);
        }
    }

    static void DeleteStudent()
    {
        
        try
        {
            if (Clasroom.clasrooms.Count == 0)
            {
                Console.WriteLine("No classrooms created");
                return;
            }
            else
            {
                int count = 0;
                foreach (var c in Clasroom.clasrooms) { count += c.GetStudents().Count; };
                if (count == 0) { Console.WriteLine("no students created"); return; }
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
        }
        catch (StudentNotFoundException e)
        {
            Console.WriteLine(e.Message);
            
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
