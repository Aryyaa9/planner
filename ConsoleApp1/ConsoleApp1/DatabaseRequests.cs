using GarageConsoleApp;
using Npgsql;
namespace GarageConsoleApp;

public static class DatabaseRequests
{
    public static (bool, int) AuthAndRegis()
    {
        Console.Write("Выберите действие: 1 - авторизация, 2 - регистрация");
        int choice = Convert.ToInt32(Console.ReadLine());

        if (choice == 1)
        {
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            (bool A, int id) = AuthUser(login, password);
            if (A)
            {
                Console.WriteLine("Авторизация успешна!");
                return (true, id);
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
            }
        }

        else if (choice == 2)
        {
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            (bool A, int id) = RegisUser(login, password);
            if (A)
            {
                Console.WriteLine("Пользователь зарегистрирован!");
                return (true, id);
            }
            else
            {
                Console.WriteLine("Ошибка регистрации.");
            }
        }
        else
        {
            Console.WriteLine("Некорректный выбор.");
        }

        return (false, 0);
    }

    static (bool, int) AuthUser(string login, string password)
    {
        var sql = $"select id_user from users where login = '{login}' and password_user = '{password}'";

        using var cmd = new NpgsqlCommand(sql, DatabaseService.GetSqlConnection());
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        return (count > 0, count);
    }

    static (bool, int) RegisUser(string login, string password)
    {
        var sql = $"insert into users (login, password_user) values ('{login}', '{password}')";

        using var cmd = new NpgsqlCommand(sql, DatabaseService.GetSqlConnection());
        {
            cmd.ExecuteNonQuery();
        }

        return AuthUser(login, password);
    }


    public static void AddTask(int id)
    {
        Console.Write("Введите название задачи: ");
        string name = Console.ReadLine();
        Console.Write("Введите описание задачи: ");
        string description = Console.ReadLine();
        Console.Write("Введите дату задачи в формате ГГГГ-ММ-ДД: ");
        Console.Write("Введите день: ");
        int day = int.Parse(Console.ReadLine());
        Console.Write("Введите месяц: ");
        int month = int.Parse(Console.ReadLine());
        Console.Write("Введите год: ");
        int year = int.Parse(Console.ReadLine());
        DateTime date = new DateTime(year, month, day);


        using (var cmd = new NpgsqlCommand($"INSERT INTO tasks (name, id_user, description, date_task) VALUES ('{name}', '{id}', '{description}', '{date}')", DatabaseService.GetSqlConnection()))
        {
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                Console.WriteLine("Задача успешно добавлена!");
            }
            else
            {
                Console.WriteLine("Ошибка добавления задачи.");
            }
        }
    }
    public static void DeleteTask(int id)
    {
        Console.Write("Введите название задачи для удаления: ");
        string name = Console.ReadLine();

        using (var cmd = new NpgsqlCommand($"DELETE FROM tasks WHERE name = '{name}'", DatabaseService.GetSqlConnection()))
        {
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                Console.WriteLine("Задача успешно удалена!");
            }
            else
            {
                Console.WriteLine("Ошибка удаления задачи.");
            }
        }
    }

    public static void EditTask(int id)
    {
        Console.Write("Введите название задачи для редактирования: ");
        string name = Console.ReadLine();
        Console.Write("Введите новое название задачи: ");
        string newName = Console.ReadLine();
        Console.Write("Введите новое описание задачи: ");
        string newDescription = Console.ReadLine();
        Console.Write("Введите новую дату задачи в формате ГГГГ-ММ-ДД: ");
        Console.Write("Введите день: ");
        int day = int.Parse(Console.ReadLine());
        Console.Write("Введите месяц: ");
        int month = int.Parse(Console.ReadLine());
        Console.Write("Введите год: ");
        int year = int.Parse(Console.ReadLine());
        DateTime newDate = new DateTime(year, month, day);


        using (var cmd = new NpgsqlCommand($"UPDATE tasks SET name = '{newName}', description = '{newDescription}', date_task = '{newDate}' WHERE id_user = '{id}' and name = '{name}'", DatabaseService.GetSqlConnection()))
        {
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                Console.WriteLine("Задача успешно отредактирована!");
            }
            else
            {
                Console.WriteLine("Ошибка редактирования задачи.");
            }
        }

    }

    public static void ViewTasksToday(int id, DateTime today)
    {
        using (var cmd = new NpgsqlCommand($"SELECT * FROM tasks WHERE id_user = '{id}' AND date_task = '{today}'", DatabaseService.GetSqlConnection()))
        {
            using var reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                Console.WriteLine($"Название: {reader[0]}, Описание: {reader[2]}, Дата: {reader[3]}");
            }
        }
    }

    public static void ViewTasksTomorrow(int id, DateTime tomorrow)
    {
        using (var cmd = new NpgsqlCommand($"SELECT * FROM tasks WHERE id_user = '{id}' AND date_task = '{tomorrow}'", DatabaseService.GetSqlConnection()))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Название: '{reader[0]}', Описание: '{reader[1]}', Дата: '{reader[2]}'");
                }
            }
        }
    }

    public static void ViewTasksWeek(int id, DateTime startDate, DateTime endDate)
    {
        using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM tasks WHERE id_user = '{id}' AND date_task >= '{startDate}' AND date_task <= '{endDate}'", DatabaseService.GetSqlConnection()))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Название: {reader[0]}, Описание: {reader[1]}, Дата: {reader[2]}");
                }
            }
        }
    }


    public static void ViewAllUpcomingTasks(int id)
    {
        Console.WriteLine("Все предстоящие задачи:");
        using (var cmd = new NpgsqlCommand($"SELECT * FROM tasks WHERE date_task >= '{DateTime.Today}' ORDER BY date_task", DatabaseService.GetSqlConnection()))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Название: {reader[0]}, Описание: {reader[1]}, Дата: {reader[2]}");
                }
            }
        }
    }


    public static void ViewAllCompletedTasks(int id)
    {
        Console.WriteLine("Все выполненные задачи:");

        using (var cmd = new NpgsqlCommand($"SELECT * FROM tasks WHERE date_task < '{DateTime.Today}' ORDER BY date_task DESC", DatabaseService.GetSqlConnection()))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Название: {reader[0]}, Описание: {reader[1]}, Дата: {reader[2]}");
                }
            }
        }
    }
}


