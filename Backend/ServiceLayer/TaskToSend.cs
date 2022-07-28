using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskToSend
    {
        [JsonInclude]
        public int Id { get; set; }
        [JsonInclude]
        public DateTime CreationTime { get; set; }
        [JsonInclude]
        public string Title { get; set; }
        [JsonInclude]
        public string Description { get; set; }
        [JsonInclude]
        public DateTime DueDate { get; set; }

        public TaskToSend()
        {
            Id = 0;
            CreationTime = DateTime.Now;
            Description = "";
            DueDate = DateTime.Now;
            Title = "";
        }

        public TaskToSend(int Id,DateTime CreationTime,string Title,string Description,DateTime DueDate)
        {
            this.Id = Id;
            this.CreationTime = CreationTime;
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;

        }
    }
}
