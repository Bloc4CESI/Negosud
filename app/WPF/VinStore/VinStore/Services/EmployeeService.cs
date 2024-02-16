using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using ApiNegosud.Models;
using Newtonsoft.Json;

namespace VinStore.Services
{
    public static class EmployeeService
    {
        public static async Task<Employee> GetEmployeeByMail(string mail)
        {
            try
            {
                var response = await ApiConnexion.ApiClient.GetStringAsync($"https://localhost:7281/api/Employee/GetByEmail/{mail}");
                var employee = JsonConvert.DeserializeObject<Employee>(response);
                return employee;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nom d'utilisateur ou mot de passe incorrect.");
                return null;
            }
        }
    }
}
