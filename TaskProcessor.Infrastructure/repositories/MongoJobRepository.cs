
using TaskProcessor.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Models;

public class MongoJobRepository : IMongoJobRepository
{
    private readonly IMongoCollection<Job> _jobs;

    public MongoJobRepository()
    {
        var client = new MongoClient("mongodb://mongodb:27017");
        var database = client.GetDatabase("TaskProcessorDb");
        _jobs = database.GetCollection<Job>("Jobs");
    }

    public async Task Add(Job job)
    {
       await _jobs.InsertOneAsync(job);
    } 

    public async Task UpdateStatus(string jobId, JobStatus status, int? tentativas = null)
    {
        var update = Builders<Job>.Update
            .Set(j => j.Status, status);

        if (tentativas.HasValue)
            update = update.Set(j => j.Tentativas, tentativas.Value);

        await _jobs.UpdateOneAsync(j => j.Id == jobId.ToString(), update);
    }

    public async Task<Job> GetById(string id) => await _jobs.Find(j => j.Id == id.ToString()).FirstOrDefaultAsync();
}
