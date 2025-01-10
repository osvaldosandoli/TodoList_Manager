using System.ComponentModel.DataAnnotations;

namespace TodoList_Manager.Models
{
    public class Prioridade
    {
        [Key]
        private int prioridadeID { get; set; }
        private string? descricaoPrioridade { get; set; }

        public Prioridade() { }

        public Prioridade(int id, string descricao)
        {
            prioridadeID = id;
            descricaoPrioridade = descricao;
        }

        public int GetPrioridadeID()
        {
            return prioridadeID;
        }

        public string? GetDescricaoPrioridade()
        {
            return descricaoPrioridade;
        }


    }
}
