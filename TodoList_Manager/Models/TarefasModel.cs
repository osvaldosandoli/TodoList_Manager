using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoList_Manager.Models
{
    [Table("Tarefas")]
    public class Tarefa
    {
        [Key]
        public int IdTarefa { get; set; }
        public string tarefa { get; set; }
        public int prioridade { get; set; }
        public TimeOnly tempo { get; set; }
        public DateTime agendamento { get; set; }
        public int status { get; set; }

    }
}
