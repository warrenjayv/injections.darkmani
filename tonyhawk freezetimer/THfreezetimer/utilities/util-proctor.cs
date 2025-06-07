using System;
using System.Diagnostics;

#pragma warning disable CS8981

namespace utility
{
  public class proctor
  {
    public static string target = "THHDGame";

    public static int id = 0;

    public static void findtarget()
    {
      Process[] pARR = Process.GetProcessesByName(target);

      if (pARR.Length < 1)
      {
        writer.write("utility.proctor.error: GAME NOT FOUND", color.red);
        writer.write("did you run the game?", color.red);
        return;
      }

      writer.write("searching for program...", color.white);

      for (int i = 0; i < pARR.Length; i++)
      {
        writer.write(pARR[i].MainWindowTitle + "   " + pARR[i].Id.ToString(), color.green);
      }

      // get manual input if array is large
      if (pARR.Length > 1)
      {
        writer.write("please enter the program process id: ", color.white);
        try
        {
          if (Int32.TryParse(Console.ReadLine(), out int val))
          {
            id = val;
            writer.write("target found.", color.green);
          }
        }
        catch (Exception e)
        {
          writer.write("proctor.findtarget()<error>: invalid entry [is a number?]", color.red);
          writer.write(e.Message, color.red);
        }
      }
      else
      {
        id = pARR[0].Id;
        writer.write("target found. " + "[" + id.ToString() + "]", color.green);
      }

    }
    
  }
}