using Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conexiones
{
    /*
    public class Con_Aut
    {
        public async Task<(bool estado, string mensaje, string datosUsuario)> Con_IniciarSesion(UsuarioMuoqa usuario)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var json = JsonSerializer.Serialize(new { Usuario = usuario.Usuario, Contraseña = usuario.Contrasena });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var respuesta = await client.PostAsync("https://muoqa.com/autenticacion/iniciar-desde-app", content);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        //Leemos la respuesta como un string
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        //La respuesta de tipo "string" la convertimos a json que contiene el token
                        var datos = JsonSerializer.Deserialize<EstadoAut>(resultado);
                        if (datos == null || (datos != null && !datos.estado))
                            return (false, datos.mensaje, "");
                        return (true, "exitoso", datos.Token);
                    }
                    else
                    {
                        //Si hay un error convertimos el string en json y leemos el error
                        string error = await respuesta.Content.ReadAsStringAsync();
                        var errorObj = JsonSerializer.Deserialize<EstadoAut>(error);
                        return (false, errorObj.mensaje, "");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return (false, ex.Message, "");
                }
            }
        }
    }*/
}
