using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IValueHolderReadOnly<T> : INotifyPropertyChanged
    {
        T Value { get; }
    }

    public interface IValueHolder<T> : IValueHolderReadOnly<T>
    {
        new T Value { get; set; }
    }

    public class ValueHolder<T> : IValueHolder<T>
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;
                    PropertyChanged.Raise(() => Value);
                }
            }
        }

        T IValueHolderReadOnly<T>.Value { get { return _value; } }

        public ValueHolder(T val)
        {
            _value = val;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
