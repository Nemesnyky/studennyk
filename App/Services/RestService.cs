using System.Text;
using System.Text.Json;

using ModelTask = App.Models.Task;

namespace App.Services;

public class RestService
{
    HttpClient _client;
    JsonSerializerOptions _serializerOptions;


    public RestService()
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5004/api/v1/"),
        };
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<List<ModelTask>> RefreshDataAsync()
    {
        var tasks = new List<ModelTask>();

        try
        {
            HttpResponseMessage response = await _client.GetAsync("tasks");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                tasks = JsonSerializer.Deserialize<List<ModelTask>>(content, _serializerOptions);
            }
        }
        catch (Exception _)
        {

        }

        return tasks;
    }

    public async Task<ModelTask> GetTask(long id)
    {
        ModelTask task = null;
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                task = JsonSerializer.Deserialize<ModelTask>(content, _serializerOptions);
            }
        }
        catch (Exception _)
        {
        }

        return task;
    }

    public async Task<long> AddTask(ModelTask task)
    {

        try
        {
            string json = JsonSerializer.Serialize<ModelTask>(task, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PostAsync("tasks", content);
            string response_content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<long>(response_content, _serializerOptions);
        }
        catch (Exception _)
        {
            return -1;
        }
    }

    public async Task UpdateTaskTitle(long id, string title)
    {
        try
        {
            string json = JsonSerializer.Serialize(new { title }, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync($"tasks/{id}", content);
        }
        catch (Exception _)
        {
        }
    }

    public async Task UpdateTaskDescription(long id, string description)
    {
        try
        {
            string json = JsonSerializer.Serialize(new { description }, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync($"tasks/{id}", content);
        }
        catch (Exception _)
        {
        }
    }

    public async Task UpdateTaskStatus(long id, bool isDone)
    {
        try
        {
            string json = JsonSerializer.Serialize(new { isDone }, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync($"tasks/{id}", content);
        }
        catch (Exception _)
        {
        }
    }

    public async Task UpdateTaskDue(long id, DateTimeOffset due)
    {
        try
        {
            string json = JsonSerializer.Serialize(new { due }, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync($"tasks/{id}", content);
        }
        catch (Exception _)
        {
        }
    }

    public async Task DeleteTask(long id)
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"tasks/{id}");
        }
        catch (Exception ex)
        {
        }
    }
}
