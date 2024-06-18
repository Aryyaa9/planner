namespace GarageConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        bool Permission = false;
        int IdUser = 0;

        while (!Permission)
        {
            (Permission, IdUser) = DatabaseRequests.AuthAndRegis();
        }

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Добавить задачу");
            Console.WriteLine("2 - Удалить задачу");
            Console.WriteLine("3 - Редактировать задачу");
            Console.WriteLine("4 - Просмотреть задачи на сегодня");
            Console.WriteLine("5 - Просмотреть задачи на завтра");
            Console.WriteLine("6 - Просмотреть задачи на неделю");
            Console.WriteLine("7 - Просмотреть все предстоящие задачи");
            Console.WriteLine("8 - Просмотреть все выполненные задачи");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DatabaseRequests.AddTask(IdUser);
                    break;
                case 2:
                    DatabaseRequests.DeleteTask(IdUser);
                    break;
                case 3:
                    DatabaseRequests.EditTask(IdUser);
                    break;
                case 4:
                    DatabaseRequests.ViewTasksToday(IdUser, DateTime.Today);
                    break;
                case 5:
                    DatabaseRequests.ViewTasksTomorrow(IdUser, DateTime.Today.AddDays(1));
                    break;
                case 6:
                    DatabaseRequests.ViewTasksWeek(IdUser, DateTime.Today, DateTime.Today.AddDays(7));
                    break;
                case 7:
                    DatabaseRequests.ViewAllUpcomingTasks(IdUser);
                    break;
                case 8:
                    DatabaseRequests.ViewAllCompletedTasks(IdUser);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор.");
                    break;

            }
        }
    }
}