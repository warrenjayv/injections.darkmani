using System;

using freezetimer.switchboard;

namespace utility
{
  public static class settings
  {
    static string _content = "";

    public static void set()
    {
      try
      {
        read(); parse();
      }
      catch (Exception e)
      {
        writer.write("settings file error\n• e.Message", color.red);
      }
    }
    public static void read()
    {
      string _path = @"./settings";
      _content = "";

      using (FileStream _fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
      using (TextReader _sr = new StreamReader(_fs))
      {
        string line;
        while ((line = _sr.ReadLine()) != null)
        {
          _content += line + '\n';
        }
      }
    }

    public static void parse()
    {
      if (_content.Length > 0)
      {
        string[] _prms = _content.Split('\n');

        for (int i = 0; i < _prms.Length; i++)
        {
          string[] _prse = _prms[i].Split(' ');

          if (_prse[0] == "LIBERAL" && _prse.Length > 2) { flags.LIBERAL = int.Parse(_prse[2]);  }


        }
      }
    }
  }
}