﻿using System;

namespace AntiDuplWPF.ObjectModel
{
    public class LogEntry
    {
        public LogEntry(string message)
        {
            Message = message;
            Time = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }

        public string Time { get; private set; }
        public string Message { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Time, Message);
        }
    }
}
