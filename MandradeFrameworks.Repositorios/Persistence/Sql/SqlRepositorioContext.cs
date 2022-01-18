﻿using MandradeFrameworks.Mensagens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MandradeFrameworks.Mensagens.Models;
using MandradeFrameworks.Repositorios.Models;
using MandradeFrameworks.Retornos.Models;
using System.Linq;

namespace MandradeFrameworks.Repositorios.Persistence.Sql
{
    public abstract class SqlRepositorio<TContext> : SqlRepositorio where TContext : StandardContext
    {
        public SqlRepositorio(IServiceProvider provider, TContext context) : base(provider)
        {
            _context = context;
        }

        protected readonly TContext _context;

        /// <summary>
        /// Adiciona uma entidade ao banco de dados. Caso o método <see cref="DbContext.SaveChangesAsync"/> retorne -1 ou ocorra alguma exception no processo,
        /// será adicionado uma mensagem do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado -1 no retorno desta operação
        /// </summary>
        /// <returns>A quantidade de registros afetados por essa operação</returns>
        protected async Task<int> AdicionarEntidade<T>(T entidade)
        {
            try
            {
                await _context.AddAsync(entidade);
                int linhasAfetadas = await _context.SaveChangesAsync();

                if (linhasAfetadas < 0)
                    _mensageria.AdicionarMensagemErro("Não foi possível realizar esta operação no momento. Tente novamente mais tarde");

                return linhasAfetadas;
            }
            catch (Exception ex)
            {
                _mensageria.AdicionarMensagemErro("Ocorreu um erro ao realizar esta operação.");
                _mensageria.AdicionarMensagemErro(ex.ToString());

                return -1;
            }
        }

        /// <summary>
        /// Adiciona mais de uma entidade ao banco de dados. Caso o método <see cref="DbContext.SaveChangesAsync"/> retorne -1 ou ocorra alguma exception no processo,
        /// será adicionado uma mensagem do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado -1 no retorno desta operação
        /// </summary>
        /// <returns>A quantidade de registros afetados por essa operação</returns>
        protected async Task<int> AdicionarEntidade<T>(List<T> entidades)
        {
            try
            {
                await _context.AddRangeAsync(entidades);
                int linhasAfetadas = await _context.SaveChangesAsync();

                if (linhasAfetadas < 0)
                    _mensageria.AdicionarMensagemErro("Não foi possível realizar esta operação no momento. Tente novamente mais tarde");

                return linhasAfetadas;
            }
            catch (Exception ex)
            {
                _mensageria.AdicionarMensagemErro("Ocorreu um erro ao realizar esta operação.");
                _mensageria.AdicionarMensagemErro($"Dados do erro: {ex}");

                return -1;
            }
        }

        /// <summary>
        /// Altera uma entidade ao banco de dados. Caso o método <see cref="DbContext.SaveChangesAsync"/> retorne -1 ou ocorra alguma exception no processo,
        /// será adicionado uma mensagem do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado -1 no retorno desta operação
        /// </summary>
        /// <returns>A quantidade de registros afetados por essa operação</returns>
        protected async Task<int> AlterarEntidade<T>(T entidade)
        {
            try
            {
                _context.Update(entidade);
                int linhasAfetadas = await _context.SaveChangesAsync();

                if (linhasAfetadas < 0)
                    _mensageria.AdicionarMensagemErro("Não foi possível realizar esta operação no momento. Tente novamente mais tarde");

                return linhasAfetadas;
            }
            catch (Exception ex)
            {
                _mensageria.AdicionarMensagemErro("Ocorreu um erro ao realizar esta operação.");
                _mensageria.AdicionarMensagemErro(ex.ToString());

                return -1;
            }
        }

        /// <summary>
        /// Altera mais de uma entidade ao banco de dados. Caso o método <see cref="DbContext.AddAsync"/> retorne -1 ou ocorra alguma exception no processo,
        /// será adicionado uma mensagem do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado -1 no retorno desta operação
        /// </summary>
        /// <returns>A quantidade de registros afetados por essa operação</returns>
        protected async Task<int> AlterarEntidade<T>(List<T> entidades)
        {
            try
            {
                _context.UpdateRange(entidades);
                int linhasAfetadas = await _context.SaveChangesAsync();

                if (linhasAfetadas < 0)
                    _mensageria.AdicionarMensagemErro("Não foi possível realizar esta operação no momento. Tente novamente mais tarde");

                return linhasAfetadas;
            }
            catch (Exception ex)
            {
                _mensageria.AdicionarMensagemErro("Ocorreu um erro ao realizar esta operação.");
                _mensageria.AdicionarMensagemErro($"Dados do erro: {ex}");

                return -1;
            }
        }

        /// <summary>
        /// Deleta uma entidade ao banco de dados. Caso o método <see cref="DbContext.SaveChangesAsync"/> retorne -1 ou ocorra alguma exception no processo,
        /// será adicionado uma mensagem do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado -1 no retorno desta operação
        /// </summary>
        /// <returns>A quantidade de registros afetados por essa operação</returns>
        protected async Task<int> DeletarEntidade<T>(T entidade)
        {
            try
            {
                _context.Remove(entidade);
                int linhasAfetadas = await _context.SaveChangesAsync();

                if (linhasAfetadas < 0)
                    _mensageria.AdicionarMensagemErro("Não foi possível realizar esta operação no momento. Tente novamente mais tarde");

                return linhasAfetadas;
            }
            catch (Exception ex)
            {
                _mensageria.AdicionarMensagemErro("Ocorreu um erro ao realizar esta operação.");
                _mensageria.AdicionarMensagemErro(ex.ToString());

                return -1;
            }
        }

        /// <summary>
        /// Deleta mais de uma entidade ao banco de dados. Caso o método <see cref="DbContext.AddAsync"/> retorne -1 ou ocorra alguma exception no processo,
        /// será adicionado uma mensagem do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado -1 no retorno desta operação
        /// </summary>
        /// <returns>A quantidade de registros afetados por essa operação</returns>
        protected async Task<int> DeletarEntidade<T>(List<T> entidades)
        {
            try
            {
                _context.RemoveRange(entidades);
                int linhasAfetadas = await _context.SaveChangesAsync();

                if (linhasAfetadas < 0)
                    _mensageria.AdicionarMensagemErro("Não foi possível realizar esta operação no momento. Tente novamente mais tarde");

                return linhasAfetadas;
            }
            catch (Exception ex)
            {
                _mensageria.AdicionarMensagemErro("Ocorreu um erro ao realizar esta operação.");
                _mensageria.AdicionarMensagemErro($"Dados do erro: {ex}");

                return -1;
            }
        }

        /// <summary>
        /// Realiza uma query em cima do context informado utilizando um membro herdado da classe <see cref="BaseSpecification{T}"/>
        /// </summary>
        /// <returns>Uma lista de entidades do tipo especificado que satisfaça os critérios informados na classe <see cref="BaseSpecification{T}"/></returns>
        protected async Task<List<T>> ConsultaComSpecification<T>(BaseSpecification<T> specification) where T : class
        {
            var query = AdicionarSpecification<T>(specification);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Realiza uma query em cima do context informado utilizando um membro herdado da classe <see cref="BaseSpecification{T}"/> e retorna
        /// uma lista embrulhada no objeto <see cref="ListaPaginada{T}"/>. Caso ocorra algum erro, será adicionado uma mensagem 
        /// do tipo <see cref="TipoMensagem.Erro"/> no sistema de mensageria e será retornado null no retorno desta operação
        /// </summary>
        /// <returns>Uma lista de entidades do tipo especificado que satisfaça os critérios informados na classe <see cref="BaseSpecification{T}"/></returns>
        protected async Task<ListaPaginada<T>> ConsultaComSpecification<T>(BaseSpecificationPaginated<T> specification) where T : class
        {
            if (specification.Pagina == 0 || specification.QuantidadeRegistros == 0)
                _mensageria.AdicionarMensagemErro("Consulta utilizando paginação não pode ter Página 0 ou Quantidade de Registros 0.");

            if (_mensageria.PossuiErros())
                return null;

            var query = AdicionarSpecification<T>(specification);
            var registrosJaObtidos = specification.Pagina * specification.QuantidadeRegistros;

            var itens = await query
                    .Skip(registrosJaObtidos)
                    .Take(specification.QuantidadeRegistros)
                    .ToListAsync();

            var quantidadeTotalRegistros = await query.CountAsync();
            var quantidadePaginas = (double) quantidadeTotalRegistros / specification.QuantidadeRegistros;
            var quantidadeTotalPaginas = (int) Math.Ceiling(quantidadePaginas);

            return new ListaPaginada<T>(itens, specification.Pagina, quantidadeTotalPaginas);
        }

        private IQueryable<T> AdicionarSpecification<T>(BaseSpecification<T> specification) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            if (specification.Criterios != null)
                query = query.Where(specification.Criterios);

            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending);

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}