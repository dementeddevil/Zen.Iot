using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;

namespace Zen.Aero.Media.Client
{
    /// <summary>
    /// Data to represent an item in the nav menu.
    /// </summary>
    public class NavMenuItem : ViewModelBase
    {
        private bool _isSelected;
        private Visibility _selectedVis = Visibility.Collapsed;

        public string Label { get; set; }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar => (char)Symbol;

        public Type DestPage { get; set; }

        public object Arguments { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                RaisePropertyChanged();
            }
        }

        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set
            {
                _selectedVis = value;
                RaisePropertyChanged();
            }
        }
    }
}