using BarcodeStandard;
using Entidades;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloFinalizarVenta : ModeloDeVistaBase
    {
        #region Propiedades
        private List<Productos> _listaProductos;
        private Ventas _venta;
        private string _metodoDePago;
        private DateTime _fechaRegistrada;
        private decimal _subTotal;
        private decimal _descuento;
        private decimal _total;
        private int _indiceActual;
        private string _vuelto;
        private string _montoPagado;
        private string _nombreTienda;
        public string MontoPagado
        {
            get => _montoPagado;
            set
            {
                _montoPagado = value;
                OnPropertyChanged(nameof(MontoPagado));
            }
        }
        public string Vuelto
        {
            get => _vuelto;
            set
            {
                _vuelto = value;
                OnPropertyChanged(nameof(Vuelto));
            }
        }
        #endregion
        public ModeloFinalizarVenta(Ventas venta)
        {
            _venta = venta;
            _fechaRegistrada = venta.Fecha;
            _metodoDePago = venta.FormaDePago;
            _descuento = venta.Descuento;
            _subTotal = venta.SubTotal;
            _total = venta.Total;
            _metodoDePago = venta.FormaDePago;
            _listaProductos = venta.Productos;
            _indiceActual = 0;
            Vuelto = "0";
            MontoPagado = venta.Total.ToString();
            _nombreTienda = "Negocio";
            ImprimirPagina = new ComandoModeloDeVista(EjecutarImprimirPagina);
        }
        #region Comandos
        public ICommand ImprimirPagina { get; }
        #endregion
        #region Funciones
        private async Task EjecutarImprimirPagina(object arg)
        {
            PrintDocument pd = new PrintDocument(); 
            pd.PrinterSettings.PrinterName = "FUKUNTickets"; 
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); 
            pd.DefaultPageSettings.PaperSize = new PaperSize("Ticket", 280, 600);
            //Sucribimos lso eventos para cuando empiece y termine una impresion 
            pd.PrintPage += new PrintPageEventHandler(ImpresionDePagina);
            pd.EndPrint += new PrintEventHandler(FinDeImpresion); 
            pd.Print();
        }
        /*
         * En esta funciona las variables float que se usan para sacar la pos 'x' de los textos, varian la asignacion de los nombres
         * Quiero decir que xCantidad a veces es el ancho del texto cantidad y otras veces puede ser la posicion del texto cantidad
        */
        private void ImpresionDePagina(object sender, PrintPageEventArgs e)
        {
            Font fuenteDeco = new Font("Roboto Flex", 10);
            Font fuenteInfo = new Font("Roboto Flex", 8);
            Font fuenteInfoBold = new Font("Roboto Flex", 8, FontStyle.Bold);
            Font fuenteTitutlo = new Font("Roboto Flex", 18, FontStyle.Bold);
            Font fuenteIntermedia = new Font("Roboto Flex", 12, FontStyle.Bold);
            int nLocal = 1;
            int idVenta = 1;
            float limiteIzquierdo = e.MarginBounds.Left;
            float limiteDerecho = e.MarginBounds.Right;
            string nombreNegocio = _nombreTienda.ToUpper(); ;
            string ticket = idVenta.ToString("D8");
            string direccionDelNegocio = "Gabriela Mistral 3591 - Cordoba Argentina";
            string numeroTicket = $"TICKET DE COMPRA\nNº {nLocal.ToString("D4")}-{idVenta.ToString("D8")}";
            string divisor = "---------------------------------------------------------";
            SizeF anchoNombreTiendaUno = e.Graphics.MeasureString(_nombreTienda, fuenteInfo);
            ///
            string tituloFormaDePago = "Forma de pago: ";
            string tituloMontoPagado = "Monto pagado: ";
            string tituloVuelto = "Vuelto: ";
            SizeF anchoTituloFormaDePago = e.Graphics.MeasureString(tituloFormaDePago, fuenteInfoBold);
            SizeF anchoTituloMontoPagado = e.Graphics.MeasureString(tituloMontoPagado, fuenteInfoBold);
            SizeF anchoTituloVuelto = e.Graphics.MeasureString(tituloVuelto, fuenteInfoBold);
            float xFormaDePago = limiteIzquierdo + anchoTituloFormaDePago.Width;
            float xMontoPagado = limiteIzquierdo + anchoTituloMontoPagado.Width;
            float xVuelto = limiteIzquierdo + anchoTituloVuelto.Width;
            ///
            string tituloSubTotal = "SUBTOTAL: ";
            string tituloDescuento = "DESCUENTO: ";
            string tituloPrecioTotal= "TOTAL: ";
            SizeF anchoTituloSubTotal = e.Graphics.MeasureString(tituloSubTotal, fuenteInfoBold);
            SizeF anchoTituloDescuento = e.Graphics.MeasureString(tituloDescuento, fuenteInfoBold);
            SizeF anchoTituloPrecioTotal = e.Graphics.MeasureString(tituloPrecioTotal, fuenteInfoBold);
            string _strSubTotal = "$" + _subTotal.ToString("N2", new CultureInfo("es-AR"));
            SizeF anchoSubTotal = e.Graphics.MeasureString(_strSubTotal, fuenteInfoBold);
            SizeF anchoDescuento = e.Graphics.MeasureString(_descuento.ToString(), fuenteInfoBold);
            string _strTotal = "$" + _total.ToString("N2", new CultureInfo("es-AR"));
            SizeF anchoPrecioTotal = e.Graphics.MeasureString(_strTotal, fuenteInfoBold);
            float xDetallesVenta = limiteDerecho - anchoTituloDescuento.Width - anchoSubTotal.Width;
            float xSubTotal = limiteDerecho - anchoSubTotal.Width;
            float xDescuento = limiteDerecho - anchoDescuento.Width;
            float xPrecioTotal = limiteDerecho - anchoPrecioTotal.Width;
            ///
            string tituloCajero = "Cajero: ";
            string tituloCaja = "Caja: ";
            string nombreCajero = "ADMIN";
            string numeroCaja = 1.ToString("D3");
            SizeF anchoCajero= e.Graphics.MeasureString(tituloCajero, fuenteInfoBold);
            SizeF anchoCaja= e.Graphics.MeasureString(tituloCaja, fuenteInfoBold);
            SizeF anchoNombreCajero = e.Graphics.MeasureString(nombreCajero, fuenteInfoBold);
            SizeF anchoNumeroCaja = e.Graphics.MeasureString(numeroCaja, fuenteInfoBold);
            float xNombreCajero = limiteIzquierdo + anchoCajero.Width;
            float xCaja = limiteDerecho - anchoNombreCajero.Width - anchoNumeroCaja.Width;
            float xNumeroCaja = limiteDerecho - anchoNumeroCaja.Width;
            ///
            string tituloFecha = "Fecha: ";
            string titulohora = "Hora: ";
            string fechaActual = _fechaRegistrada.ToString("dd/MM/yyyy");
            SizeF anchoTituloFecha = e.Graphics.MeasureString(tituloFecha, fuenteInfo);
            SizeF anchoTituloHora = e.Graphics.MeasureString(titulohora, fuenteInfo);
            string horaActual = _fechaRegistrada.ToString("HH:mm:ss");
            SizeF anchoHoraActual = e.Graphics.MeasureString(horaActual, fuenteInfo);
            float xFechaActual = e.MarginBounds.Left + anchoTituloFecha.Width;
            float xHoraActual = limiteDerecho - anchoHoraActual.Width;
            float xTituloHora = limiteDerecho - anchoHoraActual.Width - anchoTituloHora.Width;
            ///Esto es para medir el ancho del texto, lo usaremos para centrarlo 
            SizeF achoNombreNegocio = e.Graphics.MeasureString(nombreNegocio, fuenteTitutlo);
            SizeF achoNumeroTicket = e.Graphics.MeasureString(numeroTicket, fuenteIntermedia);
            SizeF achoDireccionDelNegocio = e.Graphics.MeasureString(direccionDelNegocio, fuenteInfo);
            SizeF anchoDeco = e.Graphics.MeasureString(divisor, fuenteDeco);
            /// Calcullamos su posicion centrada
            float nombreNegocioCentrado = limiteIzquierdo + (e.MarginBounds.Width - achoNombreNegocio.Width) / 2;
            float direccionDelNegocioCentrado = limiteIzquierdo + (e.MarginBounds.Width - achoDireccionDelNegocio.Width) / 2;
            float numeroTicketCentrado = limiteIzquierdo + (e.MarginBounds.Width - achoNumeroTicket.Width) / 2;
            float divisorCentrado = limiteIzquierdo + (e.MarginBounds.Width - anchoDeco.Width) / 2;
            float xNombreTiendaUnoCentrado = limiteIzquierdo + (e.MarginBounds.Width - anchoNombreTiendaUno.Width) / 2;
            float y = 10;
            e.Graphics.DrawString(nombreNegocio, fuenteTitutlo, Brushes.Black, nombreNegocioCentrado, y);
            y += 40;
            e.Graphics.DrawString(direccionDelNegocio, fuenteInfo, Brushes.Black, direccionDelNegocioCentrado, y);
            y += 20;
            e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
            y += 20;
            e.Graphics.DrawString(numeroTicket, fuenteIntermedia, Brushes.Black, numeroTicketCentrado, y);
            y += 40;
            e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
            y += 20;
            e.Graphics.DrawString(fechaActual, fuenteInfo, Brushes.Black, xFechaActual, y);
            e.Graphics.DrawString(horaActual, fuenteInfo, Brushes.Black, xHoraActual, y);
            e.Graphics.DrawString(tituloFecha, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
            e.Graphics.DrawString(titulohora, fuenteInfoBold, Brushes.Black, xTituloHora, y);
            y += 15;
            e.Graphics.DrawString(tituloCajero, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
            e.Graphics.DrawString(nombreCajero, fuenteInfo, Brushes.Black, xNombreCajero, y);
            e.Graphics.DrawString(tituloCaja, fuenteInfoBold, Brushes.Black, xTituloHora, y);
            e.Graphics.DrawString(numeroCaja, fuenteInfo, Brushes.Black, xHoraActual, y);
            y += 20;
            e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
            y += 20;
            while (_indiceActual < _listaProductos.Count)
            {
                string vCantidad = _listaProductos[_indiceActual].Cantidad.ToString() + "x";
                string vPrecioVenta = "$" + decimal.Parse(_listaProductos[_indiceActual].Venta).ToString("N2", new CultureInfo("es-AR"));
                string vNombre = _listaProductos[_indiceActual].Nombre.ToUpper();
                decimal vPrecioTotal = decimal.Parse(_listaProductos[_indiceActual].Venta, NumberStyles.Any, new CultureInfo("es-AR")) * _listaProductos[_indiceActual].Cantidad;
                string strVPrecioTotal = "$" + vPrecioTotal.ToString("N2", new CultureInfo("es-AR"));
                SizeF anchoVCantidad = e.Graphics.MeasureString(vCantidad, fuenteInfo);
                SizeF anchoVPrecioTotal = e.Graphics.MeasureString(strVPrecioTotal, fuenteInfo);
                float posPrecioTotal = limiteDerecho - anchoVPrecioTotal.Width;
                float xVCantidad = anchoVCantidad.Width + limiteIzquierdo;
                e.Graphics.DrawString(vCantidad, fuenteInfo, Brushes.Black, limiteIzquierdo, y);
                e.Graphics.DrawString(vPrecioVenta, fuenteInfo, Brushes.Black, xVCantidad, y);
                y += 15;
                e.Graphics.DrawString(vNombre, fuenteInfo, Brushes.Black, limiteIzquierdo, y);
                e.Graphics.DrawString(strVPrecioTotal, fuenteInfo, Brushes.Black, posPrecioTotal, y);
                y += 20;
                _indiceActual++;

                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
            e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
            y += 20;
            e.Graphics.DrawString(tituloSubTotal, fuenteInfoBold, Brushes.Black, xDetallesVenta, y);
            e.Graphics.DrawString(_strSubTotal, fuenteInfo, Brushes.Black, xSubTotal, y);
            y += 15;
            e.Graphics.DrawString(tituloDescuento, fuenteInfoBold, Brushes.Black, xDetallesVenta, y);
            e.Graphics.DrawString(_descuento.ToString(), fuenteInfo, Brushes.Black, xDescuento, y);
            y += 15;
            e.Graphics.DrawString(tituloPrecioTotal, fuenteInfoBold, Brushes.Black, xDetallesVenta, y);
            e.Graphics.DrawString(_strTotal, fuenteInfo, Brushes.Black, xPrecioTotal, y);
            y += 20;
            e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
            y += 20;
            e.Graphics.DrawString(tituloFormaDePago, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
            e.Graphics.DrawString(_metodoDePago, fuenteInfoBold, Brushes.Black, xFormaDePago, y);
            y += 15;
            e.Graphics.DrawString(tituloMontoPagado, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
            e.Graphics.DrawString(MontoPagado, fuenteInfoBold, Brushes.Black, xMontoPagado, y);
            y += 15;
            e.Graphics.DrawString(tituloVuelto, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
            e.Graphics.DrawString(Vuelto, fuenteInfoBold, Brushes.Black, xVuelto, y);
            y += 20;
            e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
            y += 20;
            e.Graphics.DrawString(_nombreTienda, fuenteInfo, Brushes.Black, xNombreTiendaUnoCentrado, y);
            y += 20;
            Barcode barcode = new Barcode(); 
            var img = barcode.Encode(BarcodeStandard.Type.Code128, ticket, new SKColor(0, 0, 0), new SKColor(255, 255, 255), 200, 30);
            float xCodigobarras = limiteIzquierdo + (e.MarginBounds.Width - img.Width) / 2;
            //Esto se usa por que e.Graphics.DrawImage espera un objeto de tipo Image
            using (SKData data = img.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = new System.IO.MemoryStream(data.ToArray()))
            {
                Image imagen = Image.FromStream(stream);
                e.Graphics.DrawImage(imagen, xCodigobarras, y);
            }
            y += 35;
            SizeF anchoTicket = e.Graphics.MeasureString(ticket, fuenteInfo);
            float xTicket = limiteIzquierdo + (e.MarginBounds.Width - anchoTicket.Width) / 2;
            e.Graphics.DrawString(ticket, fuenteInfo, Brushes.Black, xTicket, y);
            e.HasMorePages = false;
            _indiceActual = 0;
        }
        // Cuando termina la impresión, mando el comando de corte
        private void FinDeImpresion(object sender, PrintEventArgs e)
        {
            // Avanza unas líneas para que quede espacio debajo del ticket
            byte[] feed = new byte[] { 0x1B, 0x64, 0x06 }; // 6? líneas CREO QUE EL 3 PARAMETRO SON LAS LINEAS
            ComportamientoImpresora.SendBytesToPrinter("FUKUNTickets", feed);
            byte[] cut = new byte[] { 0x1D, 0x56, 0x00 }; // ESC/POS corte total
            ComportamientoImpresora.SendBytesToPrinter("FUKUNTickets", cut);
        }
        #endregion
    }
}
