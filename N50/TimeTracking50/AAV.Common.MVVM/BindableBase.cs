using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace MVVM.Common
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// AP: A copy from a w8 demo project
    /// </summary>
    //[Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <x name="storage">Reference to a property with both getter and setter.</x>
        /// <x name="value">Desired value for the property.</x>
        /// <x name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can bo provided automatically when invoked from compilers that
        /// support CallerMemberName.</x>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <x name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can bo provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</x>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}