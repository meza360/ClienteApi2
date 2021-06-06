using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClienteApi2.Models;
using Newtonsoft.Json;
using ClienteApi2.ReferenciaServicioJava;

namespace ClienteApi2.Controllers
{
    public class MedicamentosController : Controller
    {
        private ServiciosClient proxy = new ServiciosClient();
      
        private string urlAPi = "http://localhost:62799/";
        //private string url2 = ReferenciaServicioJava.
        private string urlApiJava = "http://localhost:8080/web-app-java/Servicios";

        // Retorno de vistas 

        public ActionResult Index()
        {
            var listado = new List<SelectListItem>() {

                new  SelectListItem(){
                Text = "Codigo",
                Value = "1"
                },
                new SelectListItem(){
                Text = "Nombre",
                Value = "2"
                },
                new SelectListItem()
                {
                    Text = "Laboratorio",
                    Value = "3"
                }

            };

            ViewBag.Opciones = listado;
            return View();
        }

        //GET: Productos
        [HttpGet]
        public async Task<ActionResult> Productos()
        {
            List<medicamento> listado = new List<medicamento>();
            using (var alumnos = new HttpClient())
            {
                alumnos.BaseAddress = new Uri("http://localhost:8080/web-app-java/Servicios");
                alumnos.DefaultRequestHeaders.Clear();
                alumnos.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respuesta = await alumnos.GetAsync(proxy.listarMedicamentosAsync().ToString());
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = respuesta.Content.ReadAsStringAsync().Result;
                    listado = JsonConvert.DeserializeObject<List<medicamento>>(resultado);
                }
                
                return View(proxy.listarMedicamentos());
            }
        }

        //GET: busqueda por codigo
        [HttpGet]
        public ActionResult Busqueda(string selmed,string buscador)
        {
            int opcion = int.Parse(selmed);
            
            //int selMed = 1;
            Medicina med = new Medicina();
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.AutoFlush = true;
            Debug.Indent();
            Debug.WriteLine("El valor del metodo es: " + opcion);
            Debug.WriteLine("El valor del buscador es: " + buscador);
            Debug.Unindent();

            if (opcion == 1)
            {
                int argumento = int.Parse(buscador);
                var respuesta = proxy.listarcodigo(argumento);
                if (respuesta != null)
                {
                    med.Codigo = respuesta.codigo;
                    med.Medicamento = respuesta.medicamento1;
                    med.Dosis = respuesta.dosis;
                    med.Precio_unitario = respuesta.precio_unitario;
                    med.Cantidad_existencia = respuesta.cantidad_existencia;
                    med.Laboratorio_farmaceutico = respuesta.laboratorio_farmaceutico;
                    med.Vencimiento = respuesta.vencimiento;
                    med.Presentacion = respuesta.presentacion;
                    return View(respuesta);
                }
                else
                {
                    return Index();
                }
                
               
            }
            else if (opcion == 2)
            {
                string argumento = Convert.ToString(buscador);
                List<medicamento> listaMedicamentos = new List<medicamento>();
                List<Medicina> listaMedicinas = new List<Medicina>();
                Medicina b = new Medicina();
                listaMedicamentos = proxy.listarNombre(argumento).ToList();
                foreach (medicamento a in listaMedicamentos)
                {
                    b.Codigo = a.codigo;
                    b.Medicamento = a.medicamento1;
                    b.Dosis = a.dosis;
                    b.Precio_unitario = a.precio_unitario;
                    b.Cantidad_existencia = a.cantidad_existencia;
                    b.Laboratorio_farmaceutico = a.laboratorio_farmaceutico;
                    b.Vencimiento = a.vencimiento;
                    b.Presentacion = a.presentacion;
                    listaMedicinas.Add(b);
                }

                BusquedaParametros(argumento);

            }

            return View();
            
        }

        public ActionResult Busqueda()
        {
            return View();
        }

        //GET: busqueda por nombre
        [HttpGet]
        public ActionResult BusquedaParametros(string buscador)
        {
            string argumento = Convert.ToString(buscador);
            List<medicamento> listaMedicamentos = new List<medicamento>();
            List<Medicina> listaMedicinas = new List<Medicina>();
            Medicina b = new Medicina();
            listaMedicamentos = proxy.listarNombre(argumento).ToList();
            foreach (medicamento a in listaMedicamentos)
            {
                b.Codigo = a.codigo;
                b.Medicamento = a.medicamento1;
                b.Dosis = a.dosis;
                b.Precio_unitario = a.precio_unitario;
                b.Cantidad_existencia = a.cantidad_existencia;
                b.Laboratorio_farmaceutico = a.laboratorio_farmaceutico;
                b.Vencimiento = a.vencimiento;
                b.Presentacion = a.presentacion;
                listaMedicinas.Add(b);
            }

            return View();
        }



        public ActionResult Contacto()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetalleMedicamentos(int id)
        {
            medicamento med = proxy.listarcodigo(id);
            return View(med);
        }


        public ActionResult Facturacion()
        {
            return View();
        }
        
        public ActionResult Nosotros()
        {
            return View();
        }


        public ActionResult Sesiones()
        {
            return View();
        }

        public ActionResult Insercion()
        {
            return View();
        }




        
    }
}