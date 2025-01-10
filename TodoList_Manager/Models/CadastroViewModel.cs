using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TodoList_Manager.Models
{
    public class CadastroViewModel
    {
        // Correspondendo os nomes da Tarefa diretamente
        [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
        public string Tarefa { get; set; } // 'tarefa' da model

        [Required(ErrorMessage = "A prioridade é obrigatória.")]
        public int Prioridade { get; set; } // 'prioridade' da model


        [Required(ErrorMessage = "O tempo estimado é obrigatório.")]
        [RegularExpression(@"^\d{2}:\d{2}$", ErrorMessage = "O tempo deve estar no formato hh:mm.")]
        public string Tempo { get; set; } // Recebe a entrada no formato de texto

        [Required(ErrorMessage = "O agendamento é obrigatório.")]
        public DateTime Agendamento { get; set; } // 'agendamento' da model

        [Required(ErrorMessage = "O status é obrigatório.")]
        public int Status { get; set; } // 'status' da model



        //  [Required(ErrorMessage = "A prioridade é obrigatória.")]
       // public int StatusID { get; set; } // ID do Status selecionado
        //public int PrioridadeID { get; set; } // ID da Prioridade selecionada
        public IEnumerable<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PrioridadeList { get; set; } = new List<SelectListItem>();

       

    }
}
