using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using TodoList_Manager.Database;
using TodoList_Manager.Models;
using System.Diagnostics;

public class CadastroController : Controller
{
   private readonly Config _context;

    public CadastroController(Config context)
    {
        _context = context;
    }
   

    public IActionResult Cadastro()
    {

        var viewModel = new CadastroViewModel
        {
            StatusList = ListarStatus(),
            PrioridadeList = ListarPrioridade()
        };

        Debug.WriteLine($"StatusList count: {viewModel.StatusList.Count()}");
        Debug.WriteLine($"PrioridadeList count: {viewModel.PrioridadeList.Count()}");

        return View(viewModel);
    }

    public List<SelectListItem> ListarStatus()
    {

        var statusList = _context.Status.ToList(); 
        return statusList.Select(s => new SelectListItem
        {
            Value = s.statusID.ToString(),
            Text = s.descricaoStatus
        }).ToList();

    }

    public List<SelectListItem> ListarPrioridade()
    {

        var prioridadeList = _context.Prioridade.ToList(); 
        return prioridadeList.Select(p => new SelectListItem
        {
            Value = p.GetPrioridadeID().ToString(),
            Text = p.GetDescricaoPrioridade()
        }).ToList();
    }


    [HttpPost]
    public IActionResult Cadastrar(CadastroViewModel viewModel)
    {
        Console.WriteLine($"Tarefa: {viewModel.Tarefa}");
        Console.WriteLine($"Prioridade: {viewModel.Prioridade}");
        Console.WriteLine($"Tempo: {viewModel.Tempo}");
        Console.WriteLine($"Agendamento: {viewModel.Agendamento}");
        Console.WriteLine($"Status: {viewModel.Status}");

        Console.WriteLine(ModelState);

        if (ModelState.IsValid)
        {
            try
            {   
                //Executando o Mapeamento da Model para o viewModel
                var tarefa = new Tarefa
                {
                    tarefa = viewModel.Tarefa, 
                    prioridade = viewModel.Prioridade,
                    tempo = TimeOnly.Parse(viewModel.Tempo),
                    agendamento = viewModel.Agendamento, 
                    status = viewModel.Status 

                };

                _context.Tarefa.Add(tarefa);
                _context.SaveChanges();

                TempData["AlertMessage"] = "Cadastrado com sucesso!";

                return RedirectToAction("Listagem", "Listar");
               
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Erro ao cadastrar: {ex.Message}";
                return RedirectToAction("Cadastro");
            }
        }
        else
        {
            TempData["AlertMessage"] = "Erro ao cadastrar: Verifique os erros e tente novamente.";
            return RedirectToAction("Cadastro"); 
        }
    }
    public IActionResult Editar()
    {
        return RedirectToAction("Editar", "Editar");
    }
}