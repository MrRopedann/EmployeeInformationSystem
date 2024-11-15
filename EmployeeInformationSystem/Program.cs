using EmployeeInformationSystem.Interface;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Repository;
using EmployeeInformationSystem.Service;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        const string ConnectionString = "Server=DESKTOP-FQGHNUM;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True;";

        IEmployeeRepository repository = new EmployeeRepository(ConnectionString);
        EmployeeService service = new EmployeeService(repository);

        Console.WriteLine("Добро пожаловать в систему управления сотрудниками!");

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Добавить сотрудника");
            Console.WriteLine("2. Посмотреть всех сотрудников");
            Console.WriteLine("3. Обновить информацию о сотруднике");
            Console.WriteLine("4. Удалить сотрудника");
            Console.WriteLine("5. Выйти");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddEmployee(service);
                    break;
                case "2":
                    ViewEmployees(service);
                    break;
                case "3":
                    UpdateEmployee(service);
                    break;
                case "4":
                    DeleteEmployee(service);
                    break;
                case "5":
                    Console.WriteLine("Завершение работы...");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Повторите попытку.");
                    break;
            }
        }
    }

    static void AddEmployee(EmployeeService service)
    {
        string firstName;
        while (true)
        {
            Console.Write("Введите имя: ");
            firstName = Console.ReadLine();
            if (IsValidName(firstName))
            {
                break; // Корректное имя
            }
            else
            {
                Console.WriteLine("Имя должно содержать только буквы и начинаться с заглавной буквы. Попробуйте снова.");
            }
        }

        string lastName;
        while (true)
        {
            Console.Write("Введите фамилию: ");
            lastName = Console.ReadLine();
            if (IsValidName(lastName))
            {
                break; // Корректная фамилия
            }
            else
            {
                Console.WriteLine("Фамилия должна содержать только буквы и начинаться с заглавной буквы. Попробуйте снова.");
            }
        }

        string email;
        while (true)
        {
            Console.Write("Введите email: ");
            email = Console.ReadLine();
            if (IsValidEmail(email))
            {
                break; // Корректный email
            }
            else
            {
                Console.WriteLine("Некорректный формат email. Попробуйте снова.");
            }
        }

        DateTime dob;
        while (true)
        {
            Console.Write("Введите дату рождения (yyyy-MM-dd): ");
            string dobInput = Console.ReadLine();
            if (DateTime.TryParse(dobInput, out dob))
            {
                int age = DateTime.Now.Year - dob.Year;
                if (dob > DateTime.Now.AddYears(-age)) age--; // Уточняем возраст
                if (age >= 18 && age <= 100)
                {
                    break; // Корректная дата
                }
                else
                {
                    Console.WriteLine("Возраст должен быть от 18 до 100 лет. Попробуйте снова.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат даты. Используйте формат yyyy-MM-dd.");
            }
        }

        Console.Write("Введите зарплату: ");
        decimal salary;
        while (!decimal.TryParse(Console.ReadLine(), out salary) || salary <= 0)
        {
            Console.WriteLine("Введите корректное положительное число для зарплаты.");
        }

        var employee = new Employee
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            DateOfBirth = dob,
            Salary = salary
        };

        try
        {
            service.AddEmployee(employee);
            Console.WriteLine("Сотрудник успешно добавлен!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении сотрудника: {ex.Message}");
        }
    }

    static bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Регулярное выражение для проверки имени
        string namePattern = @"^[A-ZА-ЯЁ][a-zа-яё]+$";
        return Regex.IsMatch(name, namePattern);
    }

    static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Регулярное выражение для проверки формата email
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }


    static void ViewEmployees(EmployeeService service)
    {
        var employees = service.GetAllEmployees();

        Console.WriteLine("\nСписок сотрудников:");
        Console.WriteLine("ID\tИмя\tФамилия\tEmail\tДата рождения\tЗарплата");
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.EmployeeID}\t{employee.FirstName}\t{employee.LastName}\t{employee.Email}\t{employee.DateOfBirth:yyyy-MM-dd}\t{employee.Salary}");
        }
    }

    static void UpdateEmployee(EmployeeService service)
    {
        Console.Write("Введите ID сотрудника, данные которого нужно обновить: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID должен быть числом. Попробуйте снова.");
            return;
        }

        Console.WriteLine("Что вы хотите обновить?");
        Console.WriteLine("1. Имя");
        Console.WriteLine("2. Фамилию");
        Console.WriteLine("3. Email");
        Console.WriteLine("4. Дата рождения");
        Console.WriteLine("5. Зарплата");
        string choice = Console.ReadLine();

        string column = choice switch
        {
            "1" => "FirstName",
            "2" => "LastName",
            "3" => "Email",
            "4" => "DateOfBirth",
            "5" => "Salary",
            _ => throw new Exception("Неверный выбор")
        };

        object value;

        switch (column)
        {
            case "FirstName":
            case "LastName":
                while (true)
                {
                    Console.Write($"Введите новое значение для {column}: ");
                    string input = Console.ReadLine();
                    if (IsValidName(input))
                    {
                        value = input;
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"{column} должно содержать только буквы и начинаться с заглавной буквы.");
                    }
                }
                break;

            case "Email":
                while (true)
                {
                    Console.Write("Введите новый email: ");
                    string input = Console.ReadLine();
                    if (IsValidEmail(input))
                    {
                        value = input;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Некорректный формат email. Попробуйте снова.");
                    }
                }
                break;

            case "DateOfBirth":
                while (true)
                {
                    Console.Write("Введите новую дату рождения (yyyy-MM-dd): ");
                    string input = Console.ReadLine();
                    if (DateTime.TryParse(input, out DateTime dob))
                    {
                        int age = DateTime.Now.Year - dob.Year;
                        if (dob > DateTime.Now.AddYears(-age)) age--; // Уточняем возраст
                        if (age >= 18 && age <= 100)
                        {
                            value = dob;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Возраст должен быть от 18 до 100 лет. Попробуйте снова.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный формат даты. Используйте формат yyyy-MM-dd.");
                    }
                }
                break;

            case "Salary":
                while (true)
                {
                    Console.Write("Введите новую зарплату: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal salary) && salary > 0)
                    {
                        value = salary;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Зарплата должна быть положительным числом. Попробуйте снова.");
                    }
                }
                break;

            default:
                throw new Exception("Неверный выбор");
        }

        try
        {
            service.UpdateEmployee(id, column, value);
            Console.WriteLine("Данные сотрудника успешно обновлены!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении данных: {ex.Message}");
        }
    }

    static void DeleteEmployee(EmployeeService service)
    {
        Console.Write("Введите ID сотрудника для удаления: ");
        int id;

        // Проверяем, что ID введен корректно
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out id) && id > 0)
            {
                break; // Корректный ID
            }
            else
            {
                Console.WriteLine("Некорректный ID. Введите положительное число.");
                Console.Write("Введите ID сотрудника для удаления: ");
            }
        }

        try
        {
            service.DeleteEmployee(id);
            Console.WriteLine("Сотрудник успешно удален!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении сотрудника: {ex.Message}");
        }
    }

}