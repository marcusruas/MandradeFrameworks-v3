using MandradeFrameworks.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MandradeFrameworks.SharedKernel.Models.Paginacao
{
    /// <summary>
    /// Objeto para retornar uma lista paginada.
    /// </summary>
    public class ListaPaginada<T>
    {
        public ListaPaginada(IEnumerable<T> itens, int paginaAtual, int quantidadeRegistrosSolicitada, int quantidadeTotalRegistros)
        {
            Itens = itens;
            PaginaAtual = paginaAtual;
            QuantidadeTotalRegistros = quantidadeTotalRegistros;
            QuantidadeTotalPaginas = (int)Math.Ceiling(QuantidadeTotalRegistros / (double)quantidadeRegistrosSolicitada);
        }

        public IEnumerable<T> Itens { get; }
        /// <summary>
        /// Página anterior da pesquisa
        /// </summary>
        public int PaginaAtual { get; }
        /// <summary>
        /// Retorna a quantidade total de registros da pesquisa
        /// </summary>
        public int QuantidadeTotalRegistros { get; }
        /// <summary>
        /// Quantidade total de páginas da pesquisa
        /// </summary>
        public int QuantidadeTotalPaginas { get; }
    }
}