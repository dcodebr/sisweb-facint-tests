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
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
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