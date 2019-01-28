﻿using System.Windows.Input;

namespace AntiDuplWPF.ObjectModel
{
    public class BindableMenuItem
    {
        public string Name { get; set; }
        public BindableMenuItem[] Children { get; set; }
        public ICommand Command { get; set; }
    }
}
