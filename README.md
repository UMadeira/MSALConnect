# MSALConnect

Exemplo de uma aplicação ASP.NET MVC para demonstrar a utilização do [Microsoft.Graph](https://graph.microsoft.io/).   

Esta aplicação utiliza a Microsoft Authentication Library (MSAL) que permite a autentificação no Azure Active Directory e também através de contas Microsoft (MSA). O MSAL ainda está em desenvolvimento e supostamente o processo de sign-out não é completamente suportado.

Atendendo a que o objetivo é a utilização do Azure Active Directory associada aos domínios de student.uma.pt ou staff.uma.pt, está a ser utilizado o endpoint ```https://login.microsoftonline.com/<TenantId>/v2.0``` em vez do endpoint genérico ```https://login.microsoftonline.com/common/v2.0```. 

A principal vantagem é que, através deste endpoint específico, a autentificação é redirecionada diretamente para o Azure Active Directory sem necessidade de realizar a escolha prévia entre Azure Active Diretory e Microsoft Account.

## Alterações que são necessário realizar

1. Gerar um ```App Id``` e um ```App Secret``` para a aplicação através do site [Application Registration Portal](https://apps.dev.microsoft.com).
2. Alterar o ficheiro ```Web.config``` e substituir nas seguintes linhas os valores obtivos no site de registo.
```
  <!-- https://apps.dev.microsoft.com -->
  <add key="ida:AppId"     value="<App Id>" />
  <add key="ida:AppSecret" value="<App Secret>" />