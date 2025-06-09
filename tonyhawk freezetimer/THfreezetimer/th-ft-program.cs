using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using freezetimer.switchboard;
using utility;

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
      titler.generator.print_title();
      writer.write("type /h to see commands", color.gray);
      while (true)
      {
        try
        {

          response.read_command(Console.ReadLine());
        }
        catch (Exception e)
        {
          writer.write(e.Message, color.red);
        }
      }

    }
  }
}