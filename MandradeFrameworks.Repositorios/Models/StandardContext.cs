using MandradeFrameworks.SharedKernel.Models;
using MandradeFrameworks.SharedKernel.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MandradeFrameworks.Repositorios.Models
{
    public abstract class StandardContext<TContext> : DbContext
    where TContext : DbContext
    {
        public StandardContext(DbContextOptions<TContext> options, IUsuarioAutenticado usuario) : base(options)
        {
            _usuario = usuario;
        }

        private readonly IUsuarioAutenticado _usuario;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<Entidade>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DefinirUsuarioCriador(_usuario.NomeCompleto);
                        break;

                    case EntityState.Modified:
                        entry.Entity.DefinirUsuarioAtualizador(_usuario.NomeCompleto);
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
