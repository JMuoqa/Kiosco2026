using Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
using Kiosco2026.ModeloDeLasVistas.Hijas;

namespace Kiosco2026.Vistas.VENTAS
{
    public partial class FinalizarVenta : Window
    {
        public FinalizarVenta(Ventas venta)
        {
            InitializeComponent();
            this.DataContext = new ModeloFinalizarVenta(venta);
        }

    }

    
}