using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList_Manager.Models
{
   
    public class ListagemTarefas
    {
        public int IdTarefa { get; set; }
        public string Tarefa { get; set; }
        public TimeOnly Tempo { get; set; }
        public DateTime Agendamento { get; set; }
        public string Prioridade { get; set; }
        public string Status { get; set; }

    }
}
