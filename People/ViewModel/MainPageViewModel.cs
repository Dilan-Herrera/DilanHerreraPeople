using People.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace People.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _newPersonName;
        private string _statusMessage;

        public ObservableCollection<Person> People { get; } = new ObservableCollection<Person>();

        public string NewPersonName
        {
            get => _newPersonName;
            set
            {
                _newPersonName = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand GetAllPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public MainPageViewModel()
        {
            AddPersonCommand = new Command(AddPerson);
            GetAllPeopleCommand = new Command(GetAllPeople);
            DeletePersonCommand = new Command<int>(DeletePerson); // Comando para eliminar
        }

        private void AddPerson()
        {
            StatusMessage = "";

            App.PersonRepo.AddNewPerson(NewPersonName);
            StatusMessage = App.PersonRepo.StatusMessage;
            NewPersonName = string.Empty;
        }

        private void GetAllPeople()
        {
            StatusMessage = "";

            var people = App.PersonRepo.GetAllPeople();
            People.Clear();
            foreach (var person in people)
            {
                People.Add(person);
            }
        }

        private void DeletePerson(int id)
        {
            try
            {
                StatusMessage = "";
                App.PersonRepo.DeletePerson(id);

                App.Current.MainPage.DisplayAlert(
                    "Registro Eliminado",
                    "Dilan Herrera acaba de eliminar un registro",
                    "OK");

                StatusMessage = App.PersonRepo.StatusMessage;

                GetAllPeople();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar la persona: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
