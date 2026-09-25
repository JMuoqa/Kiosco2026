using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosco2026.ModeloDeLasVistas
{
    public class ModeloDeVistaBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        string carpetaDatos = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Muoqa",
            "MuoqaStock"
        );
        public void EscribirEnTXTError(HistorialErrores ERROR)
        {
            string ruta = carpetaDatos+"\\log_error.txt";
            DateTime hoy = DateTime.Now;
            string logError = File.ReadAllText(ruta);
            logError += "{" + $"fecha:{hoy.ToString("dd-MM-yyyy HH:mm:ss")}," +
                $"mensaje: {ERROR.Mensaje}," +
                $"tipo_error: {ERROR.TipoError}," +
                $"stack_trace: {ERROR.StackTrace}," +
                $"namespace: {ERROR.NameSpace}," +
                $"clase: {ERROR.Clase}," +
                $"metodo: {ERROR.Metodo}," +
                $"usuario: {ERROR.Cajero}" + "}" + "\n\n";
            File.WriteAllText(ruta, logError);
        }
    }
}
