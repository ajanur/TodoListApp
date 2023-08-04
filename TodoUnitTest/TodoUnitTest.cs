
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoListApp.Models;
using TodoListApp.Controllers;
using Moq;
using System.Collections.Generic;
using TodoListApp.ServiceExtension;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.IO;

namespace TodoUnitTest
{
    [TestClass]
    public class TodoUnitTest
    {
        List<Todo> testTodos;
        TodoController controller;

        [TestInitialize]
        public void setup()
        {
            // Arrange
            //There is no Repository object to mocked up
            testTodos = GetTestTodos();

            //Created Fake Logger Object
            var loggerObj = new Mock<ILoggerManager>();
            //ILoggerManager logger = new LoggerManager();
            //LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "\\nlog.config"));

            controller = new TodoController(testTodos,loggerObj.Object);
        }
        [TestMethod]
        public void GetAllTodo()
        {
            // Arrange

            // Act
            var actionResult = controller.Get() as OkObjectResult;
            var result = actionResult.Value as List<Todo>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(testTodos.Count, result.Count);
        }

        [TestMethod]
        public void GeTodoById()
        {
            // Arrange
            var testTodo = testTodos[0];

            // Act
            var actionResult = controller.Get(testTodo.Id) as OkObjectResult;
            var result = actionResult.Value as Todo;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(testTodo.Name, result.Name);
        }

        [TestMethod]
        [DataRow("Demo1")]
        [DataRow("Demo2")]
        public void GeTodoByName(string name)
        {
            // Arrange
            var testTodo = testTodos[0];

            // Act
            var actionResult = controller.GetByName(name) as OkObjectResult;
            var result = actionResult.Value as Todo;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void GetTodoByPriority(int priority)
        {
            // Arrange
            var testTodo = testTodos[0];

            // Act
            var actionResult = controller.GetTodoByPriority(priority) as OkObjectResult;
            var result = actionResult.Value as List<Todo>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(priority, result[0].Priority);
        }

        [TestMethod]
        [DataRow("Created")]
        [DataRow("Completed")]
        public void GetTodoByStatus(string status)
        {
            // Arrange
            var testTodo = testTodos[0];

            // Act
            var actionResult = controller.GetTodoByStatus(status) as OkObjectResult;
            var result = actionResult.Value as List<Todo>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result[0].Status);
        }

        [TestMethod]
        [DataRow(1,"Created")]
        [DataRow(4,"Completed")]
        public void GetTodoByPriorityAndStatus(int priority, string status)
        {
            // Arrange
            var testTodo = testTodos[0];

            // Act
            var actionResult = controller.GetTodoByPriorityAndStatus(priority,status) as OkObjectResult;
            var result = actionResult.Value as List<Todo>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result[0].Status);
        }

        [TestMethod]
        [DataRow("Demo5", 3, "Created")]
        [DataRow("Demo6", 2, "Created")]
        public void AddTodo(string name, int priorty, string status)
        {
            // Arrange

            Todo todo = new Todo() { Id= System.Guid.NewGuid(), Name=name, Priority=priorty, Status= status };

            // Act
            var actionResult = controller.Post(todo);
            var contentResult = actionResult as OkResult;
            var statusCode = contentResult.StatusCode;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [DataRow(3, "Completed")]
        [DataRow(2, "InProgress")]
        public void UpdateTodo(int priorty, string status)
        {
            // Arrange
            var testTodo = testTodos[0];
            testTodo.Priority = priorty;
            testTodo.Status = status;

            // Act
            var actionResult = controller.Put(testTodo);
            var contentResult = actionResult as OkResult;
            var statusCode = contentResult.StatusCode;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void DeleteTodo()
        {
            // Arrange
            var testTodo = testTodos[0];
            var id = testTodo.Id;

            // Act
            var actionResult = controller.Delete(id);
            var contentResult = actionResult as OkResult;
            var statusCode = contentResult.StatusCode;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, statusCode);
        }


        private List<Todo> GetTestTodos()
        {
            var testTodos = new List<Todo>();
            testTodos.Add(new Todo { Id = System.Guid.NewGuid(), Name = "Demo1", Priority = 1,Status="Created" });
            testTodos.Add(new Todo { Id = System.Guid.NewGuid(), Name = "Demo2", Priority = 2, Status = "Created" });
            testTodos.Add(new Todo { Id = System.Guid.NewGuid(), Name = "Demo3", Priority = 3, Status = "Created" });
            testTodos.Add(new Todo { Id = System.Guid.NewGuid(), Name = "Demo4", Priority = 4, Status = "Completed" });
            return testTodos;
        }
    }
}
