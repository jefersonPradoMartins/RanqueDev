# RanqueDev API REST 
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

### Descrição do Projeto
O projeto RanqueDev é uma API Rest para simular um CRUD de perguntas e respostas, no entanto o objetivo é demonstrar uma aplicação com funcionalidade 
de criar login e senha e permitir ou negar acesso a determinados end-points. 

- Utilizei System.ComponentModel.DataAnnotations para validar as classes DTO usadas nos end-points, a baixo um exemplo da aplicação no código.
``` Csharp
    public record UserDto
    {
        [EmailAddress(ErrorMessage = "O campo Email não é um endereço de email válido.")]
        [MaxLength(256, ErrorMessage = "O campo Email deve ser do tipo string ou array com comprimento máximo de '256'.")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(256, ErrorMessage = "O campo UserName deve ser do tipo string ou array com comprimento máximo de '256'.")]
        public string UserName { get; set; } = string.Empty;

        [StringLength(maximumLength: 25, MinimumLength = 4,
        ErrorMessage = "O campo Password deve ser do tipo string ou array com comprimento mínimo de '4' e máximo de '25'.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
```
> [Saiba mais](https://learn.microsoft.com/pt-br/aspnet/mvc/overview/older-versions-1/models-data/validation-with-the-data-annotation-validators-cs)


- Utilizei Identity do ASP.NET Core conciliado com SMTP para enviar o token de confirmação de registro ou recuperação de senha.   
> O ASP.NET Core Identity é um framework de autenticação e autorização projetado para facilitar a implementação de recursos relacionados
à identidade do usuário em aplicativos web ASP.NET Core. Ele fornece uma estrutura robusta para gerenciar usuários, senhas, perfis,
autenticação por dois fatores, tokens de autenticação, recuperação de senha e outras funcionalidades relacionadas à segurança.

- Utilizei Unit of Work em repositorio de entidade que possuem transações atomicas. Se suas operações no banco de dados precisam ser
   tratadas como uma única unidade atômica ou seja, todas têm êxito ou todas falham.
> [Saiba mais](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)





