using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoList_Manager.Database;
using TodoList_Manager.Models;

namespace TodoList_Manager.Controllers
{

    public class EditarController : Controller
    {
        private readonly Config _context;


        public EditarController(Config context)
        {
            _context = context;
        }

        public IActionResult Editar(int IdTarefa)
        {
            if (IdTarefa == 0)
            {
                return BadRequest("ID inválido.");
            }

            var tarefa = _context.Tarefa.Find(IdTarefa);
            if (tarefa == null)
            {
                return NotFound();
            }
           
            var model = new CadastroViewModel
            {
                Id = tarefa.IdTarefa,
                Tarefa = tarefa.tarefa,
                Prioridade = tarefa.prioridade,
                Tempo = tarefa.tempo.ToString("HH:mm") ?? "",
                Agendamento = tarefa.agendamento,
                Status = tarefa.status,
                StatusList = GetStatusList(),
                PrioridadeList = GetPrioridadeList()

            };
           
            return View(model);
        }

        [HttpPost]
        public IActionResult Salvar(CadastroViewModel model)
        {
            Console.WriteLine(model.Id + model.Tarefa);
            if (ModelState.IsValid)
            {
                var tarefa = _context.Tarefa.Find(model.Id);
                if (tarefa == null)
                {
                    return NotFound();
                }

                tarefa.tarefa = model.Tarefa;
                tarefa.prioridade = model.Prioridade;
                tarefa.tempo = TimeOnly.Parse(model.Tempo);
                tarefa.agendamento = model.Agendamento;
                tarefa.status = model.Status;

                _context.SaveChanges();

                return RedirectToAction("Listagem", "Listar");
            }

            model.StatusList = GetStatusList();
            model.PrioridadeList = GetPrioridadeList();
            return View(model);
        }

        private List<SelectListItem> GetStatusList()
        {
            // Retorna a lista de status
            var statusList = _context.Status.ToList();
            return statusList.Select(s => new SelectListItem
            {
                Value = s.statusID.ToString(),
                Text = s.descricaoStatus
            }).ToList();
        }

        private List<SelectListItem> GetPrioridadeList()
        {
            // Retorna a lista de prioridades
            var prioridadeList = _context.Prioridade.ToList();
            return prioridadeList.Select(p => new SelectListItem
            {
                Value = p.GetPrioridadeID().ToString(),
                Text = p.GetDescricaoPrioridade()
            }).ToList();

        }
    }
}