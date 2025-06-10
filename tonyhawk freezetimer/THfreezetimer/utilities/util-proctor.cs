using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Buffers.Binary;

#pragma warning disable CS8981

using freezetimer.switchboard;

namespace utility
{
  public class proctor
  {
    [DllImport("kernel32.dll")]
    static extern int GetLastError();

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(int hProcess, int IpBaseAddress, byte[] IpBuffer, int dwSize, ref int IpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr IpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr IpAddress, uint dwSize, uint lAllocationType);

    [DllImport("kernel32.dll")]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int wSize);

    const int PAGE_READWRITE = 0x40;
    const int PROCESS_WM_READ = 0x0010;
    const int PROCESS_VM_WRITE = 0x0020;
    const int PROCESS_ALL_ACCESS = 0x01F0FFF;

    const int MEM_COMMIT = 0x1000;
    const int MEM_RELEASE = 0x8000;
    const int MEM_RESERVE = 0x2000;

    /* instruction offset */
    static Int64 instrOFF = Convert.ToInt64("46585A", 16);

    /* shell offset */
    static Int64 shellOFF = Convert.ToInt64("46580D", 16);

    /* injection bytecode ~ dont use, it is hardcoded */
    static Int64 instrINJ = Convert.ToInt64("E90858C9FE", 16);

    // public static byte[] shellcode = Convert.FromHexString("E90858C9FE"); 
    public static byte[] original_code = new byte[5];

    public static string target = "THHDGame";

    public static int id = 0;

    // size of the allocated shellcode. 
    public static int allocSZ = 0;

    public static Process procPTR;
    public static IntPtr procHND;
    public static IntPtr startOFF;
    public static IntPtr entryPNT;
    public static Int64 instADDR;
    public static IntPtr allocADDR;

    public static void inject()
    {
      int wBytes = 0;
      int rBytes = 0;

      // STORE ORIGINAL CODE
      writer.write("• storing original code...", color.blue);
      ReadProcessMemory((int)procPTR.Handle, (int)instADDR, original_code, original_code.Length, ref rBytes);
      
      if ( rBytes > 0 )
      {
        writer.write("• " + rBytes.ToString() + " bytes read. code: " + BitConverter.ToString(original_code), color.blue);
      }
      else
      {
        int errCODE = GetLastError( );
        writer.write("proctor.inject()<error>: FAILED to read original code.", color.red);
        writer.write(String.Format("system error code: {0}", errCODE), color.red);
        return;
      }
      
      // ALLOCATE SHELLCODE ADDRESS {  }
      writer.write("• injecting...", color.blue);
      writer.write("• allocate shellcode...", color.blue);
      allocADDR = VirtualAllocEx(procPTR.Handle, 0, 1000, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
      if (allocADDR == 0)
      {
        writer.write("proctor.inject()<error>: FAILED to allocate shellcode.", color.red);
        return;
      }
      writer.write("• shellcode address: " + allocADDR.ToString("X"), color.blue);

      // ASSEMBLE SHELLCODE {  } 
      /* remember, the address is relative here...*/
      int offset = (int)(startOFF + shellOFF) - (int)allocADDR;
      byte[] shellcode = new byte[] { 0xE9 }.Concat(BitConverter.GetBytes(offset - 5)).ToArray();
      allocSZ = shellcode.Length;
      writer.write("• assembling shellcode: " + BitConverter.ToString(shellcode), color.blue);
      //byte[] shellcode = new byte[] { 0xE9, 0x0 };

      if (WriteProcessMemory(procPTR.Handle, (IntPtr)allocADDR, shellcode, (uint)shellcode.Length, out wBytes))
      {
        writer.write("• " + wBytes.ToString() + " bytes written.", color.blue);
      }
      else
      {
        writer.write("proctor.inject()<error>: FAILED to allocate shellcode.", color.red);
      }

      // ASSEMBLE JUMP ADDRESS {  }
      writer.write("• assemble jump instruction...", color.blue);
      offset = (int)(allocADDR - instADDR);
      writer.write("• offset: " + offset.ToString("X"), color.blue);
      byte[] jmp = new byte[] { 0xE9 }.Concat(BitConverter.GetBytes(offset - 5)).Concat(new byte[] { 0x90 }).ToArray();
      //byte[] jmp = new byte[] { 0XE9, 0x00, 0x00, 0x00, 0x00 };

      if (WriteProcessMemory(procPTR.Handle, (IntPtr)instADDR, jmp, (uint)jmp.Length, out wBytes))
      {
        writer.write("• jump code: " + BitConverter.ToString(jmp), color.blue);
      }
      else
      {
        writer.write("proctor.inject()<error>: FAILED to assemble jump instruction.", color.red);
      }

      flags.INJECTED = 1;

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

      writer.write("• attaching to process...", color.blue);
      procPTR = Process.GetProcessById(id);

      procHND = OpenProcess(PROCESS_ALL_ACCESS, false, procPTR.Id);

      if (procHND == null)
      {
        writer.write("proctor.inject()<error>: failed to open process.", color.red);
        return;
      }

      writer.write("process opened.", color.blue);
      startOFF = procPTR.MainModule.BaseAddress;
      entryPNT = procPTR.MainModule.EntryPointAddress;

      writer.write("• base address: " + startOFF.ToString("X"), color.blue);

      instADDR = startOFF + instrOFF;
      writer.write("• instruction address: " + instADDR.ToString("X"), color.blue);

      flags.FOUND = 1;
    }

    public static void eject()
    {
      
      // FREE SHELL CODE
      if (VirtualFreeEx(procPTR.Handle, allocADDR, 0, MEM_RELEASE))
      {
        writer.write("• shellcode allocation ejected @ " + allocADDR.ToString("X"), color.green);
      }
      else
      {
        int errCODE = GetLastError();
        writer.write("proctor.eject()<error>: FAILED to free shellcode", color.red);
        writer.write("address: " + allocADDR.ToString("X"), color.red);
        writer.write(String.Format("system error code: {0}", errCODE), color.red);
        return;
      }

      // RESTORE ORIGINAL CODE
      int wBytes = 0;

      if (WriteProcessMemory(procPTR.Handle, (IntPtr)instADDR, original_code, (uint)original_code.Length, out wBytes))
      {
        writer.write("• original code restored: " + BitConverter.ToString(original_code), color.blue); 
      }
      else
      {
        writer.write("proctor.inject()<error>: FAILED to assemble jump instruction.", color.red);
      }

      flags.INJECTED = 0;
    } 
  }
}