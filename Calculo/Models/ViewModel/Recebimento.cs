using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Calculo.Models.ViewModel
{
    /// <summary>
    /// Recebimento Class
    /// </summary>
    public class Recebimento
    {
        /// <summary>
        /// esse é o valor total que o usuário irá digitar para que seja calculado o juros juntamente com o numero
        /// de parcelas selecionadas, o numero de parcela é =  a tempo
        /// </summary>

        [Required]
        public decimal ValorIntegral { get; set; }
        /// <summary>
        /// este é o número de parcelas que determina o valor exponencial da fórmula
        /// </summary>

        [Required]
        public int NParcelas { get; set; }
        /// <summary>
        /// este é o valor final do calculo, que depois de utilizar o número das parcelas, juntamente com o 
        /// valor integral surgerido reproduz o valor final
        /// </summary>

        public decimal ValorFinal { get; set; }
        /// <summary>
        /// JurosDoValor
        /// </summary>
        public decimal JurosDoValor { get; set; }
    }
}
