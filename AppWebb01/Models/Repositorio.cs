using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppWebb01.Models;

namespace AppWebb01.Models
{
    public class Repositorio
    {
        OdontoProEntities db = new OdontoProEntities();

        public List<mostrar_pacientes_Result> pacientes(string nombre) {

            List<mostrar_pacientes_Result> lista = new List<mostrar_pacientes_Result>();
            lista = db.mostrar_pacientes(nombre).ToList();
            return lista;
        }
    }
}