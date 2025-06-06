using System;

#pragma warning disable CS8981

namespace utility
{
  public static class writer
  {
    public static void write(string msg, ConsoleColor c)
    {
      color.set(c);
      Console.WriteLine(msg);
      color.set(color.white);
    }
  
    public static void write(string word1, ConsoleColor c1, string word2, ConsoleColor c2) {
      color.set(c1); Console.Write(word1);
      color.set(c2); Console.Write(word2);
      color.set(color.white); Console.WriteLine("");
    } 
  }
}