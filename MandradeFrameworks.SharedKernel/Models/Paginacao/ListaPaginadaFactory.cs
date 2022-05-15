using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandradeFrameworks.SharedKernel.Models.Paginacao
{
    public static class ListaPaginadaFactory
    {
        public static async Task<ListaPaginada<T>> CreateAsync<T>(this IQueryable<T> query, int pagina, int quantidadeRegistros)
        {
            var itens = await query.Skip((pagina - 1) * quantidadeRegistros).Take(quantidadeRegistros).ToListAsync();
            var quantidadeTotalRegistros = await query.CountAsync();

            return new ListaPaginada<T>(itens, pagina, quantidadeRegistros, quantidadeTotalRegistros);
        }

        public static ListaPaginada<T> Create<T>(IEnumerable<T> itens, int paginaAtual, int quantidadeRegistrosSolicitada, int quantidadeTotalRegistros)
            => new(itens, paginaAtual, quantidadeRegistrosSolicitada, quantidadeTotalRegistros);
    }
}
