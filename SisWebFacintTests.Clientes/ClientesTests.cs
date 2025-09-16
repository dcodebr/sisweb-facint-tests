using NuGet.Frameworks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SisWebFacintTests.Clientes;

[TestFixture]
public class ClientesTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {

        var options = new ChromeOptions();

        if (Environment.GetEnvironmentVariable("CI") == "true")
        {
            options.AddArgument("--headless=new"); // modo headless atualizado
            options.AddArgument("--no-sandbox"); // obrigatório em runners Linux
            options.AddArgument("--disable-dev-shm-usage"); // evita problemas de memória
            options.AddArgument("--disable-gpu"); // geralmente seguro
            options.AddArgument("--remote-debugging-port=9222");

            options.AddArgument("--user-data-dir=/tmp/chrome-user-data-" + Guid.NewGuid());
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-background-networking");
            options.AddArgument("--disable-sync");

        }
        else
        {
            options.AddArgument("--start-maximized");
        }

        driver = new ChromeDriver(options);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }


    [Test]
    public void Incluir_Cadastro_Com_Sucesso()
    {
        driver.Navigate().GoToUrl("http://localhost:8080/clientes");

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(dvr => dvr.FindElement(By.Id("txtnome")));

        var txtNome = driver.FindElement(By.Id("txtnome"));
        var txtCPF = driver.FindElement(By.Id("txtcpf"));
        var txtEndereco = driver.FindElement(By.Id("txtendereco"));
        var txtCelular = driver.FindElement(By.Id("txtcelular"));
        var btnAdicionar = driver.FindElement(By.Id("btnadicionar"));

        var dados = new
        {
            Nome = "Alex Diego Araujo da Rocha",
            CPF = "11122233344",
            Endereco = "Rua Barão de Maua, 106. Maringá-PR",
            Celular = "44998729313"
        };

        txtNome.SendKeys(dados.Nome);
        txtEndereco.SendKeys(dados.Endereco);

        txtCPF.Clear();
        foreach (var c in dados.CPF)
        {
            txtCPF.SendKeys(c.ToString());
            Thread.Sleep(50);    
        }

        txtCelular.Clear();
        foreach (var c in dados.Celular)
        {
            txtCelular.SendKeys(c.ToString());
            Thread.Sleep(50);    
        }

        btnAdicionar.Click();

        var tabela = driver.FindElement(By.Id("tabela"));
        var tBody = tabela.FindElement(By.TagName("tbody"));
        var linhas = tBody.FindElements(By.TagName("tr"));

        var ultimaLinha = linhas.LastOrDefault();

        var colunas = ultimaLinha?.FindElements(By.TagName("td"));

        var dadosTabela = new
        {
            Nome = colunas?[1].Text,
            CPF = colunas?[2].Text.Replace(".", "").Replace("-", ""),
            Endereco = colunas?[3].Text,
            Celular = colunas?[4].Text
        };

        Assert.Multiple(() =>
        {
            Assert.That(dadosTabela.Nome, Is.EqualTo(dados.Nome));
            Assert.That(dadosTabela.CPF, Is.EqualTo(dados.CPF));
            Assert.That(dadosTabela.Endereco, Is.EqualTo(dados.Endereco));
            Assert.That(dadosTabela.Celular, Is.EqualTo(dados.Celular));
        });
    }
}