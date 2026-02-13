using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kiosco2026.ModeloDeLasVistas
{
    public class ComandoModeloDeVista : ICommand
    {
        //Se cambio de Action<object> a -> Func<object, Task> para manejar metodos asincronos 
        private readonly Func<object, Task> _ejecutarAccion;
        private readonly Predicate<object> _posibilidadDeEjecucion;
        public ComandoModeloDeVista(Func<object, Task> ejecutarAccion)
        {
            _ejecutarAccion = ejecutarAccion;
            _posibilidadDeEjecucion = null;
        }
        public ComandoModeloDeVista(Func<object, Task> ejecutarAccion, Predicate<object> posibilidadDeEjecucion)
        {
            _ejecutarAccion = ejecutarAccion;
            _posibilidadDeEjecucion = posibilidadDeEjecucion;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _posibilidadDeEjecucion == null ? true : _posibilidadDeEjecucion(parameter);
        }

        public async void Execute(object? parameter)
        {
            try
            {
                await _ejecutarAccion(parameter);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
    }
}
