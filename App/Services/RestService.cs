using System.Text;
using System.Text.Json;

using ModelTask = App.Models.Task;

namespace App.Services;

public class RestService
{
    HttpClient _client;
    JsonSerializerOptions _serializerOptions;

    const string Url = "http://localhost:5004/api/v1/tasks/";

    public RestService()
    {
        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<List<ModelTask>> RefreshDataAsync()
    {
        var tasks = new List<ModelTask>();

        Uri uri = new Uri(string.Format(Url, string.Empty));
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
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
        Uri uri = new Uri(string.Format(Url, id));

        ModelTask task = null;
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
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
        Uri uri = new Uri(string.Format(Url, string.Empty));

        try
        {
            string json = JsonSerializer.Serialize<ModelTask>(task, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PostAsync(uri, content);
            string response_content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<long>(response_content, _serializerOptions);
        }
        catch (Exception _)
        {
            return -1;
        }
    }

    public async Task UpdateTask<T>(long id, T data)
    {
        Uri uri = new Uri(string.Format(Url, id));

        try
        {
            string json = JsonSerializer.Serialize(data, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync(uri, content);
        }
        catch (Exception _)
        {
        }
    }

    public async Task DeleteTask(long id)
    {
        Uri uri = new Uri(string.Format(Url, id));

        try
        {
            HttpResponseMessage response = await _client.DeleteAsync(uri);
        }
        catch (Exception ex)
        {
        }
    }
}
