using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using PropertyChanged;

namespace MachineLearningLab3.Basic
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
