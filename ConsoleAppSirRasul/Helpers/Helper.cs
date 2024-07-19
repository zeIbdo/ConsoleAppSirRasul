using ConsoleAppSirRasul.Exceptions;

namespace ConsoleAppSirRasul.Helpers;

public static class Helper
{
    public static bool CheckName(this string name)
    {
        if (name.Length < 3 || !char.IsUpper(name[0]) || name.Contains(" "))
        {
            return false;
        }
        return true;
    }

    public static bool CheckClassName(this string className)
    {
        if (className.Length == 5 && char.IsUpper(className[0]) && char.IsUpper(className[1])
            && char.IsDigit(className[2]) && char.IsDigit(className[3]) && char.IsDigit(className[4]))
        {
            return true;
        }
        return false;
    }

}
