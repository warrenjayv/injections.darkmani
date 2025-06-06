using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#pragma warning disable CS8981 
namespace freezetimer
{
  public static class booter
  {
    public static void set_environment()
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;
    }
  }
  class program

  {
    static void Main()
    {
      booter.set_environment();
      utility.color.set(ConsoleColor.Blue);
      titler.generator.print("trump");
      utility.color.set(ConsoleColor.White);

      while (true) { }
    }
  }
}