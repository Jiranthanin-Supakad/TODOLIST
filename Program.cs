using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleAppDemo;

class Program
{
    static void Main(string[] args)
    {
        List<TodoEntry> todoList = new List<TodoEntry>()
        {
            new TodoEntry("Sample Todo"),
            new TodoEntry("Due Todo", dueDate: DateTime.Now.AddDays(3))
        };

        while (true)
        {
            Console.WriteLine("Enter command (type \"exit\" to quit): ");
            var command = Console.ReadLine();

            if (command == "exit")
            {
                break;
            }
            if (string.IsNullOrEmpty(command))
            {
                continue;
            }

            if (command.StartsWith("create"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: create <todo-name> [<todo-description>] [<todo-due-date>]");
                    continue;
                }

                DateTime dueDate = default;
                bool hasDueDate = todoParams.Length == 4 && DateTime.TryParse(todoParams[3], out dueDate);
                DateTime? dueDateParam = hasDueDate ? dueDate : null;

                var newEntry = new TodoEntry(todoParams![1], (todoParams.Length >= 3 ? todoParams[2] : null), dueDateParam);
                todoList.Add(newEntry);

                string dueDateMessage = hasDueDate ? $"(Due date: {dueDateParam})" : "";
                Console.WriteLine($"Added '{newEntry.Title}' to Todo List {dueDateMessage}");
            }

            // command "list"
            else if (command.StartsWith("list"))
            {

                Console.WriteLine("{0,-40} {1,-30} {2,-25} {3,0}", "Id", "Title", "Description", "DueDate");
                Console.WriteLine("{0,-40} {1,-30} {2,-20} {3,0}","-------------", "---------------------", "-------------------------", "-------------------------");
                todoList.ForEach(i => Console.WriteLine($"{i.Id,-40} {i.Title,-30} {i.Description,-25} {i.DueDate}"));

            }

            // command "remove" by Guid
            else if (command.StartsWith("remove"))
            {
                string[] todoParams = command.Split(" ", 2); 
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: remove <todo-Title>");
                    Console.WriteLine("\n");
                    continue;
                }

                string todoTitle = todoParams[1]; 

                TodoEntry removeEntry = todoList.Find(i => i.Title.Equals(todoTitle)); 

                if (removeEntry != null)
                {
                    todoList.Remove(removeEntry);
                    Console.WriteLine($"Removed todo : '{removeEntry.Id}' '{removeEntry.Title}'");
                }
                else
                {
                    Console.WriteLine($"Todo with title '{todoTitle}' not found.");
                }
            }

            // command "filter" by Title
            else if (command.StartsWith("filter"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: filter <todo-title>");
                    continue;
                }

                bool found = false; 

                Console.WriteLine("{0,-40} {1,-30} {2,-25} {3,0}", "Id", "Title", "Description", "DueDate");
                Console.WriteLine("{0,-40} {1,-30} {2,-20} {3,0}","-------------", "---------------------", "-------------------------", "-------------------------");
                foreach (var todo in todoList)
                {
                    if (todo.Title.ToLower().Contains(todoParams[1].ToLower()))
                    {
                        Console.WriteLine($"{todo.Id,-40} {todo.Title,-30} {todo.Description,-25} {todo.DueDate}");
                        found = true; 
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"No todo Title found with title '{todoParams[1]}'."); 
                }
            }

            Console.WriteLine("Your command: {0}", command);
            Console.WriteLine("\n");
        }
    }
}
