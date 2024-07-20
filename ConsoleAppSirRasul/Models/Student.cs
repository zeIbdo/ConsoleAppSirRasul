namespace ConsoleAppSirRasul.Models;

public class Student
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int ClassId { get; set; }
    static int id=0;
    public int Id { get; set; }
    public Student(string name, string surname,int classId)
    {
        
        Id = ++id;
        Name = name;
        Surname = surname;
        ClassId = classId;
    }
    public override string ToString()
    {
        return $"{Id} {Name} {Surname}";
    }
}
