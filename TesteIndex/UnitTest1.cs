using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq.Protected;
using Calculo.Models.ViewModel;

/// <summary>
/// teste
/// </summary>
[TestClass]
public class IndexModelTests
{
    /// <summary>
    /// usando o IHttpClientFactory
    /// </summary>
    private Mock<IHttpClientFactory> _mockFactory;
    /// <summary>
    /// usando o HttpMessageHandler
    /// </summary>
    private Mock<HttpMessageHandler> _mockHandler;
    /// <summary>
    /// IndexModel
    /// </summary>
    private IndexModel _indexModel;
    /// <summary>
    /// inicialização do teste
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        _mockFactory = new Mock<IHttpClientFactory>();
        _mockHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockHandler.Object);
        _mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _indexModel = new IndexModel(_mockFactory.Object);
    }
    /// <summary>
    /// metodo teste
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task OnPostAsync_ValidInput_ReturnsPageResult()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("0.01"),
        };

        _mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        _indexModel.Recebimento = new Recebimento
        {
            ValorIntegral = 1000,
            NParcelas = 6
        };

        // Act
        var result = await _indexModel.OnPostAsync("");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        Assert.AreEqual("64000,00", _indexModel.ValorFinalFormatado);
    }
    /// <summary>
    /// metodo Teste
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task OnPostAsync_ApiError_ReturnsPageResult()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError, // Simula um erro da API
        };

        _mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        _indexModel.Recebimento = new Recebimento
        {
            ValorIntegral = 1000,
            NParcelas = 6
        };

        // Act
        var result = await _indexModel.OnPostAsync("");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
  
    }
    /// <summary>
    /// Metodo Teste
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task OnPostAsync_InvalidModelState_ReturnsPageResult()
    {
        // Arrange
        _indexModel.ModelState.AddModelError("Erro", "Erro de validação"); // Adiciona um erro ao ModelState

        // Act
        var result = await _indexModel.OnPostAsync("");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        Assert.IsNull(_indexModel.ValorFinalFormatado);
    }

}
