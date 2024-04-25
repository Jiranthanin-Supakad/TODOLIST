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
                Console.WriteLine("Title\t\tDescription\t\tDueDate");
                Console.WriteLine("-----------------------------------------------------");
                todoList.ForEach(i => Console.WriteLine($"{i.Title,-15} {i.Description,-20} {i.DueDate}"));

            }

            
            // command "remove" by Guid
            else if (command.StartsWith("remove"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: remove <todo-Title>");
                    Console.WriteLine("\n");
                    continue;
                }

                Guid todoId;
                if (!Guid.TryParse(todoParams[1], out todoId))
                {
                    Console.WriteLine("Invalid todolist Id format.");
                    continue;
                }


                var removeEntry = todoList.Find(i => i.Id.Equals(todoId));
                if (removeEntry != null)
                {
                    todoList.Remove(removeEntry);
                    Console.WriteLine($"Removed todo : '{removeEntry.Id}' '{removeEntry.Title}'");
                }
                else
                {
                    Console.WriteLine($"Todo Id '{todoParams[1]}' not found.");
                }
            }

            
            // command "filter" by Title
            else if (command.StartsWith("filter"))
            {

            }

            Console.WriteLine("Your command: {0}", command);
            Console.WriteLine("\n");
        }
    }
}
