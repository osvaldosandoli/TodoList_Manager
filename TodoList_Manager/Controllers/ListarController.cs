using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public async Task<IActionResult> Listagem()
        {
            var listagem = _context.ListagemTarefas.OrderBy(L => L.Agendamento).ToList();

            foreach (var tarefa in listagem)
            {
                await AtualizarStatus(tarefa.IdTarefa);
            }

            return View(listagem);

        }

        public ActionResult Excluir(int id)
        {

            string deleteTimersQuerry = "DELETE FROM timerTarefas WHERE idTarefa =" + id;
            _context.Database.ExecuteSqlRaw(deleteTimersQuerry);

            string deleteTarefasQuerry = "DELETE FROM Tarefas WHERE idTarefa =" + id;
            _context.Database.ExecuteSqlRaw(deleteTarefasQuerry);

            return RedirectToAction("Listagem");
        }

        public async Task <ActionResult> AtualizarStatus(int idTarefa)
        {
            var tarefa = await _context.Tarefa.FindAsync(idTarefa);
            var DataHoraAtual = DateTime.Now;

            if(tarefa.agendamento < DataHoraAtual && tarefa.status == 1)
            {
                tarefa.status = 3;
                _context.SaveChanges();
            }
            return RedirectToAction("Listagem");
        }

        [HttpPost]
        public async Task<IActionResult> Play(int idTarefa)
        {
            var DataHoraAtual = DateTime.Now;
            var timeIni = TimeOnly.FromDateTime(DataHoraAtual);

            try
            {
                var existingTimer = await _context.TimerTarefa
                                              .FirstOrDefaultAsync(t => t.idTarefa == idTarefa && t.endTime == null);

                if (existingTimer != null)
                {
                    return BadRequest(new { message = "Já existe um timer em andamento para essa tarefa." });
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao buscar o timer: {ex.Message}");

                return StatusCode(500, "Ocorreu um erro ao acessar o banco de dados.");
            }

            var timePlay = new TimerTarefa
            {
                idTarefa = idTarefa,
                startTime = timeIni,
                endTime = null,
                dtHrResgis = DataHoraAtual,
            };

            _context.TimerTarefa.Add(timePlay);

            if(timePlay != null)
            {
                var tarefa = await _context.Tarefa.FindAsync(idTarefa);
                tarefa.status = 2;
                _context.SaveChanges();
            }
            
            return Ok(new { message = "Tarefa iniciada com sucesso!", startTime = timeIni });

        }
        [HttpPost]
        public async Task<IActionResult> Pause(int idTarefa)
        {
            var DataHoraAtual = DateTime.Now;
            var timeFim = TimeOnly.FromDateTime(DataHoraAtual);

            var timerTarefa = _context.TimerTarefa
                              .FirstOrDefault(t => t.idTarefa == idTarefa && t.endTime == null); 

            if (timerTarefa != null)
            {
                timerTarefa.endTime = timeFim;

                _context.SaveChanges();

                await AtualizarTempo(idTarefa);
                return RedirectToAction("Listagem");
            }

            return NotFound(new { message = "Tarefa não encontrada ou já pausada." });
        }


        [HttpPost]
        public async Task<IActionResult> AtualizarTempo(int idTarefa)
        {
            var tarefa = await _context.Tarefa.FindAsync(idTarefa);
            if (tarefa == null)
            {
                return NotFound(new { message = "Tarefa não encontrada." });
            }

            
            var timer = await _context.TimerTarefa
                                      .Where(t => t.idTarefa == idTarefa)
                                      .OrderByDescending(t => t.idTimer)
                                      .FirstOrDefaultAsync();

            if (timer == null || timer.endTime == null)
            {
                return NotFound(new { message = "Nenhum timer válido encontrado para a tarefa." });
            }

            var horaAntiga = tarefa.tempoGasto;
            var horaUtilizada = timer.endTime.Value.ToTimeSpan() - timer.startTime.ToTimeSpan();

            // Atualiza o tempo no banco usando a procedure
            await _context.AtualizarHoraAsync(horaUtilizada, horaAntiga, idTarefa);

            return RedirectToAction("Listagem");
        }


        [HttpGet]
        public async Task<IActionResult> Concluir(int idTarefa)
        {
            var tarefa = await _context.Tarefa
                .FirstOrDefaultAsync(t => t.IdTarefa == idTarefa);

            if (tarefa == null)
            {
                return NotFound(new { message = "Tarefa não encontrada." });
            }

            tarefa.status = 4;
            _context.Tarefa.Update(tarefa);
            await _context.SaveChangesAsync(); 

            return RedirectToAction("Listagem");
        }

    }

}
