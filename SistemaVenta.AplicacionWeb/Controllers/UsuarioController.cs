﻿using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Newtonsoft;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.Entity;
using SistemaVenta.BLL.Interfaces;
using Newtonsoft.Json;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        private readonly IRolService _rolServicio;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioServicio, IRolService rolServicio, IMapper mapper)
        {
            _usuarioServicio= usuarioServicio;
            _rolServicio= rolServicio;
            _mapper=mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<VMRol> vmListaRoles= _mapper.Map<List<VMRol>>(await _rolServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, vmListaRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMUsuario> vmUsuarioLista = _mapper.Map<List<VMUsuario>>(await _usuarioServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data=vmUsuarioLista});
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                string nombreFoto = "";
                Stream fotoStream=null;

                if(foto!=null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo);
                    fotoStream = foto.OpenReadStream();
                }

                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/EnviarClave?correo=[correo]&clave=[clave]";

                Usuario usuario_creado = await _usuarioServicio.Crear(_mapper.Map<Usuario>(vmUsuario), fotoStream, nombreFoto, urlPlantillaCorreo);

                vmUsuario = _mapper.Map<VMUsuario>(usuario_creado);

                gResponse.Estado = true;
                gResponse.Objeto= vmUsuario;
            }
            catch (Exception e)
            {
                gResponse.Estado= false;
                gResponse.Mensaje = e.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo);
                    fotoStream = foto.OpenReadStream();
                }

                Usuario usuario_editado = await _usuarioServicio.Editar(_mapper.Map<Usuario>(vmUsuario), fotoStream, nombreFoto);

                vmUsuario = _mapper.Map<VMUsuario>(usuario_editado);

                gResponse.Estado = true;
                gResponse.Objeto = vmUsuario;
            }
            catch (Exception e)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = e.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdUsuario)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _usuarioServicio.Eliminar(IdUsuario);
            }
            catch (Exception e)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = e.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
