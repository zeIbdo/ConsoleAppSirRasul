using ConsoleAppSirRasul.Exceptions;
using ConsoleAppSirRasul.Extensions;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace ConsoleAppSirRasul.Models;

public class Clasroom
{
    static int id;
    public int Id {get; set;}
    public string Name { get; set; }
    public override string ToString()
    {
        return $"{Id} {Name}";
    }
    public List<Student> students;
    public static List<Clasroom> clasrooms = new List<Clasroom>();
    public int Limit { get; set; }
    public Clasroom()
    {
        students = new List<Student>();
    }
    public Clasroom(string name, string enumValue)
    {
        Id = ++id;
        Name = name;
        if (enumValue.ToLower() == ClassType.frontend.ToString())
            Limit = Convert.ToInt32(ClassType.frontend);
        else if (enumValue.ToLower() == ClassType.backend.ToString())
            Limit = Convert.ToInt32(ClassType.backend);
        students = new List<Student>();
    }
    public void AddClassroom()
    {
        clasrooms.Add(this);
    }
    public List<Student> GetStudents()
    {
        return students;
    }
    public List<Clasroom> GetClasrooms()
    {
        return clasrooms;
    }
   
    public void StudentAdd(Student student)
    {
        if (students.Count>= Limit)
        {
         throw new StudentLimitException("Limite catibsiniz");
        }
            students.Add(student);
    }
    public List<Student> GetStudentsByClass(int classId)
    {
        var studentsInClass = students.FindAll(s => s.ClassId == classId);
        if (studentsInClass.Count == 0)
        {
            throw new ClassroomNotFoundException("Student not found");
        }
        return studentsInClass;
    }
   
    public bool Delete(int id)
    {
        var student = students.Find(s => s.Id == id);
        if (student != null)
        {
            students.Remove(student);
            return true;
        }
        else
        {
            throw new StudentNotFoundException("Student not found");
        }
    }

    public static void ResetIdCounter()
    {
        if (clasrooms.Count > 0)
        {
            id = clasrooms.Max(c => c.Id);
        }
        else
        {
            id = 0;
        }
    }
    public static bool SameName(string name)
    {
        foreach(var cla in clasrooms) { 
        if(cla.Name == name) return false;
        }
        return true;
    }


}
