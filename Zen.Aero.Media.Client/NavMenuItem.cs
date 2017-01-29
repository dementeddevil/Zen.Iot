using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zen.Aero.Media.Client
{
    /// <summary>
    /// Data to represent an item in the nav menu.
    /// </summary>
    public class NavMenuItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        private Visibility _selectedVis = Visibility.Collapsed;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Label { get; set; }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar => (char)this.Symbol;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set
            {
                _selectedVis = value;
                this.OnPropertyChanged("SelectedVis");
            }
        }

        public Type DestPage { get; set; }

        public object Arguments { get; set; }

        public void OnPropertyChanged(string propertyName)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}