using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using apiUniversidade.Context;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiUniversidade.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/{v:apiversion}/curso")]
    public class CursoControllerv2 : ControllerBase
    {
    public readonly ILogger<CursoControllerv2> _logger;
    public readonly ApiUniversidadeContext _context;
    public CursoControllerv2(ILogger<CursoControllerv2> logger, ApiUniversidadeContext context)
    {
        _logger = logger;
        _context = context;
    }
    
        [HttpGet("geral")]
        public ActionResult<IEnumerable<Curso>> Get()
        {
            var cursos = _context.Cursos?.ToList();
            if(cursos is null)
                return NotFound();
            return cursos;
        }
        [HttpGet("(id:int)", Name ="GetCurso")]
        public ActionResult<Curso> Get(int id)
        {
            var curso = _context.Cursos?.FirstOrDefault(p => p.Id == id);
            if(curso is null)
                return NotFound("Curso não encontrado");
            return curso;
        }

        [HttpPost]
        public ActionResult Post(Curso curso){
            _context.Cursos?.Add(curso);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCurso",
            new{ id = curso.Id},
            curso);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Curso curso){
            if(id != curso.Id)
            return BadRequest();

            _context.Entry(curso).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(curso);
        }
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id){
            var curso = _context.Cursos?.FirstOrDefault(p => p.Id == id);
            if(curso is null)
                return NotFound();
            _context.Cursos?.Remove(curso);
            _context.SaveChanges();

            return Ok(curso);
        }
    }
}
