using People.Models;
using SQLite;

namespace People;

public class PersonRepository
{
    string _dbPath;
    private SQLiteConnection conn;

    public string StatusMessage { get; set; }

    private void Init()
    {
        if (conn != null)
            return;

        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Person>();
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddNewPerson(string name)
    {
        int result = 0;
        try
        {
            Init();

            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            result = conn.Insert(new Person { Name = name });

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        }
    }

    public List<Person> GetAllPeople()
    {
        try
        {
            Init();
            return conn.Table<Person>().ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }

    public void DeletePerson(int id)
    {
        try
        {
            Init();

            var personToDelete = conn.Find<Person>(id);
            if (personToDelete == null)
                throw new Exception("No se encontro a la persona");

            conn.Delete(personToDelete);
            StatusMessage = $"Persona con ID {id} eliminado correctamente";
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Error al eliminar la perosna Error: {0}", ex.Message);
        }
    }
}
