using Microsoft.EntityFrameworkCore;
using System;
using TodoList_Manager.Models;

namespace TodoList_Manager.Database
{
    public class Config : DbContext
    {
        public Config(DbContextOptions<Config> options) : base(options) { }

        // Adicione os DbSet para suas tabelas
        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Prioridade> Prioridade { get; set; }
        public DbSet<ListagemTarefas> ListagemTarefas { get; set; }

        //Mapeamento para o Entity Framework
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>().ToTable("Tarefas");

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(s => s.statusID); // Usando a propriedade StatusID diretamente
                entity.Property(s => s.descricaoStatus)
                      .HasColumnName("descricaoStatus")
                      .HasMaxLength(100); // Mapear a propriedade DescricaoStatus
            });

            modelBuilder.Entity<Prioridade>(entity =>
            {
                entity.HasKey("prioridadeID");
                entity.Property<string>("descricaoPrioridade")
                      .HasColumnName("descricaoPrioridade")
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<ListagemTarefas>().ToView("tarefas_view").HasNoKey();
        }

    }
}
