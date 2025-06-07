using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#pragma warning disable CS8981

namespace utility
{
  public class proctor
  {
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(int hProcess, int IpBaseAddress, byte[] IpBuffer, int dwSize, ref int IpNumberOfBytesRead);
    const int PROCESS_WM_READ = 0x0010;
    const int PROCESS_VM_WRITE = 0x0020;
    const int PROCESS_ALL_ACCESS = 0x01F0FFF;

    static Int64 instrOFF = Convert.ToInt64("46585A", 16);
    static Int64 instrINJ = Convert.ToInt64("E9085840FC", 16);

    public static string target = "THHDGame";

    public static int id = 0;

    public static void inject()
    {

      writer.write("• initiating injection...", color.blue);
      Process proc = Process.GetProcessById(id);

      IntPtr procHND = OpenProcess(PROCESS_ALL_ACCESS, false, proc.Id);

      if (procHND == null)
      {
        writer.write("proctor.inject()<error>: failed to open process.", color.red);
      }

      IntPtr startOFF = proc.MainModule.BaseAddress;
      IntPtr entryPNT = proc.MainModule.EntryPointAddress;
      Int64 instADDR = startOFF + instrOFF; 
      writer.write("• base address: " + startOFF.ToString(), color.blue);
      writer.write("• instruction address: " + instADDR.ToString(), color.blue );

      

    }

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