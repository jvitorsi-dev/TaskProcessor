using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskProcessor.Domain.Models;

namespace TaskProcessor.Models
{
    public class Job
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Tipo { get; set; } = string.Empty;
        public string Dados { get; set; } = string.Empty;
        public JobStatus Status { get; set; } = JobStatus.Pendente;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int Tentativas { get; set; } = 0;
    }
}