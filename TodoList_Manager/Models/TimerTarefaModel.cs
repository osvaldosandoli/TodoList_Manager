using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList_Manager.Models
{
    [Table ("TimerTarefas")]
    public class TimerTarefa
    {
        [Key]
        public int idTimer { get; set; }     // Chave primária
        public int idTarefa { get; set; }    // Chave estrangeira para Tarefa
        public TimeOnly startTime { get; set; }  // Horário de início
        public TimeOnly? endTime { get; set; }  // Horário de término (pode ser nulo, pois ainda não terminou)
        public DateTime dtHrResgis { get; set; }
    }
}
