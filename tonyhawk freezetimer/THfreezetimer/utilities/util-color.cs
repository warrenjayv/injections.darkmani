using System;

#pragma warning disable CS8981

namespace utility
{

  public static class color
  {
    public static ConsoleColor red = ConsoleColor.Red;
    public static ConsoleColor blue = ConsoleColor.Blue;
    public static ConsoleColor yellow = ConsoleColor.Yellow;
    public static ConsoleColor white = ConsoleColor.White;
    public static ConsoleColor green = ConsoleColor.Green;
    public static ConsoleColor mag = ConsoleColor.Magenta;
    public static ConsoleColor cyan = ConsoleColor.Cyan;
    public static ConsoleColor gray = ConsoleColor.Gray;
    public static ConsoleColor darkmag = ConsoleColor.DarkMagenta;


    public static void set(ConsoleColor c)
    {
      Console.ForegroundColor = c;
    }
  }
  
}