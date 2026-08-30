using BarcodeStandard;
using Dominio;
using Entidades;
using Kiosco2026.Clases.Colores;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas.Hijas
{
    public class ModeloFinalizarVenta : ModeloDeVistaBase
    {
        #region Propiedades
        ConfiguracionImpresora _impresora;
        CultureInfo _cultura;
        private List<Productos> _listaProductos;
        private ObservableCollection<Productos> _listadoDeProductos;
        public ObservableCollection<Productos> ListadoDeProductos
        {
            get => _listadoDeProductos;
            set
            {
                _listadoDeProductos = value;
                OnPropertyChanged(nameof(Productos));
            }
        }
        private Ventas _venta;
        private string _metodoDePago;
        private DateTime _fechaRegistrada;
        /*
                string fechaActual = _fechaRegistrada.ToString("dd/MM/yyyy");
                SizeF anchoTituloFecha = e.Graphics.MeasureString(tituloFecha, fuenteInfo);
                SizeF anchoTituloHora = e.Graphics.MeasureString(titulohora, fuenteInfo);
                string horaActual = _fechaRegistrada.ToString("HH:mm:ss");*/
        public string FechaDiaMesAno { 
            get{
                string fecha = _fechaRegistrada.ToString("dd/MM/yyyy");
                return fecha;
            } 
        }
        public string HoraMinutoSeg
        {
            get
            {
                string horaActual = _fechaRegistrada.ToString("HH:mm:ss");
                return horaActual;
            }
        }
        private decimal _subTotal;
        public string SubTotal
        {
            get => "$"+_subTotal.ToString("N2", new CultureInfo("es-AR"));
            set
            {
                _subTotal = decimal.Parse(value, NumberStyles.Any, new CultureInfo("es-AR"));
                OnPropertyChanged(nameof(SubTotal));
            }
        }
        private decimal _descuento;
        public string Descuento
        {
            get => "$"+_descuento.ToString("N2", new CultureInfo("es-AR"));
            set
            {
                _descuento = decimal.Parse(value, NumberStyles.Any, new CultureInfo("es-AR"));
                OnPropertyChanged(nameof(Descuento));
            }
        }
        private decimal _total;
        public string Total
        {
            get => "$" + _total.ToString("N2", new CultureInfo("es-AR"));
            set
            {
                _total = decimal.Parse(value, NumberStyles.Any, new CultureInfo("es-AR"));
                OnPropertyChanged(nameof(Total));
            }
        }
        private int _indiceActual;
        private decimal _vuelto;
        public string Vuelto
        {
            get => "$" + _vuelto.ToString("N2", new CultureInfo("es-AR"));
            set
            {

                _vuelto = decimal.Parse(value.ToString(), NumberStyles.Any, new CultureInfo("es-AR"));
                OnPropertyChanged(nameof(Vuelto));
            }
        }
        private string _montoPagado;
        public string MontoPagado
        {
            get => _montoPagado;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && decimal.TryParse(value, out decimal n))
                {
                        _montoPagado = value;
                }
                else
                    _montoPagado = "0";
                ActualizarVuelto();
                OnPropertyChanged(nameof(MontoPagado));
            }
        }
        private string _nombreTienda;
        private string _caja;
        private string _cajero;
        private string _idVenta;
        public string IdVenta
        {
            get => _idVenta;
            set
            {
                _idVenta = value;
                OnPropertyChanged(nameof(IdVenta));
            }
        }
        private string _direccionTienda;
        private Configuracion _configuracion;
        private string _ticketDeCompra;
        private AdminVentas _adminVentas;
        public string TicketDeCompra
        {
            get => _ticketDeCompra;
            set
            {
                _ticketDeCompra = value;
                OnPropertyChanged(nameof(TicketDeCompra));
            }
        }
        public string DireccionTienda
        {
            get => _direccionTienda;
            set
            {
                _direccionTienda = value;
                OnPropertyChanged(nameof(DireccionTienda));
            }
        }
        public string Caja
        {
            get => _caja;
            set
            {
                _caja = value;
                OnPropertyChanged(nameof(Caja));
            }
        }
        public string Cajero
        {
            get => _cajero;
            set
            {
                _cajero = value;
                OnPropertyChanged(nameof(Cajero));
            }
        }
        public string MetodoDePago
        {
            get => _metodoDePago;
            set
            {
                _metodoDePago = value;
                OnPropertyChanged(nameof(MetodoDePago));
            }
        }
        public string NombreTienda
        {
            get => _nombreTienda;
            set
            {
                _nombreTienda = value;
                OnPropertyChanged(nameof(NombreTienda));
            }
        }

        private ColoresProductos _coloresEntradas;
        public ColoresProductos ColoresEntradas
        {
            get => _coloresEntradas;
            set
            {
                _coloresEntradas = value;
                OnPropertyChanged(nameof(_coloresEntradas));
            }
        }
        #endregion
        public ModeloFinalizarVenta(Ventas venta)
        {
            _cultura = new CultureInfo("es-AR");
            _coloresEntradas = new ColoresProductos();
            ColoresEntradas.MontoPagado = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF"));
            GotFocus = new ComandoModeloDeVista(EjecutarGotFocus);
            LostFocus = new ComandoModeloDeVista(EjecutarLostFocus);
            string ruta = "config.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _configuracion = JsonSerializer.Deserialize<Configuracion>(jsonDeConfiguracion);
            _venta = venta;
            _fechaRegistrada = venta.Fecha;
            _metodoDePago = venta.FormaDePago;
            Descuento = venta.Descuento.ToString();
            SubTotal = venta.SubTotal.ToString();
            _total = venta.Total;
            _listaProductos = venta.Productos;
            ListadoDeProductos = new ObservableCollection<Productos>(_listaProductos);
            _indiceActual = 0;
            Vuelto = "0";
            MontoPagado = venta.Total.ToString();
            _nombreTienda = _configuracion.NombreNegocio;
            Caja = _configuracion.NumeroDeCaja.ToString("D3");
            Cajero = _configuracion.NombreDeCaja;
            _adminVentas = new AdminVentas();
            EjecutarRegistrarVenta(null);
            DireccionTienda = _configuracion.DireccionDelLocal;
            ImprimirPagina = new ComandoModeloDeVista(EjecutarImprimirPagina);
        }
        #region Comandos
        public ICommand ImprimirPagina { get; }
        public ICommand GotFocus { get; }
        public ICommand LostFocus { get; }
        #endregion
        #region Funciones
        private void ActualizarVuelto()
        {
            if(decimal.TryParse(_montoPagado, out decimal monto)){
                decimal resultado = monto - _total;
                Vuelto = resultado.ToString(_cultura);
            }
        }
        private async Task EjecutarRegistrarVenta(object arg)
        {
            var datos = await _adminVentas.RegistrarVenta(_venta);
            IdVenta = datos.id.ToString("D8");
            TicketDeCompra = $"TICKET DE COMPRA \nNº{_configuracion.NumeroDeSucursal.ToString("D4")}-{IdVenta}";
        }
        private async Task EjecutarImprimirPagina(object arg)
        {
            PrintDocument pd = new PrintDocument();
            string ruta = "configImpresora.json";
            string jsonDeConfiguracion = File.ReadAllText(ruta);
            _impresora = JsonSerializer.Deserialize<ConfiguracionImpresora>(jsonDeConfiguracion);
            //Aca convertimos el 80 mm a centésimas de pulgada. PaperSize usa centésimas de pulgada (1/100 inch)
            int anchoPapel = (int)(_impresora.AnchoPapel / 25.4 * 100);
            pd.PrinterSettings.PrinterName = _impresora.NombreImpresora; 
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            Font fuenteInfo = new Font("Roboto Flex", 8);
            pd.DefaultPageSettings.PaperSize = new PaperSize("Ticket", anchoPapel, 10000);
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
            try
            {

                Font fuenteDeco = new Font("Roboto Flex", 10);
                Font fuenteInfo = new Font("Roboto Flex", 8);
                Font fuenteInfoBold = new Font("Roboto Flex", 8, FontStyle.Bold);
                Font fuenteTitutlo = new Font("Roboto Flex", 18, FontStyle.Bold);
                Font fuenteIntermedia = new Font("Roboto Flex", 12, FontStyle.Bold);
                int nLocal = _configuracion.NumeroDeSucursal;
                float limiteIzquierdo = e.MarginBounds.Left;
                float limiteDerecho = e.MarginBounds.Right;
                string nombreNegocio = _nombreTienda.ToUpper(); ;
                string ticket = IdVenta;
                string direccionDelNegocio = DireccionTienda;
                string numeroTicket = TicketDeCompra;
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
                string tituloPrecioTotal = "TOTAL: ";
                SizeF anchoTituloSubTotal = e.Graphics.MeasureString(tituloSubTotal, fuenteInfoBold);
                SizeF anchoTituloDescuento = e.Graphics.MeasureString(tituloDescuento, fuenteInfoBold);
                SizeF anchoTituloPrecioTotal = e.Graphics.MeasureString(tituloPrecioTotal, fuenteInfoBold);
                // "N2" ESPECIFICA QUE TENDRA 2 0 DECIMALES
                string _strSubTotal = "$" + _subTotal.ToString("N2", new CultureInfo("es-AR"));
                string descuento = "$" + _descuento.ToString("N2", new CultureInfo("es-AR"));
                SizeF anchoSubTotal = e.Graphics.MeasureString(_strSubTotal, fuenteInfoBold);
                SizeF anchoDescuento = e.Graphics.MeasureString(descuento, fuenteInfoBold);
                string _strTotal = "$" + _total.ToString("N2", new CultureInfo("es-AR"));
                SizeF anchoPrecioTotal = e.Graphics.MeasureString(_strTotal, fuenteInfoBold);
                float xDetallesVenta = limiteDerecho - anchoTituloDescuento.Width - anchoSubTotal.Width;
                float xSubTotal = limiteDerecho - anchoSubTotal.Width;
                float xDescuento = limiteDerecho - anchoDescuento.Width;
                float xPrecioTotal = limiteDerecho - anchoPrecioTotal.Width;
                ///
                string tituloCajero = "Cajero: ";
                string tituloCaja = "Caja: ";
                string nombreCajero = Cajero;
                string numeroCaja = Caja;
                SizeF anchoCajero = e.Graphics.MeasureString(tituloCajero, fuenteInfoBold);
                SizeF anchoCaja = e.Graphics.MeasureString(tituloCaja, fuenteInfoBold);
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
                    string vCantidad = _listaProductos[_indiceActual].Cantidad.ToString() + $" {_listadoDeProductos[_indiceActual].UnidadMedida} " + "x";
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
                    RectangleF rectNombre = new RectangleF(limiteIzquierdo, y, 220, 100);
                    //Aca uso la sobre carag que usa un RectangleF para que si el nombre es muy largo haga "wrap"
                    e.Graphics.DrawString(vNombre, fuenteInfo, Brushes.Black, rectNombre);
                    //Aca se calcula tamaño de el string, y despues accedemos a el alto real que tiene
                    SizeF anchoNombreProducto = e.Graphics.MeasureString(vNombre, fuenteInfo, 220);
                    //Se le resta 15 para alinearlo
                    y += anchoNombreProducto.Height - 12;
                    e.Graphics.DrawString(strVPrecioTotal, fuenteInfo, Brushes.Black, posPrecioTotal, y);
                    y += 20;
                    _indiceActual++;
                    /*Esto lo que hace es copiar de vuelta la pagina ena vez pase el limite, no es necesario ya que el ticket es dinamico
                    if (y > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }*/
                }
                e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
                y += 20;
                e.Graphics.DrawString(tituloSubTotal, fuenteInfoBold, Brushes.Black, xDetallesVenta, y);
                e.Graphics.DrawString(_strSubTotal, fuenteInfo, Brushes.Black, xSubTotal, y);
                y += 15;
                e.Graphics.DrawString(tituloDescuento, fuenteInfoBold, Brushes.Black, xDetallesVenta, y);
                e.Graphics.DrawString(descuento, fuenteInfo, Brushes.Black, xDescuento, y);
                y += 15;
                e.Graphics.DrawString(tituloPrecioTotal, fuenteInfoBold, Brushes.Black, xDetallesVenta, y);
                e.Graphics.DrawString(_strTotal, fuenteInfo, Brushes.Black, xPrecioTotal, y);
                y += 20;
                e.Graphics.DrawString(divisor, fuenteDeco, Brushes.Black, divisorCentrado, y);
                y += 20;
                e.Graphics.DrawString(tituloFormaDePago, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
                e.Graphics.DrawString(_metodoDePago, fuenteInfo, Brushes.Black, xFormaDePago, y);
                y += 15;
                e.Graphics.DrawString(tituloMontoPagado, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
                e.Graphics.DrawString(MontoPagado, fuenteInfo, Brushes.Black, xMontoPagado, y);
                y += 15;
                e.Graphics.DrawString(tituloVuelto, fuenteInfoBold, Brushes.Black, limiteIzquierdo, y);
                e.Graphics.DrawString(Vuelto, fuenteInfo, Brushes.Black, xVuelto, y);
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
            catch (Exception ex) 
            {
                HistorialErrores error = new HistorialErrores(ex.Message, ex.GetType().FullName, ex.StackTrace, "Kiosco2026.ModeloDeLasVistas.Hijas", "ModeloFinalizarVenta", "ImpresionDePagina", _configuracion.NombreDeCaja);
                EscribirEnTXTError(error);
            }
        }
        // Cuando termina la impresión, mando el comando de corte
        private void FinDeImpresion(object sender, PrintEventArgs e)
        {
            // Avanza unas líneas para que quede espacio debajo del ticket
            byte[] feed = new byte[] { 0x1B, 0x64, 0x06 }; // 6? líneas CREO QUE LA TERCER COLUMNA ES EL PARAMETRO DE LAS LINEAS
            ComportamientoImpresora.SendBytesToPrinter(_impresora.NombreImpresora, feed);
            byte[] cut = new byte[] { 0x1D, 0x56, 0x00 }; // ESC/POS corte total
            ComportamientoImpresora.SendBytesToPrinter(_impresora.NombreImpresora, cut);
        }
        private async Task EjecutarGotFocus(object arg)
        {
            MontoPagado = "";
        }
        private async Task EjecutarLostFocus(object arg)
        {
            if (string.IsNullOrWhiteSpace(MontoPagado))
                MontoPagado = "0";
        }
        #endregion
    }
}
