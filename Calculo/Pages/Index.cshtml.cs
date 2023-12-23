using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculo.Models.ViewModel;
using Calculo.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Extensions;

/// <summary>
/// indexModel
/// </summary>
public class IndexModel : PageModel
{
    /// <summary>
    /// Valor final que vêm da classe recebimento
    /// </summary>

    public decimal ValorFinal { get; set; }
    /// <summary>
    /// Recebimento
    /// </summary>
    [FromForm]
    public Recebimento Recebimento { get; set; }
    /// <summary>
    /// Irá receber o valor final após o calculo
    /// </summary>
    public string ValorFinalFormatado { get; set; }
    /// <summary>
    /// armazena a referência para a instância do IHttpClientFactory que é passada para o construtor
    /// </summary>
    private readonly IHttpClientFactory _cFactory;
    /// <summary>
    /// este contrutor da classe IndexModel aceita uma instância de IHttpClientFactory como parâmetro
    ///  Essa instância será fornecida pelo sistema de Injeção de Dependência do .NET Core quando uma instância de IndexModel for criada
    /// </summary>
    /// <param name="cFactory"></param>
    public IndexModel(IHttpClientFactory cFactory)
    {
        _cFactory = cFactory;
    }
    /// <summary>
    /// está preparando os dados que serão usados na página quando uma solicitação GET é feita para ela
    /// </summary>
    /// <returns></returns>
    public IActionResult OnGet()
    {

        // Inicializa o objeto Recebimento
        Recebimento = new Recebimento();

        // Verifica se o TempData contém um valor para "ValorFinal"
        if (TempData["ValorFinal"] is string valorFinalStr)
        {
            
            // Tenta converter a string para double
            if (double.TryParse(valorFinalStr, out double valorFinal))
            {
                // Se a conversão for bem-sucedida, atribui o valor à propriedade ValorFinal do objeto Recebimento
                Recebimento.ValorFinal = (decimal)valorFinal;
            }
        }

        // Retorna a página
        return Page();
    }
    /// <summary>
    /// manipulador de ação para o HTTPost
    /// </summary>
    /// <param name="clear">Limpa o valor final formatado</param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync(string clear)
    {

        if (!string.IsNullOrEmpty(clear))
        {

            TempData["ValorFinalFormatado"] = string.Empty;
            if (Recebimento.ValorFinal != 0)
            {
                var url = Request.GetDisplayUrl();
                if (!string.IsNullOrEmpty(url))
                {
                    Response.Redirect(url);
                }

            }
            return Page();
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var valorInicial = Recebimento.ValorIntegral;
        var tempo = Recebimento.NParcelas;
        // endereço do http da outra  API fixada na outra API.
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44350/api/Juros");
        var client = _cFactory.CreateClient();
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var juros = await response.Content.ReadAsStringAsync();
            if (decimal.TryParse(juros, out decimal jurosDecimal))
            {
                decimal valorFinal = 0;
                if (tempo > 1)
                {
                    valorFinal = valorInicial * (decimal)Math.Pow((double)(1m + jurosDecimal), tempo);

                }
                else if (tempo == 1)
                {
                    valorFinal = valorInicial;
                }
                valorFinal = Math.Truncate(100 * valorFinal) / 100;
                ValorFinalFormatado = valorFinal.ToString("F2");
            }

            if (jurosDecimal == 0)
            {
                ValorFinalFormatado = "valor não calculado";
            }


        }

        return Page();
    }

}
