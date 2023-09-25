
using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();
            Assert.IsNotNull(newEmployee.EmployeeId);
            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
            Assert.AreEqual(employee.Department, newEmployee.Department);
            Assert.AreEqual(employee.Position, newEmployee.Position);
        }

        [TestMethod]
        public void GetEmployeeById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var employee = response.DeserializeContent<Employee>();
            Assert.AreEqual(expectedFirstName, employee.FirstName);
            Assert.AreEqual(expectedLastName, employee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Department = "Engineering",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer VI",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newEmployee = putResponse.DeserializeContent<Employee>();

            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sunny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [TestMethod]
        public void GetReportingStructureById_Returns_NotFound()
        {
            //Arrange
            var employeeId = "Invalid_Id";

            //Exec
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [TestMethod]
        public void GetReportingStructureById_Returns_Ok()
        {

            //Arange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedReports = 4;

            //Exec
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, reportingStructure.employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.employee.LastName);
            Assert.AreEqual(expectedReports, reportingStructure.numberOfReports);
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            //Set up vars
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedSalary = 150000;
            var expectedEffectiveDate = DateTime.Today;

            //We need to add a record before we can return a record since we did not seed this
            var compensation = new Compensation()
            {
                employee = new Employee()
                {
                    EmployeeId = employeeId
                },
                salary = expectedSalary,
                effectiveDate = expectedEffectiveDate
            };
            var request = new JsonSerialization().ToJson(compensation);

            //Exec the add
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
                new StringContent(request, Encoding.UTF8, "application/json"));
            var postResponse = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            var newCompensation = postResponse.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.employee.EmployeeId);
            Assert.AreEqual(compensation.salary, newCompensation.salary);
            Assert.AreEqual(compensation.effectiveDate, newCompensation.effectiveDate);
        }
        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            //Set up vars
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedSalary = 150000;
            var expectedEffectiveDate = DateTime.Today;

            //We need to add a record before we can return a record since we did not seed this
            var newCompensation = new Compensation()
            {
                employee = new Employee()
                {
                    EmployeeId = employeeId
                },
                salary = expectedSalary,
                effectiveDate = expectedEffectiveDate
            };
            var request = new JsonSerialization().ToJson(newCompensation);

            //Exec the add
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
                new StringContent(request, Encoding.UTF8, "application/json"));
            var postResponse = postRequestTask.Result;

            //First make sure it worked
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);
            //End create new compensation

            //Now do a get
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var getResponse = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
            var compensation = getResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedSalary, compensation.salary);
            Assert.AreEqual(expectedEffectiveDate, compensation.effectiveDate);
        }
        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            //Setup
            var employeeId = "InvalidId";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [TestMethod]
        public void CreateCompensation_Returns_BadRequest()
        {
            //Set up vars
            var employeeId = "InvalidId";
            var expectedSalary = 10;
            var expectedEffectiveDate = DateTime.Today;

            //We need to add a record before we can return a record since we did not seed this
            var compensation = new Compensation()
            {
                employee = new Employee()
                {
                    EmployeeId = employeeId
                },
                salary = expectedSalary,
                effectiveDate = expectedEffectiveDate
            };
            var request = new JsonSerialization().ToJson(compensation);

            //Exec the add
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
                new StringContent(request, Encoding.UTF8, "application/json"));
            var postResponse = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }
    }
}
