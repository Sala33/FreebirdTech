# Freebird

<!---Esses são exemplos. Veja https://shields.io para outras pessoas ou para personalizar este conjunto de escudos. Você pode querer incluir dependências, status do projeto e informações de licença aqui--->

![GitHub status](https://img.shields.io/github/checks-status/Sala33/FreebirdTech/82f0cc49f3e86f97cd259c4dec2189954a98df1f)
![Discord Chat](https://img.shields.io/discord/785957787594915851)
![Size](https://img.shields.io/github/repo-size/Sala33/FreebirdTech)
![License](https://img.shields.io/github/license/Sala33/FreebirdTech)

<img src="wwwroot/img/backg1.png" alt="exemplo imagem">

>Ferramenta Open Source para fomentar empresas, ecossistemas, organizações e associações da Indústria Criativa. Cada usuário tem liberdade para executar, acessar, modificar e desenvolver o código fonte para ser implementado em ecossistemas criativos.

### Ajustes e melhorias

Roadmap:

- [x] Arquitetura MVC em Blazor
- [x] Back end .NetCore
- [x] Modificação para MicroServiços
- [ ] Habilitação da API em .NetCore
- [ ] Habilitação de plugins de Front-End em React

## 💻 Pré-requisitos
<!---Estes são apenas requisitos de exemplo. Adicionar, duplicar ou remover conforme necessário--->
* .Net 5.0
* PostGreS para funcionalidade out-of-the box, porém qualquer banco pode ser implementado de acordo com o [guia](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli).
* [Documentação](https://freebirdcore.herokuapp.com/documentation/index.html).
* [Docker](https://docs.docker.com/get-docker/)

## 🚀 Instalando o Freebird

Para instalar o Freebird, siga estas etapas:

### Maps API
* Primeiro obtenha uma chave da API grátis do [Google Maps](https://developers.google.com/maps/documentation)
* Vá até Views/Servicos/Details.cshtml e procure a linha 206.
* Modifique a chave {YOUR_API_KEY_HERE} para utilizar a chave obtida pelo google

### PostGres
* Use o serviço de database de sua preferência, recomendamos o Tier grátis do [Heroku](https://devcenter.heroku.com/articles/heroku-postgresql)
* Vá até o arquivo appsettings.json e encontre as chaves `{HOST}` `{DATABASE}` `{PORT}` `{USERNAME}` `{PASSWORD}` e modifique para utilizar os valores fornecidos pelo serviço utilizado
* Ou caso seja disponível troque pela string de conexão fornecida pelo banco de dados.

### O projeto é baseado em docker para facilitar a implantação.

* Para disponibilizar o app online recomendamos a utilização do Tier grátis do [Heroku](https://medium.com/@lorranpalmeira/deploy-de-uma-aplica%C3%A7%C3%A3o-net-core-3-0-docker-no-heroku-481c21f8d3c5) o projeto já está configurado para a utilização no Heroku, portanto só os passos de configuração da conta e do CLI são necessários.
* Abra o arquivo Deploy.ps1 no seu editor de texto de preferência.
* Troque o nome nas chaves `{APP-NAME}` para o nome definido no passo anterior.
* Abra o Powershell ou CMD no windows e execute o arquivo Deploy.ps1

### Construir a imagem do Docker manualmente:
```
docker build -t <nome> .
docker run <nome>
```
### Visual Studio

Caso esteja usando o Visual Studio para edição do código o freebird é configurado para utilizar o botão run do docker no Visual Studio;

## ☕ Usando <nome_do_projeto>

Para usar <nome_do_projeto>, siga estas etapas:

```
<exemplo_de_uso>
```

Adicione comandos de execução e exemplos que você acha que os usuários acharão úteis. Fornece uma referência de opções para pontos de bônus!

## 📫 Contribuindo para <nome_do_projeto>
<!---Se o seu README for longo ou se você tiver algum processo ou etapas específicas que deseja que os contribuidores sigam, considere a criação de um arquivo CONTRIBUTING.md separado--->
Para contribuir com <nome_do_projeto>, siga estas etapas:

1. Bifurque este repositório.
2. Crie um branch: `git checkout -b <nome_branch>`.
3. Faça suas alterações e confirme-as: `git commit -m '<mensagem_commit>'`
4. Envie para o branch original: `git push origin <nome_do_projeto> / <local>`
5. Crie a solicitação de pull.

Como alternativa, consulte a documentação do GitHub em [como criar uma solicitação pull](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## 🤝 Colaboradores

Agradecemos às seguintes pessoas que contribuíram para este projeto:

<table>
  <tr>
    <td align="center">
      <a href="#">
        <img src="https://avatars3.githubusercontent.com/u/31936044" width="100px;" alt="Foto do Iuri Silva no GitHub"/><br>
        <sub>
          <b>Iuri Silva</b>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="#">
        <img src="https://s2.glbimg.com/FUcw2usZfSTL6yCCGj3L3v3SpJ8=/smart/e.glbimg.com/og/ed/f/original/2019/04/25/zuckerberg_podcast.jpg" width="100px;" alt="Foto do Mark Zuckerberg"/><br>
        <sub>
          <b>Mark Zuckerberg</b>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="#">
        <img src="https://miro.medium.com/max/360/0*1SkS3mSorArvY9kS.jpg" width="100px;" alt="Foto do Steve Jobs"/><br>
        <sub>
          <b>Steve Jobs</b>
        </sub>
      </a>
    </td>
  </tr>
</table>


## 😄 Seja um dos contribuidores<br>

Quer fazer parte desse projeto? Clique [AQUI](CONTRIBUTING.md) e leia como contribuir.

## 📝 Licença

Esse projeto está sob licença. Veja o arquivo [LICENÇA](LICENSE.md) para mais detalhes.

[⬆ Voltar ao topo](#nome-do-projeto)<br>
