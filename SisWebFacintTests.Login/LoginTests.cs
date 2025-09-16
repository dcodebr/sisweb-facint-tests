using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SisWebFacintTests.Login;

[TestFixture]
public class LoginTests
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

    [Test]
    public void Realizar_Login_Bem_Sucedido()
    {
        driver.Navigate().GoToUrl("http://localhost:8080");

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(dvr => dvr.FindElement(By.Id("btnlogin")));

        IWebElement txtEmail = driver.FindElement(By.Id("txtemail"));
        IWebElement txtSenha = driver.FindElement(By.Id("txtsenha"));
        IWebElement btnLogin = driver.FindElement(By.Id("btnlogin"));
        IWebElement pResultado = driver.FindElement(By.Id("resultado"));

        //Efetuar login
        txtEmail.SendKeys("usuario@usuario.com.br");
        txtSenha.SendKeys("123456789");
        btnLogin.Click();

        var resultado = pResultado.Text;

        Assert.That(resultado, Is.EqualTo("Usuário autenticado."), "Falhou!");
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }
}