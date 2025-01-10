using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TodoList_Manager.Database;
using TodoList_Manager.Models;

namespace TodoList_Manager.Controllers
{
    public class ListarController : Controller
    {

        private readonly Config _context;
        

        public ListarController(Config context)
        {
            _context = context;
        }
        public IActionResult Listagem()
        {
            var listagem = _context.ListagemTarefas.OrderBy(L => L.Agendamento).ToList();
            return View(listagem);
           
        }

        public ActionResult Excluir(int id)
        {
            var tarefa = _context.Tarefa.Find(id);
            if (tarefa != null)
            {
                _context.Tarefa.Remove(tarefa);
                _context.SaveChanges();
            }

            return RedirectToAction("Listagem");
        }
    }
     
}
