using FilmesAPI.Data.DTOS;
using FilmesAPI.Data;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesAPI.Controllers;

//As [] são anotações:
[ApiController] //API de acesso
[Route("[controller]")] //Rota de acesso do usuário a classe, tenho que passar o nome da classe dentro dos cochetes
public class FilmeController : ControllerBase
{
    //Criando um objeto de lista de filme estático


    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //Como estamos add informação a aplicação então a funcionalidade utilizada será Post

    //Metodo pronto para cadastrar filme no nosso sistema,
    //Se estamos cadastrando então o usuário precisa passar as informações para a API
    //[FromBody] -> as informções vem do corpo da requisição

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);

    }


    /// <summary>
    /// Exibe os filmes do banco de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a exibição seja feita</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50, [FromQuery] string? nomeCinema = null)
    {
        if(nomeCinema == null)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
        }

        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).Where(filme => filme.Sessoes.Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());



    }

    /// <summary>
    /// Exibe um filme em específico do banco de dados
    /// </summary>
    /// <param name="id">Para recuperar um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a exibição seja feita</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult? RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
            
    }

    /// <summary>
    /// Atualiza um filme no banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para atualizar um filme</param>
    /// <param name="id">Objeto necessário para diferenciar um filme dos outros</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso a atualização seja feita</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }


    /// <summary>
    /// Atualiza um filme parcialmente no banco de dados
    /// </summary>
    /// <param name="patch">Objeto com os campos necessários para atualizar um filme parcialmente</param>
    /// <param name="id">Objeto necessário para diferenciar um filme dos outros</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso a atualização seja feita</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var FilmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(FilmeParaAtualizar, ModelState);

        if (!TryValidateModel(FilmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(FilmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }


    /// <summary>
    /// Deleta um filme do banco de dados
    /// </summary>
    /// <param name="id">Objeto necessário para diferenciar um filme dos outros</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso a deleção seja feita</response>
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Filmes.Remove(filme);
        _context.SaveChanges();
        return NoContent();

    }
}