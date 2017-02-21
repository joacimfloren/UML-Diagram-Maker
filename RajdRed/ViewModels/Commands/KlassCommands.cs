﻿using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace RajdRed.ViewModels.Commands
{
    public class KlassCommands : ICommand
    {
        public KlassViewModel KlassViewModel { get; set; }
        public event EventHandler CanExecuteChanged;

        public KlassCommands(KlassViewModel kvm)
        {
            KlassViewModel = kvm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            UserControl uc = parameter as UserControl;
        }
    }
}