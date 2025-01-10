using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TodoList_Manager.Models
{
    public class Status
    {
        [Key]
        public int statusID { get; set; }
        public string? descricaoStatus { get; set; }
    }
}
