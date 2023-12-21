using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOS;


    public class CreateFilmeDto
    {
       

        [Required(ErrorMessage = "O titulo do filme é obrigatório")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O genêro do filme é obrigatório")]
        [StringLength(100, ErrorMessage = "o tamanho do gênero não pode exceder 100 caracteres")]
        public string Genero { get; set; }

        [Required]
        [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos")]
        public int Duracao { get; set; } 
    }

