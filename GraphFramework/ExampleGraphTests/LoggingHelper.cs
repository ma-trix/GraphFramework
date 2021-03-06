﻿using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;

namespace ExampleGraphTests
{
    public class LoggingHelper
    {
        public void ClearLogFile()
        {
            FileAppender fileAppender = LogManager.GetRepository()
                                                  .GetAppenders().FirstOrDefault(appender => appender is FileAppender) as FileAppender;
            
            if (fileAppender != null && File.Exists(fileAppender.File))
            {
                string path = fileAppender.File;
                FileAppender curAppender = fileAppender;
                curAppender.File = path;

                FileStream fs = null;
                try
                {
                    fs = new FileStream(path, FileMode.Create);
                }
                catch (Exception ex)
                {
                    (LogManager.GetLogger(GetType())).Error("Could not clear the file log", ex);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }

                }
            }
        }
    }
}