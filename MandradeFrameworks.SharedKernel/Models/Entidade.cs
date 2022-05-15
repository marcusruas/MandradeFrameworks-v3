using System;
using System.Collections.Generic;
using System.Text;

namespace MandradeFrameworks.SharedKernel.Models
{
    /// <summary>
    /// Entidade com propriedades básicas
    /// </summary>
    public abstract class Entidade
    {
        /// <summary>
        /// Instanciar entidade com novo Id e data de criação como a data atual
        /// </summary>
        public Entidade()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
        }

        /// <summary>
        /// Identificador da entidade
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Data em que a entidade foi criada
        /// </summary>
        public DateTime DataCriacao { get; set; }
        /// <summary>
        /// Usuário criador da entidade
        /// </summary>
        public string CriadoPor { get; set; }
        /// <summary>
        /// Última vez que a entidade foi atualizada
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
        /// <summary>
        /// Último usuário que atualizou a entidade
        /// </summary>
        public string AtualizadoPor { get; set; }

        public void DefinirUsuarioCriador(string usuario)
        {
            DataCriacao = DateTime.Now;
            CriadoPor = usuario;
        }

        public void DefinirUsuarioAtualizador(string usuario)
        {
            DataAtualizacao = DateTime.Now;
            AtualizadoPor = usuario;
        }
    }
}
