using ApiContracts.Account;
using ApiContracts.References.Agreement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ConsoleUI
{
    internal class Program
    {
        private const string APP_PATH = "http://localhost:60999/";
        private static string token;

        private static void Main(string[] args)
        {
            var userName = "pavel@karasov.net";
            var password = "test_Mega2";

            var tokenDictionary = GetTokenDictionary(userName, password);
            token = tokenDictionary["access_token"];

            var items = GetAllAgreement(token).ToList();
            Console.WriteLine("List all Agreement:");

            foreach (var item in items)
            {
                Console.WriteLine($"id - {item.Id} name - {item.Name} - number {item.Number}");
            }

            if (items.Any())
            {
                var agreement = items.First();
                Console.WriteLine("Begin update First Agreement ");

                var update = new AgreementCreateBindingModel()
                {
                    Id = agreement.Id,
                    Name = $"agreement {DateTime.Now}",
                    Number = agreement.Number,
                    BegDate = agreement.BegDate,
                    EndDate = agreement.EndDate
                };

                var result = UpdateAgreement(token, update);

                Console.WriteLine("End update First Agreement " + (result ? "ok" : "bad"));

                Console.WriteLine("Get new Version");

                var newAgreement = GetAgreement(token, update.Id);

                Console.WriteLine($"id - {newAgreement.Id} name - {newAgreement.Name} - number {newAgreement.Number}");

                Console.WriteLine("Calculation Agreement");

                var calculation = GetCalculationAgreement(token, update.Id, new DateTime(2018, 4, 1), new DateTime(2018, 3, 1), new DateTime(2018, 4, 1));

                Console.WriteLine($"Name - '{newAgreement.Name}' 04.2018 calculation {calculation.Consumption}");
            }

            Console.WriteLine($"Finish");
        }

        private static string Register(string email, string password)
        {
            var registerModel = new RegisterBindingModel()
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            };
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync($"{APP_PATH}/api/Agreement/Register", registerModel).Result;
                return response.StatusCode.ToString();
            }
        }

        private static Dictionary<string, string> GetTokenDictionary(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ),
                    new KeyValuePair<string, string>( "username", userName ),
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync($"{APP_PATH}/Token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }

        private static HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        private static AgreementView GetAgreement(string token, long id)
        {
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync($"{APP_PATH}/api/Agreement/{id}").Result;
                if (response.StatusCode > System.Net.HttpStatusCode.Ambiguous)
                    Console.WriteLine(response.StatusCode);
                var json = response.Content.ReadAsStringAsync().Result;
                var item = JsonConvert.DeserializeObject<AgreementView>(json);
                return item;
            }
        }

        private static IEnumerable<AgreementView> GetAllAgreement(string token)
        {
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync($"{APP_PATH}/api/Agreement").Result;
                if (response.StatusCode > System.Net.HttpStatusCode.Ambiguous)
                    Console.WriteLine(response.StatusCode);
                var json = response.Content.ReadAsStringAsync().Result;
                var items = JsonConvert.DeserializeObject<List<AgreementView>>(json);
                return items;
            }
        }

        private static bool UpdateAgreement(string token, AgreementCreateBindingModel agreement)
        {
            using (var client = CreateClient(token))
            {
                var result = client.PutAsJsonAsync($"{APP_PATH}/api/Agreement/{agreement.Id}", agreement).Result;
                if (result.StatusCode > System.Net.HttpStatusCode.Ambiguous)
                    Console.WriteLine(result.StatusCode);
                return result.StatusCode < System.Net.HttpStatusCode.Ambiguous;
            }
        }

        private static AgreementCalculationPeriodView GetCalculationAgreement(string token, long id, DateTime period, DateTime start, DateTime end)
        {
            using (var client = CreateClient(token))
            {
                var parameters = new AgreementCalculationPeriodBindingModel()
                {
                    Id = id,
                    Period = period,
                    BeginDate = start,
                    EndDate = end
                };

                var response = client.PostAsJsonAsync($"{APP_PATH}/api/Agreement/CalculationPeriod", parameters).Result;
                if (response.StatusCode > System.Net.HttpStatusCode.Ambiguous)
                    Console.WriteLine(response.StatusCode);
                var json = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<AgreementCalculationPeriodView>(json);
                return result;
            }
        }
    }
}