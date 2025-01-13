using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Listagem()
        {
            var listagem = _context.ListagemTarefas.OrderBy(L => L.Agendamento).ToList();
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

        [HttpPost]
        public async Task<IActionResult> Play(int idTarefa)
        {
            var DataHoraAtual = DateTime.Now;
            var timeIni = TimeOnly.FromDateTime(DataHoraAtual);

            try
            {
                // Verifica se já existe um timer em andamento para a tarefa (opcional, mas recomendado)
                var existingTimer = await _context.TimerTarefa
                                              .FirstOrDefaultAsync(t => t.idTarefa == idTarefa && t.endTime == null);

                if (existingTimer != null)
                {
                    return BadRequest(new { message = "Já existe um timer em andamento para essa tarefa." });
                    //Console.WriteLine("Erro ja existe outra");
                }
            }
            catch (Exception ex)
            {
                // Logando a exceção para depuração
                Console.Error.WriteLine($"Erro ao buscar o timer: {ex.Message}");

                // Retorne uma resposta de erro adequada
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
            _context.SaveChanges();

            return Ok(new { message = "Tarefa iniciada com sucesso!", startTime = timeIni });
           
        }
        [HttpPost]
        public async Task<IActionResult> Pause(int idTarefa)
        {
            var DataHoraAtual = DateTime.Now;
            var timeFim = TimeOnly.FromDateTime(DataHoraAtual);

            var timerTarefa = _context.TimerTarefa
                              .FirstOrDefault(t => t.idTarefa == idTarefa && t.endTime == null);  // Pega a tarefa onde o endTime ainda não foi preenchido

            if (timerTarefa != null)
            {
                    timerTarefa.endTime = timeFim;
                
                _context.SaveChanges();

                await AtualizarTempo(idTarefa);
               // return Ok(new { message = "Tarefa pausada com sucesso!", timeFim }); 
                return RedirectToAction("Listagem");
            }
           
            return NotFound(new { message = "Tarefa não encontrada ou já pausada." });
        }


        /*  [HttpPost]
         public async Task<IActionResult> AtualizarTempo(int idTarefa)
         {
             TimerTarefa timer = new TimerTarefa();

             var tarefa = await _context.Tarefa.FindAsync(idTarefa);
             var horaAntiga = tarefa.tempoGasto;
             TimeSpan horaUtilizada = (TimeSpan)(timer.endTime - timer.startTime);

            if(horaAntiga != null)
             {
                 horaAntiga = tarefa.tempoGasto;
             }
             else
             {
                 horaAntiga = null;
             }

        // Chamar o método que executa a procedure
        await _context.AtualizarHoraAsync(horaUtilizada, horaAntiga, idTarefa);

            return RedirectToAction("Listagem");
        }*/
        [HttpPost]
        public async Task<IActionResult> AtualizarTempo(int idTarefa)
        {
            // Busca os dados da tarefa
            var tarefa = await _context.Tarefa.FindAsync(idTarefa);
            if (tarefa == null)
            {
                return NotFound(new { message = "Tarefa não encontrada." });
            }

            // Busca o timer relacionado à tarefa
            var timer = await _context.TimerTarefa
                                      .Where(t => t.idTarefa == idTarefa)
                                      .OrderByDescending(t => t.idTimer)
                                      .FirstOrDefaultAsync();

            if (timer == null || timer.endTime == null)
            {
                return NotFound(new { message = "Nenhum timer válido encontrado para a tarefa." });
            }

            // Calcula o tempo utilizado
            var horaAntiga = tarefa.tempoGasto; // Se null, considera zero
            var horaUtilizada = timer.endTime.Value.ToTimeSpan() - timer.startTime.ToTimeSpan();

            // Atualiza o tempo no banco usando sua procedure ou lógica
            await _context.AtualizarHoraAsync(horaUtilizada, horaAntiga, idTarefa);

            return RedirectToAction("Listagem");
        }
    }

}
