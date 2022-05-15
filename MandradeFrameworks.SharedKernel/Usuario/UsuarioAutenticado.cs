using MandradeFrameworks.SharedKernel.Exceptions;
using MandradeFrameworks.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace MandradeFrameworks.SharedKernel.Usuario
{
    public class UsuarioAutenticado : IUsuarioAutenticado
    {
        public UsuarioAutenticado()
        {
            NomeCompleto = NOME_USUARIO_GENERICO;
            Permissoes = new List<string>();
        }
        
        public string NomeCompleto { get; private set; }
        public string Email { get; private set; }
        public IEnumerable<string> Permissoes { get; private set; }

        private const string NOME_USUARIO_GENERICO = "Token Anônimo";

        public bool EstaAutenticado()
            => NomeCompleto != NOME_USUARIO_GENERICO;

        public bool PossuiPermissao(string permissao)
        {
            if (string.IsNullOrWhiteSpace(permissao))
                return true;

            return Permissoes.Any(pm => pm == permissao);
        }

        public void AlterarDadosUsuario(JwtSecurityToken token)
        {
            if (token is null)
                throw new TokenInvalidoException();

            NomeCompleto = ObterPropriedade(token, "nome");
            Email = ObterPropriedade(token, "email");
            Permissoes = ObterPropriedade(token, "permissoes").Split(",");
        }

        private string ObterPropriedade(JwtSecurityToken token, string propriedade)
            => token.Claims.FirstOrDefault(claim => claim.Type == propriedade).Value;
    }
}
