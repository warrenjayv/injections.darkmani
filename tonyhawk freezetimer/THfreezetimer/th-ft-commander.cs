using System;
using utility;
#pragma warning disable CS8981

namespace freezetimer
{
  namespace switchboard
  {
    public static class flags
    {
      public static int LIBERAL = 0;
      public static int INJECTED = 0;
      public static int FOUND = 0;

      public static bool assert_injection()
      {
        if (INJECTED == 1)
        {
          writer.write("! target already injected. /e to eject.", color.red);
          return false;
        }
        return true;
      }

      public static bool assert_ejection()
      {
        if (INJECTED == 0)
        {
          writer.write("! target already ejected. /i to inject.", color.red);
          return false;
        }
        return true;
      }

      public static bool assert_found()
      {
        if (FOUND == 1)
        {
          writer.write("! target already found or attached.", color.green);
          return false;
        }
        return true;
      }
    }

    public static class response
    {
      static string h = "available commands:\n /f - FIND target program & attach if found\n /i - INJECT shellcode\n /e - EJECT shellcode\n /h - HELP - show command list";
      public static void read_command(string user)
      {
        switch (user)
        {
          case "/h":
            writer.write(h, color.gray);
            break;
          case "/f":
            if ( flags.assert_found())    proctor.findtarget();
            break;
          case "/i":
            if ( flags.assert_injection()) proctor.inject();
            break;
          case "/e":
            if ( flags.assert_ejection())  proctor.eject();
            break;
          default:
            writer.write(h, color.gray);
            break;
        }
      }
      
    }
    
  }
}